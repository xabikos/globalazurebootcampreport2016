﻿using GlobalAzureBootcampReport.Azure;
using GlobalAzureBootcampReport.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Core.Interfaces.Streaminvi;

namespace GlobalAzureBootcampReport.Data.Impl {
	internal class TwitterManager : ITwitterManager {

		private const int BatchSize = 5;
		private int _topUsersCounter;
		private readonly List<Tweet> _tweetsCache = new List<Tweet>();

		private IFilteredStream _stream;
		private readonly ITweetsRepository _tweetsRepository;
		private readonly ICache _cache;
		private readonly AzureHelper _azureHelper;

		public TwitterManager(ITweetsRepository tweetsRepository, ICache cache, AzureHelper azureHelper) {
			_tweetsRepository = tweetsRepository;
			_cache = cache;
			_azureHelper = azureHelper;
		}

		public async Task Connect() {

			// create the stream only once
			if (_stream == null || _stream.StreamState == Tweetinvi.Core.Enum.StreamState.Stop) {

				_stream = Stream.CreateFilteredStream();
				_stream.AddTrack("#mondaymotivation");

				_stream.MatchingTweetReceived += async (sender, args) => {
					Console.WriteLine("A tweet containing 'tweetinvi' has been found; the tweet is '" + args.Tweet + "'");
					Debug.WriteLine(args.Tweet.Text);
					var customTweet = args.Tweet.ToCustomTweet();
					await _tweetsRepository.SaveTweet(customTweet);
					UpdateTweetsAndClients(customTweet);
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

		private async Task UpdateStatisticsAndClients(Tweet tweet) {
			var allUsersStatistics = await _cache.GetItemAsync<IList<UserStat>>(_cache.AllUsersStatsKey);

			var userStats = allUsersStatistics.FirstOrDefault(us => us.UserId == tweet.CreatedBy.IdStr);
			// First tweet of the user
			if (userStats == null) {
				var userStat = new UserStat {
					UserId = tweet.CreatedBy.IdStr,
					TweetsNumber = 1,
					Name = tweet.CreatedBy.Name,
					ProfileUrl = "https://www.twitter.com/" + tweet.CreatedBy.ScreenName,
					Country = tweet.Place?.Country,
					ImageUrl = _azureHelper.GetUserProfileImagesContainerReference() + tweet.CreatedBy.IdStr
				};
				allUsersStatistics.Add(userStat);
			}
			else {
				userStats.TweetsNumber++;
			}
			_cache.SetItemAsync(_cache.AllUsersStatsKey,
				allUsersStatistics.Select(us => new { us.UserId, us.TweetsNumber })).Wait();

			var topUsersStatistics = _cache.GetItemAsync<IList<UserStat>>(_cache.TopUsersStatsKey).Result;

			var newTopUsersStatistics = allUsersStatistics.OrderByDescending(us => us.TweetsNumber).Take(15).ToList();

			if (topUsersStatistics.Count < 15 ||
				topUsersStatistics.Intersect(newTopUsersStatistics, new UserStatComparer()).Any()) {
				_topUsersCounter++;
				foreach (var userStat in newTopUsersStatistics) {
					var fullUserStat = topUsersStatistics.FirstOrDefault(us => us.UserId == userStat.UserId);
					if (fullUserStat != null) {
						userStat.Name = fullUserStat.Name;
						userStat.ProfileUrl = fullUserStat.ProfileUrl;
						userStat.Country = fullUserStat.Country;
						userStat.ImageUrl = fullUserStat.ImageUrl;
					}
					else {
						userStat.Name = tweet.CreatedBy.Name;
						userStat.ProfileUrl = "https://www.twitter.com/" + tweet.CreatedBy.ScreenName;
						userStat.Country = tweet.Place?.Country;
						userStat.ImageUrl = _azureHelper.GetUserProfileImagesContainerReference() + tweet.CreatedBy.IdStr;
					}
				}
				_cache.SetItemAsync(_cache.TopUsersStatsKey, newTopUsersStatistics).Wait();
			}

			if (_topUsersCounter == BatchSize) {
				_topUsersCounter = 0;
				// TODO Push tweets to clients
				//_context.Value.Clients.All.updateUsersStats(newTopUsersStatistics);
			}
		}

		private void UpdateTweetsAndClients(Tweet tweet) {
			_tweetsCache.Add(tweet);
			if (_tweetsCache.Count == BatchSize) {
				// TODO Push tweets to clients
				//_context.Value.Clients.All.addTweetsToList(_tweetsCache);
				_tweetsCache.Clear();
			}
		}
	}
}
