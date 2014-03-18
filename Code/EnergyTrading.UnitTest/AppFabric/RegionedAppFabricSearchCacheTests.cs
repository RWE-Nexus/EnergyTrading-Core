namespace EnergyTrading.UnitTest.AppFabric
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Caching;
    using System.Threading;

    using EnergyTrading.Caching;
    using EnergyTrading.Caching.AppFabric.Search;
    using EnergyTrading.Search;

    using Microsoft.ApplicationServer.Caching;

    using NUnit.Framework;

    [TestFixture]
    public class RegionedAppFabricSearchCacheTests
    {
        private static DataCache cache;

        private const string CacheName = "EnergyTradingCoreTesting";
        private const string RegionName = "RegionedAppFabricSearchCacheTestsRegion";

        [TestFixtureSetUp]
        public static void TestSetUp(TestContext context)
        {
            var config = new DataCacheFactoryConfiguration
            {
                Servers = new List<DataCacheServerEndpoint>
                {
                    new DataCacheServerEndpoint("C012A4700", 22233)
                }
            };
            var factory = new DataCacheFactory(config);

            cache = factory.GetCache(CacheName);
        }

        [TestFixtureTearDown]
        public void TestCleanUp()
        {
            cache.RemoveRegion(RegionName);
        }

        [Test]
        [Ignore]
        public void ShouldCreateRegionIfNotAlreadyThere()
        {
            var sut = new RegionedAppFabricSearchCache(cache, new AbsoluteCacheItemPolicyFactory(10), RegionName);

            var created = cache.CreateRegion(RegionName);

            Assert.IsFalse(created); // because the constructor should have already created the region
        }

        [Test]
        [Ignore]
        public void ShouldExpireAsPerPolicy()
        {
            var sut = new RegionedAppFabricSearchCache(cache, new AbsoluteCacheItemPolicyFactory(5), RegionName);

            var result = new SearchResult(new List<int> { 1, 2, 3 }, DateTime.Now, false, 0);

            sut.Add("bob", result);

            Thread.Sleep(TimeSpan.FromSeconds(8));

            var cached = sut.Get("bob", 1);

            Assert.IsNull(cached);
        }

        [Test]
        [Ignore]
        public void ShouldNotExpireIfPolicySaysInfinite()
        {
            var sut = new RegionedAppFabricSearchCache(cache, new AbsoluteCacheItemPolicyFactory(ObjectCache.InfiniteAbsoluteExpiration.Second), RegionName);

            var result = new SearchResult(new List<int> { 1, 2, 3 }, DateTime.Now, false, 0);

            sut.Add("bob", result);

            Thread.Sleep(TimeSpan.FromSeconds(10));

            var cached = sut.Get("bob", 1);

            Assert.IsNotNull(cached);
        }

        [Test]
        [Ignore]
        public void ShouldReturnCachedValue()
        {
            var sut = new RegionedAppFabricSearchCache(cache, new AbsoluteCacheItemPolicyFactory(15), RegionName);

            var result = new SearchResult(new List<int> { 1, 2, 3 }, DateTime.Now, false, 0);

            sut.Add("bob", result);

            Thread.Sleep(TimeSpan.FromSeconds(8));

            var cached = sut.Get("bob", 1);

            Assert.IsNotNull(cached);
        }

        [Test]
        [Ignore]
        public void ShouldSwallowKeyAlreadyExists()
        {
            var sut = new RegionedAppFabricSearchCache(cache, new AbsoluteCacheItemPolicyFactory(15), RegionName);

            var result = new SearchResult(new List<int> { 1, 2, 3 }, DateTime.Now, false, 0);

            sut.Add("bob", result);
            
            sut.Add("bob", result); // would throw DataCacheException(KeyAlreadyExists) usually

            Assert.IsTrue(true);
        }

        [Test]
        [Ignore]
        public void ShouldClearRegionOnCallingClear()
        {
            var sut = new RegionedAppFabricSearchCache(cache, new AbsoluteCacheItemPolicyFactory(150), RegionName);

            var result = new SearchResult(new List<int> { 1, 2, 3 }, DateTime.Now, false, 0);

            sut.Add("bob", result);

            sut.Clear();

            var cached = sut.Get("bob", 1);

            Assert.IsNull(cached);
        }
    }
}