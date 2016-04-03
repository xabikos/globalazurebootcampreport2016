using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Interfaces;

namespace GlobalAzureBootcampReport.Data {
	public interface IDocumentDbManager
	{
		Task<IEnumerable<Tweet>> GetLatestTweets(int minutesToRetrieve);

		Task SaveTweet(Tweet tweet);
	}
}
