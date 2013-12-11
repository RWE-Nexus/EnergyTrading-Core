namespace EnergyTrading.Caching
{
    using System.Runtime.Caching;

    public interface ICacheItemPolicyFactory
    {
        CacheItemPolicy CreatePolicy();
    }
}