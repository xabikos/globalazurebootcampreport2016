using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Tweetinvi;
using GlobalAzureBootcampReport.Data;

namespace GlobalAzureBootcampReport.Controllers
{
	public class HomeController : Controller
	{
		private ITwitterManager _twitterManager;
		private ITweetsRepository _repo;

		public HomeController(ITwitterManager twitterManager, ITweetsRepository repo) {
			_twitterManager = twitterManager;
			_repo = repo;
		}

		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> About()
		{
			ViewData["Message"] = "Your application description page.";
			var latestTweetsCount = (await _repo.GetLatestTweets()).Count();
			var userTweets =_repo.GetUserTweets("1422966553").ToList();
			ViewBag.Count = latestTweetsCount;
			return View();
		}

		public IActionResult Contact()
		{
			ViewData["Message"] = "Your contact page.";
			var tweets = Timeline.GetHomeTimeline();
			ViewBag.Tweet = tweets.First().Text;

			return View();
		}

		public IActionResult Error()
		{
			return View();
		}

		public ContentResult Connect() {
			try {
				_twitterManager.Connect();
				return new ContentResult {
					Content = "Successfully connect"
				};
			}
			catch (Exception ex) {
				return new ContentResult {
					Content = $"Error during connect with message: {ex.Message}"
				};
			}
		}

		public ContentResult Disconnect() {
			try {
				_twitterManager.Disconnect();
				return new ContentResult {
					Content = "Successfully disconnect"
				};
			}
			catch (Exception ex) {
				return new ContentResult {
					Content = $"Error during disconnect with message: {ex.Message}"
				};
			}
		}

	}
}
