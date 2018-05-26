
using System;

namespace CartApi.Caching
{
    public interface ICacheClient
    {
        void SetValue(string key, object value, TimeSpan expireIn);
        T GetValue<T>(string key);
    }
}