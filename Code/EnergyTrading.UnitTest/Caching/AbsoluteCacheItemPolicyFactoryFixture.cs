namespace EnergyTrading.UnitTest.Caching
{
    using System;
    using System.Collections.Specialized;
    using System.Runtime.Caching;
    using System.Threading;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using EnergyTrading.Caching;
    using EnergyTrading.Configuration;

    [TestClass]
    public class AbsoluteCacheItemPolicyFactoryFixture
    {
        [TestMethod]
        public void ShouldCreateAbsoluteCacheItemPolicyBasedOnTheConfiguration()
        {
            const string PolicyKey = "MDM.Market";

            var appSettings = new NameValueCollection();
            appSettings["CacheItemPolicy.Expiration." + PolicyKey] = "8";

            var configManager = new Mock<IConfigurationManager>();
            configManager.Setup(x => x.AppSettings).Returns(appSettings);

            ICacheItemPolicyFactory policyFactory = new AbsoluteCacheItemPolicyFactory(PolicyKey, configManager.Object);
            var policyItem = policyFactory.CreatePolicy();

            var marketName = "ABC market";
            var marketKey = "Market-1";
            var cache = new MemoryCache("MDM.Market");
            cache.Add(marketKey, marketName, policyItem);

            // Should get cache item 
            Assert.AreEqual(marketName, cache[marketKey]);

            // wait until the expiry time
            Thread.Sleep(TimeSpan.FromSeconds(10));

            // should not be in the cache
            Assert.IsNull(cache[marketKey]);
        }

        [TestMethod]
        public void ShouldExpireCacheAfterConfigurableTime()
        {
            const string PolicyKey = "MDM.Market";

            var appSettings = new NameValueCollection();
            appSettings["CacheItemPolicy.Expiration." + PolicyKey] = "8";

            var configManager = new Mock<IConfigurationManager>();
            configManager.Setup(x => x.AppSettings).Returns(appSettings);

            ICacheItemPolicyFactory policyFactory = new AbsoluteCacheItemPolicyFactory(PolicyKey, configManager.Object);
            var policyItem = policyFactory.CreatePolicy();

            var marketName = "ABC market";
            var marketKey = "Market-1";
            var cache = new MemoryCache("MDM.Market");
            cache.Add(marketKey, marketName, policyItem);

            // Should get cache item 
            Assert.AreEqual(marketName, cache[marketKey]);

            // Keep on accessing cache, it should expire approximately with in 10 iterations
            int count = 0;
            while (cache[marketKey] != null && count < 10)
            {
                count++;
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
            
            Console.WriteLine("Cache has expired in {0} seconds:", count);
            // should not be in the cache after configuratble time
            Assert.IsNull(cache[marketKey]);
        }
    }
}