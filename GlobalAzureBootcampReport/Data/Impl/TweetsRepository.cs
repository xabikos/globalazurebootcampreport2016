using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Core.Interfaces;

namespace GlobalAzureBootcampReport.Data.Impl {
	public class TweetsRepository {

		private DocumentDbManager _documentDbManager;

		public TweetsRepository() {
			_documentDbManager = new DocumentDbManager();
		}

		public async Task SaveTweet(ITweet tweet) {
			await _documentDbManager.AddTweetToUser(tweet);
		}

	}
}
