using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnergyTrading.UnitTest.Container.Unity.AutoRegistration
{
    public class TestCache : ICache, IDisposable
    {
        public void Set(string key, object value)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
