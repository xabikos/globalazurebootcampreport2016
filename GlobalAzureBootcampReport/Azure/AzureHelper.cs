using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;

namespace GlobalAzureBootcampReport.Azure {
	public class AzureHelper
	{
		private readonly CloudStorageAccount _account;

		public AzureHelper(IConfiguration configuration) {
			_account = CloudStorageAccount.Parse(configuration["StorageConnectionString"]);
		}

		public const string ImagesContainerName = "profileimages";
		public const string TimelineTableName = "timeline";

		public string GetUserProfileImagesContainerReference() {
			return _account.BlobStorageUri.PrimaryUri.ToString() + ImagesContainerName + "/";
		}

		public async Task<CloudTable> GetTableReference(string tableName) {
			var tableReference = _account.CreateCloudTableClient().GetTableReference(tableName);
			await tableReference.CreateIfNotExistsAsync();
			return tableReference;
		}

		public async Task<CloudBlobContainer> GetContainerReference(string containerName) {
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
