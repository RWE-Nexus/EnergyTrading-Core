namespace EnergyTrading.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;

    public static class CollectionExtensions
    {
        public static DateTime? GetDateTime(this NameValueCollection collection, string name)
        {
            var value = collection[name];
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            return DateTime.Parse(value).ToUniversalTime();
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return collection == null || !collection.Any();
        }
    }
}