using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;

namespace Demo {
	public class BlobManager {

		/// <summary>
		/// Uploads the same image twice to the blob storage
		/// </summary>
		public static void Execute() {
			var container = GetContainerReference("images");

			var imageByteArray = File.ReadAllBytes(Path.Combine("resources", "belgium.jpg"));
			var imageBlob = container.GetBlockBlobReference("belgium.jpg");

			imageBlob.Properties.ContentType = "image/jpeg";
			imageBlob.UploadFromByteArray(imageByteArray, 0, imageByteArray.Length);

			var imageInAFolderPageBlob = container.GetBlockBlobReference("countries\\belgium.jpg");
			imageInAFolderPageBlob.Properties.ContentType = "image/jpeg";
			imageInAFolderPageBlob.UploadFromByteArray(imageByteArray, 0, imageByteArray.Length);
		}

		/// <summary>
		/// Creates and returns the reference to the blob container to store images in
		/// </summary>
		private static CloudBlobContainer GetContainerReference(string containerName) {
			var blobClient = AzureHelper.CloudStorageAccount.CreateCloudBlobClient();
			var container = blobClient.GetContainerReference(containerName);
			container.CreateIfNotExists();
			container.SetPermissions(
				new BlobContainerPermissions {
					PublicAccess = BlobContainerPublicAccessType.Blob
				});
			return container;
		}
	}

}
