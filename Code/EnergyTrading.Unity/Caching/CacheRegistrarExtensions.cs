namespace EnergyTrading.Caching
{
    using Microsoft.Practices.Unity;

    public static class CacheRegistrarExtensions
    {
        public static void RegisterAbsoluteCacheItemPolicyFactory(this IUnityContainer container, string key)
        {
            var policyFactory = new AbsoluteCacheItemPolicyFactory(key);

            container.RegisterInstance<ICacheItemPolicyFactory>(key, policyFactory);
        }
    }
}