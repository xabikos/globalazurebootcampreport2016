using System.Collections.Generic;
using System.Threading.Tasks;

namespace GlobalAzureBootcampReport.Data {
	public interface ITweetsRepository {
		Task<IEnumerable<Tweet>> GetLatestTweets(int minutesToRetrieve = 60);

		IEnumerable<Tweet> GetUserTweets(string userID);

		Task SaveTweet(Tweet tweet);

		void DeleteAllTables();
	}
}