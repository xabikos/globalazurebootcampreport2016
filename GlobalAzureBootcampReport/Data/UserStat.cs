using Newtonsoft.Json;
using System.Collections.Generic;

namespace GlobalAzureBootcampReport.Data {
	public class UserStat {
		[JsonProperty("userId")]
		public string UserId { get; set; }
		[JsonProperty("name")]
		public string Name { get; set; }
		[JsonProperty("profileUrl")]
		public string ProfileUrl { get; set; }
		[JsonProperty("tweetsNumber")]
		public int TweetsNumber { get; set; }
		[JsonProperty("country")]
		public string Country { get; set; }
		[JsonProperty("imageUrl")]
		public string ImageUrl { get; set; }
	}

	public class UserStatComparer : IEqualityComparer<UserStat> {
		public bool Equals(UserStat x, UserStat y) {
			//Check whether the compared objects reference the same data.
			if (ReferenceEquals(x, y)) return true;

			//Check whether any of the compared objects is null.
			if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
				return false;

			if (ReferenceEquals(x.UserId, null) || ReferenceEquals(y.UserId, null))
				return false;

			return x.UserId == y.UserId;
		}

		public int GetHashCode(UserStat obj) {
			if (ReferenceEquals(obj, null)) return 0;
			if (ReferenceEquals(obj.UserId, null)) return 0;
			return obj.UserId.GetHashCode();
		}
	}
}
