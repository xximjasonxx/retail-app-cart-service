
using System;
using Newtonsoft.Json;
using ServiceStack.Redis;

namespace CartApi.Caching
{
    internal class RedisCacheClient : ICacheClient
    {
        private IRedisClient redisClient;

        public RedisCacheClient(string connectionString)
        {
            var manager = new RedisManagerPool(connectionString);
            this.redisClient = manager.GetClient();
        }

        ~RedisCacheClient()
        {
            if (redisClient != null)
            {
                redisClient.Dispose();
                redisClient = null;
            }
        }

        public void SetValue(string key, object value, TimeSpan expireIn)
        {
            this.redisClient.SetValue(key, JsonConvert.SerializeObject(value), expireIn);
        }

        public T GetValue<T>(string key)
        {
            if (!this.redisClient.ContainsKey(key))
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(this.redisClient.GetValue(key));
        }
    }
}