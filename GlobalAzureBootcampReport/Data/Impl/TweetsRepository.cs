using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Core.Interfaces;

namespace GlobalAzureBootcampReport.Data.Impl
{
	internal class TweetsRepository : ITweetsRepository {
		private IDocumentDbManager _documentDbManager;

		public TweetsRepository(IDocumentDbManager documentDbManager) {
			_documentDbManager = documentDbManager;
		}

		public async Task SaveTweet(ITweet tweet) {
			await _documentDbManager.SaveTweet(tweet);
		}

	}
}
