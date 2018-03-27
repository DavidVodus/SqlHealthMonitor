using System;
using System.Web;
using System.Web.Caching;
namespace SqlHealthMonitor.BLL.Services
{
    public class CacheService : ICacheService
    {
        public object Get(string key)
        {

            return HttpRuntime.Cache.Get(key);
        }

        public void Add(string key, object value, TimeSpan expiration)
        {
            HttpRuntime.Cache.Add(key, value, null, Cache.NoAbsoluteExpiration,
                expiration, CacheItemPriority.Default, null);
        }
        public T GetOrAdd<T>(string key, TimeSpan expiration, Func<T> valueFactory)
            where T : class
        {
            var cached = (T)Get(key);
            if (cached == null)
            {
                cached = valueFactory();
                Add(key, cached, expiration);
            }
            return cached;
        }
    }

}
