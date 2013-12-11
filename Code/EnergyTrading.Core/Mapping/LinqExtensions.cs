namespace EnergyTrading.Mapping
{
    using System.Collections.Generic;
    using System.Xml.Linq;

    public static class LinqExtensions
    {
        /// <summary>
        /// Add multiple values to a XML container.
        /// </summary>
        /// <param name="container">Container to use</param>
        /// <param name="values">Values to add</param>
        public static void Add(this XContainer container, IEnumerable<object> values)
        {
            foreach (var value in values)
            {
                container.Add(value);
            }
        }
    }
}