namespace EnergyTrading.Mapping
{
    using System.Xml.Linq;

    /// <summary>
    /// Extensions for classes in EnergyTrading.Mapping
    /// </summary>
    public static class MappingExtensions
    {
        /// <summary>
        /// Registers a <see cref="XmlMapper{T}" /> against a <see cref="IXmlMappingEngine" />.
        /// </summary>
        /// <typeparam name="TEntity">Entity we are registering for</typeparam>
        /// <param name="engine">Engine to use</param>
        /// <param name="mapper">Mapper to use</param>
        public static void RegisterMapper<TEntity>(this IXmlMappingEngine engine, XmlMapper<TEntity> mapper)
            where TEntity : class, new()
        {
            engine.RegisterMap<XPathProcessor, TEntity>(mapper);
            engine.RegisterMap<TEntity, XElement>(mapper);
        }
    }
}