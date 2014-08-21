using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using EnergyTrading.Caching;
using EnergyTrading.Caching.AppFabric.Cache;
using EnergyTrading.Caching.AppFabric.Registrar;
using EnergyTrading.Caching.InMemory;
using EnergyTrading.Caching.InMemory.Registrars;
using EnergyTrading.Configuration;
using EnergyTrading.UnitTest.Caching;
using Microsoft.ApplicationServer.Caching;
using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;

namespace EnergyTrading.UnitTest.AppFabric
{
    /// <summary>
    /// This class replaces AppFabric's DataCache with inmemory cache and tests the
    /// behaviour of AppFabricCacheService class
    /// </summary>
    [TestFixture]
    public class AppFabricCachingFixture : DefaultCacheServiceCachingFixture
    {
        private Mock<IDataCache> mockDataCache;

        [SetUp]
        public override void Setup()
        {
            container = new UnityContainer();
            mockDataCache = new Mock<IDataCache>();
            var inmemorycache = new InMemoryCacheService("xyz");
            var mockRepository = new Mock<ICacheRepository>();
            container.RegisterInstance(mockRepository.Object);

            var testAppfabricCacheCollection = new ConcurrentDictionary<string, TestAppFabricClass>();
            
            mockRepository.Setup(a => a.GetNamedCache(It.IsAny<string>())).Returns<string>(regionName =>
                      new AppFabricCacheService("Test", regionName, testAppfabricCacheCollection.GetOrAdd(regionName, (a) => new TestAppFabricClass())));
            mockRepository.Setup(a => a.ClearNamedCache(It.IsAny<string>())).Returns<string>(regionName =>
                                                                                             {
                                                                                                 testAppfabricCacheCollection[regionName].ClearCache();
                                                                                                 return true;
                                                                                             });

            mockDataCache.Setup(a => a.Get<string>(It.IsAny<string>())).Returns<string>(inmemorycache.Get<string>);
        }

        [Test]
        [TestCase("", "net.tcp://localhost:22233", ExpectedException = typeof(ConfigurationErrorsException))]
        [TestCase("", "", ExpectedException = typeof(ConfigurationErrorsException))]
        public void ShouldThrowErrorIfAppFabricSettingsAreMissing(string cacheName, string uri)
        {
            container = new UnityContainer();
            var appSettings = new NameValueCollection();
            appSettings["AppFabricCacheName"] = cacheName;
            appSettings["AppFabricUri"] = uri;
            var configManager = new Mock<IConfigurationManager>();
            configManager.Setup(x => x.AppSettings).Returns(appSettings);
            AppFabricCacheRegistrar.Register(container, configManager.Object);
        }

        [Test]
        public void ShouldResolveAppFabricInstance()
        {
            container = new UnityContainer();
            var appSettings = new NameValueCollection();
            appSettings["AppFabricCacheName"] = "Test";
            appSettings["AppFabricUri"] = "net.tcp://localhost:22233";
            var configManager = new Mock<IConfigurationManager>();
            configManager.Setup(x => x.AppSettings).Returns(appSettings);
            AppFabricCacheRegistrar.Register(container, configManager.Object);
            container.Resolve<ICacheRepository>();
        }

        [Ignore]
        public override void ShouldReturnSameCacheInstanceForSameCacheNames()
        {

        }

    }
}
