﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Ku.Core.Cache.Redis
{
    internal class RedisCacheProvider: ICacheProvider
    {
        /// <summary>
        /// The cache.
        /// </summary>
        private readonly IDatabase _cache;

        /// <summary>
        /// The db provider.
        /// </summary>
        private readonly IRedisDatabaseProvider _dbProvider;

        /// <summary>
        /// The servers.
        /// </summary>
        private readonly IEnumerable<IServer> _servers;

        /// <summary>
        /// The options.
        /// </summary>
        private readonly RedisOptions _options;

        public RedisCacheProvider(IRedisDatabaseProvider dbProvider, 
            IOptions<RedisOptions> options)
        {
            _dbProvider = dbProvider;
            _options = options.Value;
            _cache = _dbProvider.GetDatabase();
            _servers = _dbProvider.GetServerList();
        }

        public void Set(string key, object value, TimeSpan? expiration = null)
        {
            if (value == null)
            {
                //认定为删除该键
                Remove(key);
                return;
            }
            _cache.StringSet(key, JsonConvert.SerializeObject(value), expiration);
        }

        public async Task SetAsync(string key, object value, TimeSpan? expiration = null)
        {
            if (value == null)
            {
                //认定为删除该键
                await RemoveAsync(key);
                return;
            }
            await _cache.StringSetAsync(key, JsonConvert.SerializeObject(value), expiration);
        }

        public T Get<T>(string key)
        {
            var result = _cache.StringGet(key);
            if (result.HasValue)
            {
                return JsonConvert.DeserializeObject<T>(result);
            }
            return default(T);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var result = await _cache.StringGetAsync(key);
            if (result.HasValue)
            {
                return JsonConvert.DeserializeObject<T>(result);
            }
            return default(T);
        }

        public bool Exists(string key)
        {
            return _cache.KeyExists(key);
        }

        public async Task<bool> ExistsAsync(string key)
        {
            return await _cache.KeyExistsAsync(key);
        }

        public void Remove(string cacheKey)
        {
            _cache.KeyDelete(cacheKey);
        }

        public void Remove(params string[] cacheKeys)
        {
            var redisKeys = cacheKeys.Where(k => !string.IsNullOrEmpty(k)).Select(k => (RedisKey)k).ToArray();
            _cache.KeyDelete(redisKeys);
        }

        public async Task RemoveAsync(string cacheKey)
        {
            await _cache.KeyDeleteAsync(cacheKey);
        }

        public async Task RemoveAsync(params string[] cacheKeys)
        {
            var redisKeys = cacheKeys.Where(k => !string.IsNullOrEmpty(k)).Select(k => (RedisKey)k).ToArray();
            await _cache.KeyDeleteAsync(redisKeys);
        }

        /// <summary>
        /// Removes cached item by cachekey's prefix.
        /// </summary>
        /// <param name="prefix">Prefix of CacheKey.</param>
        public void RemoveByPrefix(string prefix)
        {
            prefix = this.HandlePrefix(prefix);
            var redisKeys = this.SearchRedisKeys(prefix);

            _cache.KeyDelete(redisKeys);
        }
        
        /// <summary>
        /// Removes cached item by cachekey's prefix async.
        /// </summary>
        /// <param name="prefix">Prefix of CacheKey.</param>
        public async Task RemoveByPrefixAsync(string prefix)
        {
            prefix = this.HandlePrefix(prefix);

            var redisKeys = this.SearchRedisKeys(prefix);

            await _cache.KeyDeleteAsync(redisKeys);
        }

        /// <summary>
        /// Searchs the redis keys.
        /// </summary>
        private RedisKey[] SearchRedisKeys(string pattern)
        {
            var keys = new List<RedisKey>();

            foreach (var server in _servers)
                keys.AddRange(server.Keys(pattern: pattern));

            return keys.Distinct().ToArray();
        }

        /// <summary>
        /// Handles the prefix of CacheKey.
        /// </summary>
        /// <param name="prefix">Prefix of CacheKey.</param>
        /// <exception cref="ArgumentException"></exception>
        private string HandlePrefix(string prefix)
        {
            // Forbid
            if (prefix.Equals("*"))
                throw new ArgumentException("the prefix should not equal to *");

            // Don't start with *
            prefix = new System.Text.RegularExpressions.Regex("^\\*+").Replace(prefix, "");

            // End with *
            if (!prefix.EndsWith("*", StringComparison.OrdinalIgnoreCase))
                prefix = string.Concat(prefix, "*");

            return prefix;
        }
    }
}
