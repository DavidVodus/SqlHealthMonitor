using System;

namespace SqlHealthMonitor.BLL.Services
{
    public interface ICacheService
    {
        object Get(string key);
        void Add(string key, object value, TimeSpan expiration);
        /// <summary>
        /// Return value from cache if exist or execute  <paramref name="valueFactory"/>, stores value into cache and return it
        /// </summary>
        T GetOrAdd<T>(string key, TimeSpan expiration, Func<T> valueFactory)
            where T : class;
    }
}