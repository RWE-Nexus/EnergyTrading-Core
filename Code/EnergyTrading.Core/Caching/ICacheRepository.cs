using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnergyTrading.Caching
{
   public interface ICacheRepository
    {
       ICacheService GetNamedCache(string cacheName);
       bool RemoveNamedCache(string cacheName);
       bool ClearNamedCache(string cacheName);
    }
}
