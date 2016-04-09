using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using GlobalAzureBootcampReport.Data;

namespace GlobalAzureBootcampReport.Controllers {
	[Produces("application/json")]
	[Route("api/[controller]")]
	public class DataController : Controller {
		private ICache _cache;
		private ITweetsRepository _tweetsRepository;

		public DataController(ICache cache, ITweetsRepository tweetsRepository) {
			_cache = cache;
			_tweetsRepository = tweetsRepository;
		}

		[HttpGet("UsersStats")]
		public async Task<IEnumerable<UserStat>> GetUserStats()
		{
			var stats = await _cache.GetItemAsync<IEnumerable<UserStat>>(_cache.TopUsersStatsKey);
			if (stats != null) {
				stats = stats.OrderByDescending(us => us.TweetsNumber).Take(15);
				return stats;
			}
			return Enumerable.Empty<UserStat>();
		}

		[HttpGet("LatestTweets")]
		public Task<IEnumerable<Tweet>> GetLatestTweets() {
			return _tweetsRepository.GetLatestTweets(30);
		}

		[HttpGet("GetUserTweets")]
		public IEnumerable<Tweet> GetUserTweets(string userId) {
			return _tweetsRepository.GetUserTweets(userId);
		}

	}
}