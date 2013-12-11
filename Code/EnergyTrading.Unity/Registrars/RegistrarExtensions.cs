namespace EnergyTrading.Registrars
{
    using System.Xml.Linq;

    using Microsoft.Practices.Unity;

    using EnergyTrading.Mapping;

    /// <summary>
    /// Helpers for container registration.
    /// </summary>
    public static class RegistrarExtensions
    {
        /// <summary>
        /// Register a XML mapper both ways (to and from XML) against a container.
        /// </summary>
        /// <typeparam name="TEntity">Entity we are mapping</typeparam>
        /// <typeparam name="TMapper">The mapper.</typeparam>
        /// <param name="container">Unity container to register the mapper in.</param>
        /// <param name="name">Optional name to register the XML mapper against</param>
        public static void RegisterXmlMapper<TEntity, TMapper>(this IUnityContainer container, string name = null)
            where TMapper : XPathMapper<TEntity>, IXmlMapper<TEntity, XElement>
            where TEntity : class, new()
        {
            container.RegisterType<IXmlMapper<XPathProcessor, TEntity>, TMapper>(name);
            container.RegisterType<IXmlMapper<TEntity, XElement>, TMapper>(name);
        }
    }
}