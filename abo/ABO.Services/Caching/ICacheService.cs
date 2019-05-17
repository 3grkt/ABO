using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABO.Services.Caching
{
    public interface ICacheService
    {
        T Get<T>(string key) where T : class;
        void Set<T>(string key, T value, int? cacheDuration = null, bool absoluteExpiration = false);
    }
}
