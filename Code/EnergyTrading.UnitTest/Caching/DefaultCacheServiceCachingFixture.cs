using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnergyTrading.Caching;
using EnergyTrading.Caching.InMemory.Registrars;
using Microsoft.Practices.Unity;
using NUnit.Framework;

namespace EnergyTrading.UnitTest.Caching
{
    [TestFixture]
   public class DefaultCacheServiceCachingFixture
    {
        protected IUnityContainer container;
        [SetUp]
        public virtual void Setup()
        {
            container=new UnityContainer();
            InMemoryCacheRegistrar.Register(container);
        }

        [Test]
        public void ShouldAbleToResolveInstanceAfterRegistration()
        {
           var inst1= container.Resolve<ICacheRepository>();
           var inst2 = container.Resolve<ICacheRepository>();
            Assert.AreEqual(inst1,inst2);
        }

        [Test]
        public virtual void ShouldReturnSameCacheInstanceForSameCacheNames()
        {
           var cacheRepo= container.Resolve<ICacheRepository>();
           Assert.AreEqual(cacheRepo.GetNamedCache("abc"),cacheRepo.GetNamedCache("abc"));
           Assert.AreEqual(cacheRepo.GetNamedCache("abc"), cacheRepo.GetNamedCache("abC"));
        }

        [Test]
        public void ShouldReturnDifferentCacheInstanceForDifferentCacheNames()
        {
            var cacheRepo = container.Resolve<ICacheRepository>();
            Assert.AreNotEqual(cacheRepo.GetNamedCache("abc"), cacheRepo.GetNamedCache("abd"));
        }

        [Test]
        public void ShouldNotOverlapItemsWithSameKeyInDifferentCache()
        {
            var cacheRepo = container.Resolve<ICacheRepository>();
            var cache1 = cacheRepo.GetNamedCache("a");
            var cache2= cacheRepo.GetNamedCache("b");
            cache1.Add("a","xyz");
            cache2.Add("a", "abc");

            Assert.AreNotEqual(cache1.Get<string>("a"), cache2.Get<string>("a"));
            Assert.AreEqual("xyz", cache1.Get<string>("a"));
            Assert.AreEqual("abc", cache2.Get<string>("a"));
        }

        [Test]
        public void ShouldRemoveCacheOnCallingRemoveApi()
        {
            var cacheRepo = container.Resolve<ICacheRepository>();
            var cache1 = cacheRepo.GetNamedCache("a");
            cacheRepo.RemoveNamedCache("a");
            var cache2= cacheRepo.GetNamedCache("a");
            Assert.AreNotEqual(cache1, cache2);
        }

        [Test]
        public void ShouldClearItemsOnCallingClearApi()
        {
            var cacheRepo = container.Resolve<ICacheRepository>();
            var cache1 = cacheRepo.GetNamedCache("a");
            var cache2 = cacheRepo.GetNamedCache("b");
            
            cache1.Add("a_key1","a");
            cache2.Add("b_key1", "b");

            cacheRepo.ClearNamedCache("b");

            var b_key1= cache2.Get<string>("b_key1");
            var a_key1 = cache1.Get<string>("a_key1");

            Assert.NotNull(a_key1);
            Assert.Null(b_key1);
        }
    }
}
