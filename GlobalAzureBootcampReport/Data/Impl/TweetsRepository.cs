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

namespace GlobalAzureBootcampReport.Data.Impl {
	internal class TweetsRepository : ITweetsRepository {
		private const string ImagesContainerName = "profileimages";
		private const string TweetsTableName = "tweets";

		private readonly IDocumentDbManager _documentDbManager;
		private readonly CloudStorageAccount _account;

		public TweetsRepository(IConfiguration configuration, IDocumentDbManager documentDbManager) {
			_documentDbManager = documentDbManager;
			_account = CloudStorageAccount.Parse(configuration["StorageConnectionString"]);
		}

		public Task<IEnumerable<Tweet>> GetLatestTweets(int minutesToRetrieve) {
			throw new NotImplementedException();
		}

		public async Task SaveTweet(Tweet tweet) {
			// Save the tweet to Document db for the generic timeline
			var saveToTimeline = _documentDbManager.SaveTweet(tweet);


			//Save the tweet to table storage for user
			var tweetEntity = new TweetEntity(tweet.CreatedBy.IdStr, tweet.Id) {
				User = tweet.CreatedBy.Name,
				ScreenName = tweet.CreatedBy.ScreenName,
				Text = tweet.Text,
				Country = tweet.Place?.Country
			};
			var userTable = await GetTableReference(TweetsTableName);
			var insertOperation = TableOperation.Insert(tweetEntity);
			var saveToUserTweets = userTable.ExecuteAsync(insertOperation);

			var saveUserImage = CheckForNewUserAndStoreImage(tweet);
			await Task.WhenAll(saveToTimeline, saveToUserTweets, saveUserImage);
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

			var container = await GetContainerReference(ImagesContainerName);
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
			var tableReference = _account.CreateCloudTableClient().GetTableReference($"user{tableName}");
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
