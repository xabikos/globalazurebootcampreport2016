using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Core.Interfaces;

namespace GlobalAzureBootcampReport.Data.Impl
{
	internal class DocumentDbManager : IDocumentDbManager {
		private const string databaseName = "tweets";
		private const string collectionName = "timeline";

		private DocumentClient _client;

		public DocumentDbManager(IConfiguration configuration) {
			var documentDBEndpointUrl = configuration["DocumentDBEndpointUrl"];
			var documnetDBAuthoriazationKey = configuration["DocumentDBAuthorizationKey"];
			_client = new DocumentClient(new Uri(documentDBEndpointUrl), documnetDBAuthoriazationKey);
		}


		public async Task SaveTweet(ITweet tweet) {
			var userId = tweet.CreatedBy.IdStr;
			var user = tweet.CreatedBy;

			var db = (await _client.ReadDatabaseFeedAsync()).Single(d => d.Id == databaseName);
			var userTweetsCollection = (await _client.ReadDocumentCollectionFeedAsync(db.CollectionsLink)).Single(dc => dc.Id == collectionName);
			//if (userTweetsCollection == null) {
			//	// DocumentDB collections can be reserved with throughput specified in request units/second. 1 RU is a normalized request equivalent to the read
			//	// of a 1KB document.  Here we create a collection with 400 RU/s.
			//	userTweetsCollection = await _client.CreateDocumentCollectionAsync(
			//							UriFactory.CreateDatabaseUri(databaseName),
			//							new DocumentCollection { Id = userId },
			//							new RequestOptions { OfferThroughput = 800 });
			//}

			var savedTweet = await _client.CreateDocumentAsync(userTweetsCollection.DocumentsLink, new {
				id = tweet.IdStr,
				text = tweet.Text,
				creationTime = tweet.CreatedAt,
			});
		}
	}
}
