namespace EnergyTrading.Container.Unity.AutoRegistration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// Extension methods to various types
    /// </summary>
    public static class AutoRegistrationExtensions
    {
        /// <summary>
        /// Gets type attribute.
        /// </summary>
        /// <typeparam name="TAttr">Type of the attribute.</typeparam>
        /// <param name="type">Target type.</param>
        /// <returns>Attribute value</returns>
        public static TAttr GetAttribute<TAttr>(this Type type)
            where TAttr : Attribute
        {
            return type.GetCustomAttributes(false).Single(a => typeof(TAttr) == a.GetType()) as TAttr;
        }

        /// <summary>
        /// Loads assembly from given assembly file name.
        /// </summary>
        /// <param name="autoRegistration">Auto registration.</param>
        /// <param name="assemblyPath">Assembly path.</param>
        /// <returns>Auto registration</returns>
        public static IAutoRegistration LoadAssemblyFrom(this IAutoRegistration autoRegistration, string assemblyPath)
        {
            Assembly.LoadFrom(assemblyPath);
            return autoRegistration;
        }

        /// <summary>
        /// Loads assemblies from given assembly file name.
        /// </summary>
        /// <param name="autoRegistration">Auto registration.</param>
        /// <param name="assemblyPaths">Assembly paths.</param>
        /// <returns>Auto registration</returns>
        public static IAutoRegistration LoadAssembliesFrom(this IAutoRegistration autoRegistration, IEnumerable<string> assemblyPaths)
        {
            foreach (var assemblyPath in assemblyPaths)
            {
                autoRegistration.LoadAssemblyFrom(assemblyPath);
            }

            return autoRegistration;
        }

        /// <summary>
        /// Configures auto registration - starts chain of fluent configuration
        /// </summary>
        /// <param name="container">Unity container.</param>
        /// <returns>Auto registration</returns>
        public static IAutoRegistration ConfigureAutoRegistration(this IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            return new AutoRegistration(container);
        }

        /// <summary>
        /// Adds rule to exclude certain assemblies (that name starts with System or mscorlib) 
        /// and not consider their types
        /// </summary>
        /// <returns>Auto registration</returns>
        public static IAutoRegistration ExcludeSystemAssemblies(this IAutoRegistration autoRegistration)
        {
            autoRegistration.ExcludeAssemblies(a => a.GetName().FullName.StartsWith("System") || 
                                                    a.GetName().FullName.StartsWith("mscorlib"));

            return autoRegistration;
        }      
    }
}
