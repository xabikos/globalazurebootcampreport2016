using GlobalAzureBootcampReport.Data.Impl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;

namespace GlobalAzureBootcampReport.Data {
	public class TwitterConnector {

		public static void Connect() {
			var stream = Stream.CreateFilteredStream();
			stream.AddTrack("#ElClasico");
			TweetsRepository repository = new TweetsRepository();
			stream.MatchingTweetReceived += (sender, args) => {
				Console.WriteLine("A tweet containing 'tweetinvi' has been found; the tweet is '" + args.Tweet + "'");

				repository.SaveTweet(args.Tweet).Wait();
			};
			Task.Factory.StartNew(stream.StartStreamMatchingAnyCondition);
		}

	}
}
