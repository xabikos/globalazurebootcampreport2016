using GlobalAzureBootcampReport.Helpers;
using Tweetinvi;

namespace GlobalAzureBootcampReport {
	public class TwitterConfig {
		public static void ConfigureTwitterClient() {

			Auth.SetUserCredentials(
				CredentialsHelper.TwitterConsumerKey,
				CredentialsHelper.TwitterConsumerSecter,
				CredentialsHelper.TwitterUserAccessToken,
				CredentialsHelper.TwitterUserAccessSecret
			);
		}
	}
}