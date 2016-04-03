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

namespace GlobalAzureBootcampReport.Data.Impl {
	internal class TweetsRepository : ITweetsRepository {
		private const string ImagesContainerName = "profileimages";

		private readonly IDocumentDbManager _documentDbManager;
		private readonly CloudStorageAccount _account;

		public TweetsRepository(IConfiguration configuration, IDocumentDbManager documentDbManager) {
			_documentDbManager = documentDbManager;
			_account = CloudStorageAccount.Parse(configuration["StorageConnectionString"]);
		}

		public async Task SaveTweet(ITweet tweet) {
			// Save the tweet to Document db for the generic timeline
			var saveToTimeline = _documentDbManager.SaveTweet(tweet);

			// Save the tweet to table storage for user
			var tweetEntity = new TweetEntity(tweet.CreatedBy.IdStr, tweet.IdStr) {
				User = tweet.CreatedBy.Name,
				ScreenName = tweet.CreatedBy.ScreenName,
				Text = tweet.Text,
				Country = tweet.Place?.Country
			};
			var userTable = await GetTableReference(tweet.CreatedBy.IdStr);
			var insertOperation = TableOperation.Insert(tweetEntity);
			var saveToUserTweets = userTable.ExecuteAsync(insertOperation);

			var saveUserImage = CheckForNewUserAndStoreImage(tweet);
			await Task.WhenAll(saveToTimeline, saveToUserTweets, saveUserImage);
		}

		public async Task DeleteAllTables() {
			var tableClient = _account.CreateCloudTableClient();
			var tables = tableClient.ListTables().ToList();
			foreach (var table in tables)
				table.DeleteIfExists();
			await Task.FromResult(0);
		}

		private async Task CheckForNewUserAndStoreImage(ITweet tweet) {
			var user = tweet.CreatedBy;

			var container = await GetContainerReference(ImagesContainerName);
			var profileImageExists = await container.GetBlockBlobReference(user.IdStr).ExistsAsync();

			// Check if a new user
			if(!profileImageExists) {
				using (var client = new HttpClient()) {
					var profileImage = await client.GetByteArrayAsync(user.ProfileImageUrl);
					Debug.WriteLine($"image extension: {user.ProfileBackgroundImageUrl.Split('.').Last()}");

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
