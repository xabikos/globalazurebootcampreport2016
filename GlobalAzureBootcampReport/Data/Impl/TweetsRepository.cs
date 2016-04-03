using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;
using Tweetinvi.Core.Interfaces;
using System.Linq;

namespace GlobalAzureBootcampReport.Data.Impl {
	internal class TweetsRepository : ITweetsRepository {
		private readonly IDocumentDbManager _documentDbManager;
		private readonly CloudStorageAccount _account;

		public TweetsRepository(IConfiguration configuration, IDocumentDbManager documentDbManager) {
			_documentDbManager = documentDbManager;
			_account = CloudStorageAccount.Parse(configuration["StorageConnectionString"]);
		}

		public async Task SaveTweet(ITweet tweet) {
			await _documentDbManager.SaveTweet(tweet);

			var tweetEntity = new TweetEntity(tweet.CreatedBy.IdStr, tweet.IdStr) {
				User = tweet.CreatedBy.Name,
				ScreenName = tweet.CreatedBy.ScreenName,
				Text = tweet.Text,
				Country = tweet.Place?.Country
			};
			var userTable = await GetTableReference(tweet.CreatedBy.IdStr);
			var insertOperation = TableOperation.Insert(tweetEntity);
			await userTable.ExecuteAsync(insertOperation);
		}

		public async Task DeleteAllTables() {
			var tableClient = _account.CreateCloudTableClient();
			var tables = tableClient.ListTables().ToList();
			foreach (var table in tables)
				table.DeleteIfExists();
			await Task.FromResult(0);
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
