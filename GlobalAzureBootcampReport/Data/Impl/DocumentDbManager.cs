using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GlobalAzureBootcampReport.Data.Impl {
	internal class DocumentDbManager : IDocumentDbManager {
		private const string databaseName = "tweets";
		private const string collectionName = "userstweets";

		private DocumentClient _client;

		public DocumentDbManager(IConfiguration configuration) {
			var documentDBEndpointUrl = configuration["DocumentDBEndpointUrl"];
			var documnetDBAuthoriazationKey = configuration["DocumentDBAuthorizationKey"];
			_client = new DocumentClient(new Uri(documentDBEndpointUrl), documnetDBAuthoriazationKey);
		}

		public IEnumerable<Tweet> GetUserTweets(string userID) {
			return _client.CreateDocumentQuery<Tweet>(
				UriFactory.CreateDocumentCollectionUri(databaseName, collectionName))
				.Where(t => t.CreatedBy.IdStr == userID)
				.OrderByDescending(t => t.CreatedAt);

		}

		public async Task SaveUserTweet(Tweet tweet) {
			var userId = tweet.CreatedBy.IdStr;
			var user = tweet.CreatedBy;

			var db = (await _client.ReadDatabaseFeedAsync()).Single(d => d.Id == databaseName);
			var userTweetsCollection = (await _client.ReadDocumentCollectionFeedAsync(db.CollectionsLink)).Single(dc => dc.Id == collectionName);
			var savedTweet = await _client.CreateDocumentAsync(userTweetsCollection.DocumentsLink, tweet);
		}

	}

}
