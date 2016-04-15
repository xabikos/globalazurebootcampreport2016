using System.Threading.Tasks;

namespace GlobalAzureBootcampReport.Data {
	/// <summary>
	/// Contract interface for accessing application's cache
	/// </summary>
	public interface ICache	{

		/// <summary>
		/// The key for the top users statistics entry
		/// </summary>
		string TopUsersStatsKey { get; }

		/// <summary>
		/// The key for all users statistics entry
		/// </summary>
		string AllUsersStatsKey { get; }

		/// <summary>
		/// Gets asynchronously the item with the supplied <paramref name="key"/> from the cache if exists.
		/// </summary>
		Task<T> GetItemAsync<T>(string key);

		/// <summary>
		/// Stores asynchronously the supplied <paramref name="data"/> object in the cache.
		/// </summary>
		Task SetItemAsync(string cacheKey, object data);
	}
}
