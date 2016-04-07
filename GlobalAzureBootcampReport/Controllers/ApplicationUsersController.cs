using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using GlobalAzureBootcampReport.Data;

namespace GlobalAzureBootcampReport.Controllers {
	[Produces("application/json")]
	[Route("api/statistics")]
	public class StatisticsController : Controller {
		private ICache _cache;

		public StatisticsController(ICache cache) {
			_cache = cache;
		}

		// GET: api/ApplicationUsers
		[HttpGet]
		public async Task<IEnumerable<UserStat>> GetApplicationUser()
		{
			var stats = await _cache.GetItemAsync<IEnumerable<UserStat>>(_cache.TopUsersStatsKey);
			stats = stats.OrderByDescending(us => us.TweetsNumber).Take(20);
			return stats;
		}

	}
}