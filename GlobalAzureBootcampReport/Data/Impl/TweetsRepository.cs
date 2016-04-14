using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using GlobalAzureBootcampReport.Helpers;
using System;
using System.Collections.Generic;
using GlobalAzureBootcampReport.Azure;

namespace GlobalAzureBootcampReport.Data.Impl {
	internal class TweetsRepository : ITweetsRepository {

		private readonly IDocumentDbManager _documentDbManager;
		private readonly AzureHelper _azureHelper;

		public TweetsRepository(IDocumentDbManager documentDbManager, AzureHelper azureHelper) {
			_documentDbManager = documentDbManager;
			_azureHelper = azureHelper;
		}

		public async Task<IEnumerable<Tweet>> GetLatestTweets(int minutesToRetrieve = 60) {
			var table = await _azureHelper.GetTableReference(AzureHelper.TimelineTableName);
			var durationToRetrieve = new DateTimeOffset(DateTime.UtcNow.AddMinutes(-minutesToRetrieve));
			var query =
				new TableQuery<TweetEntity>().Where(TableQuery.GenerateFilterConditionForDate("Timestamp",
					QueryComparisons.GreaterThan, durationToRetrieve)).Take(100);
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
			// Save the full tweet to Document db
			var saveUserTweet = _documentDbManager.SaveUserTweet(tweet);


			// Save the tweet to table storage for timeline
			var tweetEntity = new TweetEntity(tweet.CreatedBy.IdStr, tweet.Id) {
				User = tweet.CreatedBy.Name,
				ScreenName = tweet.CreatedBy.ScreenName,
				Text = tweet.Text,
				Country = tweet.Place?.Country,
			};
			var timelineTable = await _azureHelper.GetTableReference(AzureHelper.TimelineTableName);
			var insertOperation = TableOperation.Insert(tweetEntity);
			var saveToTimeline = timelineTable.ExecuteAsync(insertOperation);

			var saveUserImage = CheckForNewUserAndStoreImage(tweet);
			await Task.WhenAll(saveUserTweet, saveToTimeline, saveUserImage);
		}

		private async Task CheckForNewUserAndStoreImage(Tweet tweet) {
			var user = tweet.CreatedBy;

			var container = await _azureHelper.GetContainerReference(AzureHelper.ImagesContainerName);
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

	}
}
