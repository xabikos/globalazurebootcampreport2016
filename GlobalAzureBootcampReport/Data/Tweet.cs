using Newtonsoft.Json;
using System;
using Tweetinvi.Core.Enum;

namespace GlobalAzureBootcampReport.Data {

	/// <summary>
	/// Representing a tweet. This is also the POCO that is stored in the document DB
	/// </summary>
	public class Tweet {

		/// <summary>
		/// The Id of the Tweet
		/// </summary>
		[JsonProperty("id")]
		public string Id { get; set; }

		/// <summary>
		/// Creation date of the Tweet
		/// </summary>
		[JsonProperty("createdAt")]
		public DateTime CreatedAt { get; set; }

		/// <summary>
		/// Formatted text of the tweet
		/// </summary>
		[JsonProperty("text")]
		public string Text { get; set; }

		/// <summary>
		/// User who created the Tweet
		/// </summary>
		[JsonProperty("createdBy")]
		public User CreatedBy { get; set; }

		/// <summary>
		/// Number of retweets related with this tweet
		/// </summary>
		[JsonProperty("retweetCount")]
		public int RetweetCount { get; set; }

		/// <summary>
		/// Number of time the tweet has been favourited
		/// </summary>
		[JsonProperty("favoriteCount")]
		public int FavoriteCount { get; set; }

		/// <summary>
		/// Main language used in the tweet
		/// </summary>
		[JsonProperty("language")]
		public Language Language { get; set; }

		/// <summary>
		/// Geographic details concerning the location where the tweet has been published
		/// </summary>
		[JsonProperty("place")]
		public Place Place { get; set; }
	}

	public class Place {
		[JsonProperty("idStr")]
		public string IdStr { get; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("fullName")]
		public string FullName { get; set; }

		[JsonProperty("url")]
		public string Url { get; set; }

		[JsonProperty("placeType")]
		public PlaceType PlaceType { get; set; }

		[JsonProperty("country")]
		public string Country { get; set; }

		[JsonProperty("countryCode")]
		public string CountryCode { get; set; }
	}

	public class User {
		/// <summary>
		/// User Id as a string
		/// </summary>
		[JsonProperty("idStr")]
		public string IdStr { get; set; }

		/// <summary>
		/// User screen name
		/// </summary>
		[JsonProperty("screenName")]
		public string ScreenName { get; set; }

		/// <summary>
		/// The name of the user, as they’ve defined it.
		/// </summary>
		[JsonProperty("name")]
		public string Name { get; set; }

		/// <summary>
		/// Text describing the user account.
		/// </summary>
		[JsonProperty("description")]
		public string Description { get; set; }

		/// <summary>
		/// Date when the user account was created on Twitter.
		/// </summary>
		[JsonProperty("createdAt")]
		public DateTime CreatedAt { get; set; }

		/// <summary>
		/// The user-defined location for this account’s profile.
		/// </summary>
		[JsonProperty("location")]
		public string Location { get; set; }

		/// <summary>
		/// The URL of the profile image
		/// </summary>
		[JsonProperty("profileImageUrl")]
		public string ProfileImageUrl { get; set; }
	}
}
