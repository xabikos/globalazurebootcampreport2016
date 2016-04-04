using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Interfaces;

namespace GlobalAzureBootcampReport.Data {
	public interface IDocumentDbManager
	{
		IEnumerable<Tweet> GetUserTweets(string userID);
		Task SaveUserTweet(Tweet tweet);
	}
}
