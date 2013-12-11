namespace EnergyTrading.Test
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public static class CheckerExtensions
    {
        public static void AutoCheck(this ICheckerCompare compare, Type type)
        {
            // Only get directly declared properties - parent will have own checker
            var properties = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

            compare.AutoCheck(properties);
        }

        public static void AutoCheck(this ICheckerCompare compare, IEnumerable<PropertyInfo> properties)
        {
            // Setup the comparisons
            foreach (var prop in properties)
            {
                // Exclude indexers
                var x = prop.GetIndexParameters();
                if (x.GetLength(0) != 0)
                {
                    continue;
                }

                compare.Compare(prop);
            }
        }
    }
}