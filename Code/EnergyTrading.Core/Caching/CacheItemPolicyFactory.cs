namespace EnergyTrading.Caching
{
    using System;
    using System.Configuration;
    using System.Runtime.Caching;

    public static class CacheItemPolicyFactory
    {
        /// <summary>
        /// Return a sliding expiration policy for the name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static CacheItemPolicy SlidingPolicy(string name)
        {
            return new CacheItemPolicy
            {
                Priority = CacheItemPriority.Default,
                SlidingExpiration = TimeSpan.FromSeconds(SlidingExpiration(name))
            };
        }

        /// <summary>
        /// Return the sliding expiration in seconds
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int SlidingExpiration(string name)
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

        private static int GetValue(string name)
        {
            int result;
            return int.TryParse(ConfigurationManager.AppSettings[name], out result) ? result : 0;
        }
   }
}
