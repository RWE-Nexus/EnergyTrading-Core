using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnergyTrading.Container.Unity;
using Microsoft.Practices.Unity;

namespace EnergyTrading.Caching.InMemory.Registrars
{
   public class InMemoryCacheRegistrar 
   {
        public static void Register(IUnityContainer container)
        {
            container.RegisterType<ICacheRepository, InMemoryCacheRepository>(new ContainerControlledLifetimeManager());
        }
    }
}
