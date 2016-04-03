using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GlobalAzureBootcampReport.Data.Impl {
	internal class DocumentDbManager : IDocumentDbManager {
		private const string databaseName = "tweets";
		private const string collectionName = "timeline";

		private DocumentClient _client;

		public DocumentDbManager(IConfiguration configuration) {
			var documentDBEndpointUrl = configuration["DocumentDBEndpointUrl"];
			var documnetDBAuthoriazationKey = configuration["DocumentDBAuthorizationKey"];
			_client = new DocumentClient(new Uri(documentDBEndpointUrl), documnetDBAuthoriazationKey);
		}

		public Task<IEnumerable<Tweet>> GetLatestTweets(int minutesToRetrieve) {
			throw new NotImplementedException();
		}

		public async Task SaveTweet(Tweet tweet) {
			var userId = tweet.CreatedBy.IdStr;
			var user = tweet.CreatedBy;

			var db = (await _client.ReadDatabaseFeedAsync()).Single(d => d.Id == databaseName);
			var userTweetsCollection = (await _client.ReadDocumentCollectionFeedAsync(db.CollectionsLink)).Single(dc => dc.Id == collectionName);
			var savedTweet = await _client.CreateDocumentAsync(userTweetsCollection.DocumentsLink, tweet);
		}

	}

}
