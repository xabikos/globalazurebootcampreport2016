using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo {
	public class TableManager {
		public static void Execute() {

			var newTweet = CreateTweet();
			UpdateTweet(newTweet);
			DeleteTweet(newTweet);
		}

		/// <summary>
		/// Creates and stores a tweet in the table storage.
		/// </summary>
		private static Tweet CreateTweet() {
			var tweet = new Tweet("userId", "tweetId") {
				Text = "This is the text of the tweet",
				User = "xabikos"
			};

			var tweetsTable = GetTweetsTable();

			var insertOperation = TableOperation.Insert(tweet);
			var result = tweetsTable.Execute(insertOperation);
			return (Tweet)result.Result;
		}

		/// <summary>
		/// Updates the supplied <paramref name="tweet"/> in the table storaga
		/// </summary>
		private static void UpdateTweet(Tweet tweet) {
			tweet.Text = "This is the modified text of the tweet";

			var tweetsTable = GetTweetsTable();
			var updateOperation = TableOperation.Replace(tweet);
			var result = tweetsTable.Execute(updateOperation);
		}

		/// <summary>
		/// Deletes the supplied <paramref name="tweet"/> from the table storage.
		/// </summary>
		private static void DeleteTweet(Tweet tweet) {
			var tweetsTable = GetTweetsTable();
			var deleteOperation = TableOperation.Delete(tweet);
			var result = tweetsTable.Execute(deleteOperation);
		}

		/// <summary>
		/// Creates and returns a reference to the table storage
		/// </summary>
		private static CloudTable GetTweetsTable() {
			var tweetsTable = AzureHelper.CloudStorageAccount.CreateCloudTableClient().GetTableReference("tweets");
			tweetsTable.CreateIfNotExists();
			return tweetsTable;
		}

	}

	/// <summary>
	/// This is the entity that is stored in the table storage. It mush derive from <see cref="TableEntity"/>
	/// </summary>
	public class Tweet : TableEntity {
		public Tweet() { }

		public Tweet(string userId, string tweetId) {
			PartitionKey = userId;
			RowKey = tweetId;
		}

		public string UserId { get { return PartitionKey; } }
		public string TweetId { get { return RowKey; } }

		public string User { get; set; }

		public string Text { get; set; }

		public string CreatedAt { get { return Timestamp.DateTime.ToString("f"); } }
	}

}
