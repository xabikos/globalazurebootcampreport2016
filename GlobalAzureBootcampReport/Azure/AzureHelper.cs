using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalAzureBootcampReport.Azure
{
	public class AzureHelper
	{
		private readonly CloudStorageAccount _account;

		public AzureHelper(IConfiguration configuration) {
			_account = CloudStorageAccount.Parse(configuration["StorageConnectionString"]);
		}

		public const string ImagesContainerName = "profileimages";

		public string GetUserProfileImagesContainerReference() {
			return _account.BlobStorageUri.PrimaryUri.ToString() + ImagesContainerName + "/";
		}
	}
}
