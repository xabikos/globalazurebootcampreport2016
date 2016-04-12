using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo {
	public class BlobManager {

		public static void Execute() {
			UploadImage();
		}

		private static void UploadImage() {
			var container = GetContainerReference("images");


			var imageByteArray = File.ReadAllBytes(Path.Combine("resources","belgium.jpg"));
			var imageBlob = container.GetBlockBlobReference("belgium.jpg");

			imageBlob.Properties.ContentType = "image/jpeg";
			imageBlob.UploadFromByteArray(imageByteArray, 0, imageByteArray.Length);

			var imageInAFolderPageBlob = container.GetBlockBlobReference("countries\\belgium.jpg");
			imageInAFolderPageBlob.Properties.ContentType = "image/jpeg";
			imageInAFolderPageBlob.UploadFromByteArray(imageByteArray, 0, imageByteArray.Length);
		}


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
