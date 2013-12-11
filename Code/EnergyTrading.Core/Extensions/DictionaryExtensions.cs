namespace EnergyTrading.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    public static class DictionaryExtensions
    {
        public static IDictionary<TKey, TValue> MergeLeft<TKey, TValue>(this IDictionary<TKey, TValue> primary, bool overwriteDuplicates, params IDictionary<TKey, TValue>[] secondaries)
        {
            return secondaries.Aggregate(primary, (current, dict) => current.Merge(dict, overwriteDuplicates));
        }

        public static IDictionary<TKey, TValue> Merge<TKey, TValue>(this IDictionary<TKey, TValue> primary, IDictionary<TKey, TValue> secondary, bool overwriteDuplicates = false)
        {
            if (primary == null)
            {
                return null;
            }

            if (secondary == null)
            {
                return primary;
            }

            foreach (var kvp in secondary)
            {
                if (!primary.ContainsKey(kvp.Key))
                {
                    primary.Add(kvp);
                }
                else if (overwriteDuplicates)
                {
                    primary[kvp.Key] = kvp.Value;
                }
            }

            return primary;
        }
    }
}