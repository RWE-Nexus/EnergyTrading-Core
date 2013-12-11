namespace EnergyTrading.Container.Unity.AutoRegistration
{
    using System;
    using System.Linq;
    using System.Reflection;

    using EnergyTrading.Extensions;

    /// <summary>
    /// Boolean extension methods
    /// </summary>
    public static class If
    {
        /// <summary>
        /// Determines whether type is decorated with specified attribute
        /// </summary>
        /// <typeparam name="TAttr">Type of the attribute.</typeparam>
        /// <param name="type">Target type.</param>
        /// <returns>True if type is decorated with specified attribute, otherwise false</returns>
        public static bool DecoratedWith<TAttr>(this Type type)
            where TAttr : Attribute
        {
            return TypeExtensions.DecoratedWith<TAttr>(type);
        }

        /// <summary>
        /// Determines whether type implements specified interface
        /// </summary>
        /// <typeparam name="TContract">Type of the interface.</typeparam>
        /// <param name="type">Target type.</param>
        /// <returns>True if type implements specified interface, otherwise false</returns>
        public static bool Implements<TContract>(this Type type) where TContract : class
        {
            return TypeExtensions.Implements<TContract>(type);
        }

        /// <summary>
        /// Determines whether type implements interface that can be constructed from specified open-generic interface
        /// </summary>
        /// <param name="type">Target type.</param>
        /// <param name="contract">Type of the open-generic interface.</param>
        /// <returns>True if type implements interface that can be constructed from specified open-generic interface, otherwise false</returns>
        public static bool ImplementsOpenGeneric(this Type type, Type contract)
        {
            return TypeExtensions.ImplementsOpenGeneric(type, contract);
        }

        /// <summary>
        /// Determines whether type implements interface that name looks like ITypeName 
        /// (first letter is I, the rest is implementing type name),
        /// for example this method returns true for type that looks like - class Logger : ILogger
        /// </summary>
        /// <param name="type">Target type.</param>
        /// <returns>True if type implements ITypeName interface, otherwise false</returns>
        public static bool ImplementsITypeName(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return type.GetInterfaces().Any(i => i.Name.StartsWith("I")
                && i.Name.Remove(0, 1) == type.Name);
        }

        /// <summary>
        /// Determines whether type implements only one interface
        /// </summary>
        /// <param name="type">Target type.</param>
        /// <returns>True if type implements single interface, otherwise false</returns>
        public static bool ImplementsSingleInterface(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return type.GetInterfaces().Count() == 1;
        }

        /// <summary>
        /// Returns true for any type that is not null
        /// </summary>
        /// <param name="type">Target type.</param>
        /// <returns>Always returns true if type is not null</returns>
        public static bool Any(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return true;
        }

        /// <summary>
        /// Determines whether type passed as method argument is equal to type passed as generic method parameter.
        /// </summary>
        /// <typeparam name="T">Generic method parameter type</typeparam>
        /// <param name="type">Method argument type.</param>
        /// <returns>
        /// <c>true</c> if generic method parameter type is equal to method argument type; otherwise, <c>false</c>.
        /// </returns>
        public static bool Is<T>(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return type == typeof(T);
        }

        /// <summary>
        /// Determines whether type is assignable from specified type.
        /// </summary>
        /// <typeparam name="T">Generic method parameter type</typeparam>
        /// <param name="type">Method argument type.</param>
        /// <returns>
        /// <c>true</c> if is assignable from the specified type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAssignableFrom<T>(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return type.IsAssignableFrom(typeof(T));
        }

        /// <summary>
        /// Returns true for any assembly that is not null
        /// </summary>
        /// <param name="assembly">Target assembly.</param>
        /// <returns>Always returns true if assembly is not null</returns>
        public static bool AnyAssembly(this Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }

            return true;
        }

        /// <summary>
        /// Determines whether specified assembly contains specified type.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="assembly">Assembly.</param>
        /// <returns>
        /// <c>true</c> if the specified assembly contains type; otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsType<T>(this Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }

            return typeof(T).Assembly == assembly;
        }
    }
}