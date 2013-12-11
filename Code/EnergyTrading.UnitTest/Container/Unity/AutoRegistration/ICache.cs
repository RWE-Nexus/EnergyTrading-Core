using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnergyTrading.UnitTest.Container.Unity.AutoRegistration
{
    public interface ICache
    {
        void Set(string key, object value);
    }
}
