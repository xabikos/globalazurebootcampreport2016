using GlobalAzureBootcampReport.Helpers;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Core.Interfaces;

namespace GlobalAzureBootcampReport.Data {

	public class DocumentDbManager {

		private const string DatabaseName = "tweets";

		private DocumentClient _client;

		public DocumentDbManager() {

		}

		protected DocumentClient Client {
			get
			{
				if (_client == null) {
					_client = new DocumentClient(
						new Uri(CredentialsHelper.DocumentDBEndpointUrl),
						CredentialsHelper.DocumentDBAuthorizationKey
					);
				}
				return _client;
			}
		}

		public async Task CreateDatabase(string name) {
			await Client.CreateDatabaseAsync(new Database { Id = name});
		}


		public async Task AddTweetToUser(ITweet tweet) {
			var userId = tweet.CreatedBy.IdStr;
			var user = tweet.CreatedBy;

			var db = (await Client.ReadDatabaseFeedAsync()).Single(d => d.Id == DatabaseName);
			var userTweetsCollection = (await Client.ReadDocumentCollectionFeedAsync(db.CollectionsLink)).SingleOrDefault(dc => dc.Id == userId);
			if (userTweetsCollection == null) {
				// DocumentDB collections can be reserved with throughput specified in request units/second. 1 RU is a normalized request equivalent to the read
				// of a 1KB document.  Here we create a collection with 400 RU/s.
				userTweetsCollection = await Client.CreateDocumentCollectionAsync(
										UriFactory.CreateDatabaseUri(DatabaseName),
										new DocumentCollection { Id = userId},
										new RequestOptions { OfferThroughput = 400 });
			}

			var savedTweet = await Client.CreateDocumentAsync(userTweetsCollection.DocumentsLink, new { id = tweet.IdStr, text = tweet.Text });
		}

	}
}
