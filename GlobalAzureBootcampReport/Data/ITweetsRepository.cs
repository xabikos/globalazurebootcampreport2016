using System.Collections.Generic;
using System.Threading.Tasks;

namespace GlobalAzureBootcampReport.Data {
	public interface ITweetsRepository {
		Task<IEnumerable<Tweet>> GetLatestTweets(int minutesToRetrieve);

		Task SaveTweet(Tweet tweet);

		void DeleteAllTables();
	}
}