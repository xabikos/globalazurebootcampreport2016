using Microsoft.AspNet.SignalR;
using System.Collections.Generic;

namespace GlobalAzureBootcampReport.Data {
	public class BootcampReportHub : Hub {

		public void UpdateUsersStats(IEnumerable<UserStat> usersStats) {
			Clients.All.updateUsersStats(usersStats);
		}

		public void AddTweetsToList(IEnumerable<Tweet> newTweets) {
			Clients.All.addTweetsToList(newTweets);
		}
	}
}
