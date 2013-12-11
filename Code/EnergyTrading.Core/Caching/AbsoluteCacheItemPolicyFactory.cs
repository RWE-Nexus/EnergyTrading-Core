namespace EnergyTrading.Caching
{
    using System;
    using System.Runtime.Caching;

    using EnergyTrading.Configuration;

    public class AbsoluteCacheItemPolicyFactory : ICacheItemPolicyFactory
    {
        private readonly IConfigurationManager configurationManager;
        private readonly int absoluteExpirationTimeInSeconds;

        public AbsoluteCacheItemPolicyFactory(string policyKey) : this(policyKey, new AppConfigConfigurationManager())
        {
        }

        public AbsoluteCacheItemPolicyFactory(string policyKey, IConfigurationManager configurationManager)
        {
            this.configurationManager = configurationManager;
            this.absoluteExpirationTimeInSeconds = this.AbsoluteExpiration(policyKey);
        }

        public AbsoluteCacheItemPolicyFactory(int absoluteExpirationTimeInSeconds)
        {
            this.absoluteExpirationTimeInSeconds = absoluteExpirationTimeInSeconds;
        }

        /// <summary>
        /// Return a absolute expiration policy
        /// </summary>
        /// <returns></returns>
        public CacheItemPolicy CreatePolicy()
        {
            return new CacheItemPolicy
            {
                Priority = CacheItemPriority.Default,
                AbsoluteExpiration = DateTime.Now.AddSeconds(this.absoluteExpirationTimeInSeconds)
            };
        }

        /// <summary>
        /// Return the absolute expiration in seconds
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private int AbsoluteExpiration(string name)
        {
            const string Key = "CacheItemPolicy.Expiration";

            var result = GetValue(Key + "." + name);
            if (result != 0)
            {
                return result;
            }

            result = GetValue(Key);
            return result != 0 ? result : 1;
        }

        private int GetValue(string name)
        {
            int result;
            return int.TryParse(configurationManager.AppSettings[name], out result) ? result : 0;
        }
    }
}