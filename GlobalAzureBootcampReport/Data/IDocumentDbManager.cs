using System.Collections.Generic;
using System.Threading.Tasks;

namespace GlobalAzureBootcampReport.Data {

	/// <summary>
	/// Contract interface for accessing the document DB
	/// </summary>
	public interface IDocumentDbManager {

		/// <summary>
		/// Retrieves and returns a list with all the tweets for the user with the supplied <paramref name="userID"/>
		/// </summary>
		IEnumerable<Tweet> GetUserTweets(string userID);

		/// <summary>
		/// Saves the tweet to the document DB
		/// </summary>
		Task SaveUserTweet(Tweet tweet);
	}
}
