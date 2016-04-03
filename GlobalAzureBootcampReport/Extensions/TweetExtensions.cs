using GlobalAzureBootcampReport.Data;
using Tweetinvi.Core.Interfaces;

namespace GlobalAzureBootcampReport.Extensions {
	public static class TweetExtensions
	{
		public static Tweet ToCustomTweet(this ITweet tweet) {
			var result = new Tweet {
				Id = tweet.IdStr,
				Text = tweet.Text,
				CreatedAt = tweet.CreatedAt,
				RetweetCount = tweet.RetweetCount,
				FavoriteCount = tweet.FavoriteCount,
				CreatedBy = new User {
					IdStr = tweet.CreatedBy.IdStr,
					Description = tweet.CreatedBy.Description,
					CreatedAt = tweet.CreatedBy.CreatedAt,
					Name = tweet.CreatedBy.Name,
					ScreenName = tweet.CreatedBy.ScreenName,
					Location = tweet.CreatedBy.Location,
					ProfileImageUrl = tweet.CreatedBy.ProfileImageUrl
				},
				Language = tweet.Language
			};
			if (tweet.Coordinates != null) {
				result.Coordinates = new Coordinates {
					Latitude = tweet.Coordinates.Latitude,
					Longitude = tweet.Coordinates.Longitude
				};
			}
			if(tweet.Place != null) {
				result.Place = new Place {
					Name = tweet.Place.Name,
					FullName = tweet.Place.FullName,
					Country = tweet.Place.Country,
					CountryCode = tweet.Place.CountryCode,
					PlaceType = tweet.Place.PlaceType,
					Url = tweet.Place.Url
				};
			}

			return result;
		}

	}
}
