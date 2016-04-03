using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalAzureBootcampReport.Data
{
	public class TweetEntity : TableEntity {
		public TweetEntity() { }

		public TweetEntity(string partitionKey, string rowKey) {
			PartitionKey = partitionKey;
			RowKey = rowKey;
		}

		public string UserId { get { return PartitionKey; } }
		public string TweetId { get { return RowKey; } }

		public string User { get; set; }

		public string ScreenName { get; set; }

		public string Text { get; set; }

		public string Country { get; set; }

		public string CreatedAt { get { return Timestamp.DateTime.ToString("f"); } }
	}
}
