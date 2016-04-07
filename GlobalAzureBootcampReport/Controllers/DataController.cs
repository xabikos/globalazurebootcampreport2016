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
			stats = stats.OrderByDescending(us => us.TweetsNumber).Take(20);
			return stats;
		}

		[HttpGet("LatestTweets")]
		public Task<IEnumerable<Tweet>> GetLatestTweets() {
			return _tweetsRepository.GetLatestTweets(30);
		}

	}
}