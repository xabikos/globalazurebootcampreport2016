using GlobalAzureBootcampReport.Extensions;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Core.Interfaces.Streaminvi;

namespace GlobalAzureBootcampReport.Data.Impl {
	internal class TwitterManager : ITwitterManager {

		private IFilteredStream _stream;
		private ITweetsRepository _tweetsRepository;

		public TwitterManager(ITweetsRepository tweetsRepository) {
			_tweetsRepository = tweetsRepository;
		}

		public async Task Connect() {

			// create the stream only once
			if (_stream == null || _stream.StreamState == Tweetinvi.Core.Enum.StreamState.Stop) {

				_stream = Stream.CreateFilteredStream();
				_stream.AddTrack("Leicester");

				_stream.MatchingTweetReceived += async (sender, args) => {
					Console.WriteLine("A tweet containing 'tweetinvi' has been found; the tweet is '" + args.Tweet + "'");
					Debug.WriteLine(args.Tweet.Text);
					await _tweetsRepository.SaveTweet(args.Tweet.ToCustomTweet());
				};
				_stream.StreamStopped += (sender, args) => Task.Factory.StartNew(_stream.StartStreamMatchingAllConditionsAsync);

				await Task.Factory.StartNew(_stream.StartStreamMatchingAnyConditionAsync);
			}
		}

		public void Disconnect() {
			if (_stream != null) {
				_stream.StopStream();
			}
		}
	}
}
