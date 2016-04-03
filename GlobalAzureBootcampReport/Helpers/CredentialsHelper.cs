using System.Configuration;

namespace GlobalAzureBootcampReport.Helpers {
	public static class CredentialsHelper {

		// Twitter credentials
		public static string TwitterConsumerKey = ConfigurationManager.AppSettings["TwitterConsumerKey"];
		public static string TwitterConsumerSecter = ConfigurationManager.AppSettings["TwitterConsumerSecret"];
		public static string TwitterUserAccessToken = ConfigurationManager.AppSettings["TwitterUserAccessToken"];
		public static string TwitterUserAccessSecret = ConfigurationManager.AppSettings["TwitterUserAccessSecret"];

		// DocumentDB credentials
		public static string DocumentDBEndpointUrl = ConfigurationManager.AppSettings["DocumentDBEndpointUrl"];
		public static string DocumentDBAuthorizationKey = ConfigurationManager.AppSettings["DocumentDBAuthorizationKey"];
	}
}
