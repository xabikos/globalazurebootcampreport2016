using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tweetinvi;
using Tweetinvi.Core.Parameters;

namespace GlobalAzureBootcampReport.Controllers {
	public class HomeController : Controller {
		public ActionResult Index() {
			return View();
		}

		public ActionResult About() {
			ViewBag.Message = "Your application description page.";
			var tweets = Timeline.GetHomeTimeline();
			ViewBag.Tweet = tweets.First().Text;
			return View();
		}

		public ActionResult Contact() {
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}