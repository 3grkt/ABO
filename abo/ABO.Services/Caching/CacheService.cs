using ABO.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Web;

namespace ABO.Services.Caching
{
    public class CacheService : ICacheService
    {
        private IAppSettings _appSettings;

        private static MemoryCache _cache = MemoryCache.Default;

        public CacheService(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        #region ICacheService Members

        public T Get<T>(string key)
            where T : class
        {
            try
            {
                return (T)_cache[key];
            }
            catch
            {
                return null;
            }
        }

        public void Set<T>(string key, T value, int? cacheDuration = null, bool absoluteExpiration = false)
        {
            double cacheMinutes = (double)(cacheDuration ?? _appSettings.DefaultCacheDuration);
            if (absoluteExpiration)
            {
                _cache.Add(key, value, DateTimeOffset.Now.AddMinutes(cacheMinutes));
            }
            else
            {
                _cache.Add(key, value, new CacheItemPolicy { SlidingExpiration = TimeSpan.FromMinutes(cacheMinutes) });
            }
        }

        #endregion
    }
}
