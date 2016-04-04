using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;
using Tweetinvi.Core.Interfaces;
using System.Linq;
using System.Net.Http;
using System.IO;
using System.Diagnostics;
using GlobalAzureBootcampReport.Helpers;
using System;
using System.Collections.Generic;
using GlobalAzureBootcampReport.Azure;

namespace GlobalAzureBootcampReport.Data.Impl {
	internal class TweetsRepository : ITweetsRepository {

		private const string TimelineTableName = "timeline";

		private readonly IDocumentDbManager _documentDbManager;
		private readonly AzureHelper _azureHelper;
		private readonly CloudStorageAccount _account;

		public TweetsRepository(IConfiguration configuration, IDocumentDbManager documentDbManager, AzureHelper azureHelper) {
			_documentDbManager = documentDbManager;
			_azureHelper = azureHelper;
			_account = CloudStorageAccount.Parse(configuration["StorageConnectionString"]);
		}

		public async Task<IEnumerable<Tweet>> GetLatestTweets(int minutesToRetrieve = 60) {
			var table = await GetTableReference(TimelineTableName);
			var oneHourAgoTimestap = new DateTimeOffset(DateTime.UtcNow.AddMinutes(-minutesToRetrieve));
			var query =
				new TableQuery<TweetEntity>().Where(TableQuery.GenerateFilterConditionForDate("Timestamp",
					QueryComparisons.GreaterThan, oneHourAgoTimestap));
			return table.ExecuteQuery(query).Select(t => new Tweet {
				Id = t.RowKey,
				CreatedBy = new User { IdStr = t.PartitionKey, Name = t.User, ScreenName = t.ScreenName},
				CreatedAt = DateTime.Parse(t.CreatedAt),
				Text = t.Text
			});
		}

		public IEnumerable<Tweet> GetUserTweets(string userID) {
			return _documentDbManager.GetUserTweets(userID);
		}

		public async Task SaveTweet(Tweet tweet) {
			// Save the tweet to Document db for the generic timeline
			var saveUserTweet = _documentDbManager.SaveUserTweet(tweet);


			//Save the tweet to table storage for user
			var tweetEntity = new TweetEntity(tweet.CreatedBy.IdStr, tweet.Id) {
				User = tweet.CreatedBy.Name,
				ScreenName = tweet.CreatedBy.ScreenName,
				Text = tweet.Text,
				Country = tweet.Place?.Country,
			};
			var timelineTable = await GetTableReference(TimelineTableName);
			var insertOperation = TableOperation.Insert(tweetEntity);
			var saveToTimeline = timelineTable.ExecuteAsync(insertOperation);

			var saveUserImage = CheckForNewUserAndStoreImage(tweet);
			await Task.WhenAll(saveUserTweet, saveUserImage);
		}

		public void DeleteAllTables() {
			var tableClient = _account.CreateCloudTableClient();
			var tables = tableClient.ListTables().ToList();
			Parallel.ForEach(tables, (table) => table.DeleteIfExists());
			//foreach (var table in tables)
			//	table.DeleteIfExists();
		}

		private async Task CheckForNewUserAndStoreImage(Tweet tweet) {
			var user = tweet.CreatedBy;

			var container = await GetContainerReference(AzureHelper.ImagesContainerName);
			var profileImageExists = await container.GetBlockBlobReference(user.IdStr).ExistsAsync();

			// Check if a new user
			if(!profileImageExists) {
				using (var client = new HttpClient()) {
					var profileImage = await client.GetByteArrayAsync(user.ProfileImageUrl);

					var blob = container.GetBlockBlobReference(user.IdStr);
					blob.Properties.ContentType = ContentTypeHelper.GetContentType(user.ProfileImageUrl);
					await blob.UploadFromByteArrayAsync(profileImage, 0, profileImage.Length);
				}
			}
		}

		private async Task<CloudTable> GetTableReference(string tableName) {
			var tableReference = _account.CreateCloudTableClient().GetTableReference(tableName);
			await tableReference.CreateIfNotExistsAsync();
			return tableReference;
		}

		private async Task<CloudBlobContainer> GetContainerReference(string containerName) {
			var blobClient = _account.CreateCloudBlobClient();
			var container = blobClient.GetContainerReference(containerName);
			await container.CreateIfNotExistsAsync();
			container.SetPermissions(
				new BlobContainerPermissions {
					PublicAccess =
						BlobContainerPublicAccessType.Blob
				});
			return container;
		}

	}
}
