using System.Collections.Generic;
using System.Threading.Tasks;

namespace GlobalAzureBootcampReport.Data {

	/// <summary>
	/// Contract interface for accessing tweets in various storages
	/// </summary>
	public interface ITweetsRepository {

		/// <summary>
		/// Returns all the latest tweets based on the supplied <paramref name="minutesToRetrieve"/> time period
		/// </summary>
		Task<IEnumerable<Tweet>> GetLatestTweets(int minutesToRetrieve = 60);

		/// <summary>
		/// Retrieves and returns a list with all the tweets for the user with the supplied <paramref name="userID"/>
		/// </summary>
		IEnumerable<Tweet> GetUserTweets(string userID);

		/// <summary>
		/// Save the tweet to the various data storages
		/// </summary>
		Task SaveTweet(Tweet tweet);
	}
}