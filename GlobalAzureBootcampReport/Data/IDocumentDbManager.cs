using System.Threading.Tasks;
using Tweetinvi.Core.Interfaces;

namespace GlobalAzureBootcampReport.Data {
	public interface IDocumentDbManager
	{
		Task SaveTweet(ITweet tweet);
	}
}
