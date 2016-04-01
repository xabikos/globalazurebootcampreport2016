using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Tweetinvi;
using Tweetinvi.Core.Credentials;

namespace GlobalAzureBootcampReport {
	public class TwitterConfig {
		public static void ConfigureTwitterClient() {
			var consumerKey = ConfigurationManager.AppSettings["ConsumerKey"];
			var consumerSecret = ConfigurationManager.AppSettings["ConsumerSecret"];
			var userAccessToken = ConfigurationManager.AppSettings["UserAccessToken"];
			var userAccessSecret = ConfigurationManager.AppSettings["UserAccessSecret"];

			Auth.SetUserCredentials(consumerKey, consumerSecret, userAccessToken, userAccessSecret);
			//var creds = new TwitterCredentials(consumerKey, consumerSecret, userAccessToken, userAccessSecret);
			//var authenticatedUser = User.GetAuthenticatedUser(creds);
			//var friends = authenticatedUser.GetFriends();
		}
	}
}