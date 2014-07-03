using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;

namespace EnergyTrading.Caching
{
    public interface ICacheService
    {
        void Remove(string key);
        void Add<T>(string key, T value, CacheItemPolicy policy=null);
        T Get<T>(string key);
    }
}
