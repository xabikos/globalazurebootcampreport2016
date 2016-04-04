using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalAzureBootcampReport.Data.Impl
{
	internal class Cache : ICache
	{
		private static string RedisConnectionString;
		private static readonly Lazy<ConnectionMultiplexer> Connection = new Lazy<ConnectionMultiplexer>(() =>
			ConnectionMultiplexer.Connect(RedisConnectionString));

		public Cache(IConfiguration configuration) {
			RedisConnectionString = configuration["RedisConnectionString"];
		}

		private static ConnectionMultiplexer RedisConnection
		{
			get
			{
				return Connection.Value;
			}
		}

		public string TopUsersStatsKey
		{
			get { return "TopUsersStatsKey"; }
		}

		public string AllUsersStatsKey
		{
			get { return "AllUsersStatsKey"; }
		}

		public async Task<T> GetItemAsync<T>(string key) {
			var db = RedisConnection.GetDatabase();
			var cacheValue = await db.StringGetAsync(key).ConfigureAwait(false);
			if (String.IsNullOrEmpty(cacheValue)) return default(T);

			var data = JsonConvert.DeserializeObject<T>(cacheValue);

			return data;
		}

		public async Task SetItemAsync(string key, object data) {
			var cacheValue = JsonConvert.SerializeObject(data);

			// update value in redis
			var db = RedisConnection.GetDatabase();
			await db.StringSetAsync(key, cacheValue).ConfigureAwait(false);
		}
	}
}
