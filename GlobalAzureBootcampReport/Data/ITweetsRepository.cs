using System.Threading.Tasks;
using Tweetinvi.Core.Interfaces;

namespace GlobalAzureBootcampReport.Data {
	public interface ITweetsRepository {
		Task SaveTweet(ITweet tweet);

		Task DeleteAllTables();
	}
}