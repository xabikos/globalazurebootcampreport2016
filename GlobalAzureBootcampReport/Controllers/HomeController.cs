using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Tweetinvi;
using GlobalAzureBootcampReport.Data;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace GlobalAzureBootcampReport.Controllers {
	public class HomeController : Controller
	{
		private readonly ITwitterManager _twitterManager;
		private readonly ITweetsRepository _repo;
		private readonly string _twitterConnectionKey;

		public HomeController(ITwitterManager twitterManager, ITweetsRepository repo, IConnectionManager connectionManager, IConfiguration configuration) {
			_twitterManager = twitterManager;
			_repo = repo;
			_twitterConnectionKey = configuration["TwitterConnectionKey"];
		}

		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> About()
		{
			ViewData["Message"] = "Your application description page.";
			var latestTweetsCount = (await _repo.GetLatestTweets()).Count();
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

		public IActionResult Connect(string id) {
			if(id != _twitterConnectionKey) {
				return new HttpNotFoundResult();
			}
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

		public IActionResult Disconnect(string id) {
			if (id != _twitterConnectionKey) {
				return new HttpNotFoundResult();
			}
			try {
				_twitterManager.Pause();
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

		public IActionResult Reconnect(string id) {
			if (id != _twitterConnectionKey) {
				return new HttpNotFoundResult();
			}
			try {
				_twitterManager.Resume();
				return new ContentResult {
					Content = "Successfully resumed"
				};
			}
			catch (Exception ex) {
				return new ContentResult {
					Content = $"Error during resumed with message: {ex.Message}"
				};
			}
		}

	}
}
