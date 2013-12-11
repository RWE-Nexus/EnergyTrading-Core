namespace EnergyTrading.Container.Unity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.Configuration;

    using EnergyTrading.Logging;

    /// <summary>
    /// Helper methods for <see cref="UnityContainer" />s to simplify configuration and registration.
    /// </summary>
    public static class UnityExtensions
    {
        private static readonly ILogger Logger = LoggerFactory.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static Func<IEnumerable<UnityContainerExtension>> coreFunc = StandardCoreExtensions;
        private static Func<IEnumerable<UnityContainerExtension>> childFunc = StandardChildExtensions;

        /// <summary>
        /// Returns the configured core extensions.
        /// </summary>
        /// <returns>Enumeration of the core extensions.</returns>
        public static IEnumerable<UnityContainerExtension> CoreExtensions()
        {
            return coreFunc == null ? StandardCoreExtensions() : coreFunc();
        }

        /// <summary>
        /// Returns the configured child core extensions.
        /// </summary>
        /// <returns>Enumeration of the core extensions.</returns>
        public static IEnumerable<UnityContainerExtension> ChildCoreExtensions()
        {
            return childFunc == null ? StandardChildExtensions() : childFunc();
        }

        /// <summary>
        /// Set the set of standard core extensions for use by <see cref="InstallCoreExtensions" />.
        /// </summary>
        /// <param name="extensions"></param>
        public static void SetChildExtensions(Func<IEnumerable<UnityContainerExtension>> extensions)
        {
            childFunc = extensions;
        }

        /// <summary>
        /// Set the set of standard core extensions for use by <see cref="InstallCoreExtensions" />.
        /// </summary>
        /// <param name="extensions"></param>
        public static void SetCoreExtensions(Func<IEnumerable<UnityContainerExtension>> extensions)
        {
            coreFunc = extensions;
        }

        /// <summary>
        /// Checks whether an extension is configured before returning it.
        /// </summary>
        /// <typeparam name="TConfigurator">Unity extension to check</typeparam>
        /// <param name="container">Container to check in.</param>
        /// <returns>Unity extension if found.</returns>
        /// <exception cref="NotImplementedException">Raised if the extension is not installed in the container.</exception>
        public static TConfigurator CheckConfigure<TConfigurator>(this IUnityContainer container)
            where TConfigurator : UnityContainerExtension
        {
            var extension = container.Configure<TConfigurator>();
            if (extension == null)
            {
                throw new NotImplementedException("Required extension not registered: " + typeof(TConfigurator).Name);
            }

            return extension;
        }

        /// <summary>
        /// Standard set of extensions for child containers.
        /// <para>
        /// Currently
#pragma warning disable 612,618
        /// <see cref="UnityDefaultBehaviorExtension" />, <see cref="InjectedMembers" />
#pragma warning restore 612,618
        /// and <see cref="TypeTrackingExtension" />
        /// </para>
        /// </summary>
        /// <returns>Enumeration of the default extensions</returns>
        public static IEnumerable<UnityContainerExtension> StandardChildExtensions()
        {
            return new List<UnityContainerExtension>
            {
                new UnityDefaultBehaviorExtension(),
#pragma warning disable 612,618 // Marked as obsolete, but Unity still uses it internally.
                new InjectedMembers(),
#pragma warning restore 612,618
                new TypeTrackingExtension()
            };
        }

        /// <summary>
        /// Standard set of extensions for containers.
        /// <para>
        /// Currently
#pragma warning disable 612,618
        /// <see cref="UnityDefaultBehaviorExtension" />, <see cref="InjectedMembers" />
#pragma warning restore 612,618
        /// <see cref="UnityDefaultStrategiesExtension"/> and <see cref="TypeTrackingExtension" />
        /// </para>
        /// </summary>
        /// <returns>Enumeration of the default extensions</returns>
        public static IEnumerable<UnityContainerExtension> StandardCoreExtensions()
        {
            return new List<UnityContainerExtension>
            {
                new UnityDefaultBehaviorExtension(),
#pragma warning disable 612,618 // Marked as obsolete, but Unity still uses it internally.
                new InjectedMembers(),
#pragma warning restore 612,618
                new UnityDefaultStrategiesExtension(),
                new TypeTrackingExtension()
            };
        }

        /// <summary>
        /// Determines whether this type can be resolved as the default.
        /// </summary>
        /// <typeparam name="T">The type to test for resolution</typeparam>
        /// <param name="container">The unity container.</param>
        /// <returns>
        ///     <c>true</c> if this instance can resolve; otherwise, <c>false</c>.
        /// </returns>
        public static bool CanResolve<T>(this IUnityContainer container)
        {
            return container.CheckConfigure<TypeTrackingExtension>().CanResolve<T>();
        }

        /// <summary>
        /// Determines whether this type can be resolved with the specified name.
        /// </summary>
        /// <typeparam name="T">The type to test for resolution</typeparam>
        /// <param name="container">The unity container.</param>
        /// <param name="name">The name associated with the type.</param>
        /// <returns>
        ///     <c>true</c> if this instance can resolve; otherwise, <c>false</c>.
        /// </returns>
        public static bool CanResolve<T>(this IUnityContainer container, string name)
        {
            return container.CheckConfigure<TypeTrackingExtension>().CanResolve<T>(name);
        }

        /// <summary>
        /// Determines whether this instance can be resolved at all with or without a name.
        /// </summary>
        /// <typeparam name="T">The type to test for resolution</typeparam>
        /// <param name="container">The unity container.</param>
        /// <returns>
        ///     <c>true</c> if this instance can resolve; otherwise, <c>false</c>.
        /// </returns>
        public static bool CanResolveAny<T>(this IUnityContainer container)
        {
            return container.CheckConfigure<TypeTrackingExtension>().CanResolveAny<T>();
        }

        /// <summary>
        /// Tries to resolve the type, returning null if not found.
        /// </summary>
        /// <typeparam name="T">The type to try and resolve</typeparam>
        /// <param name="container">The unity container.</param>
        /// <returns>An object of type <see typeparamref="T"/> if found, or <c>null</c> if not.</returns>
        /// <remarks>Needs the <see cref="TypeTrackingExtension"/> installed.</remarks>
        public static T TryResolve<T>(this IUnityContainer container)
        {
            return container.CheckConfigure<TypeTrackingExtension>().TryResolve<T>();
        }

        /// <summary>
        /// Tries to resolve the type with the specified of name, returning null if not found.
        /// </summary>
        /// <typeparam name="T">The type to try and resolve</typeparam>
        /// <param name="container">The unity container.</param>
        /// <param name="name">The name associated with the type.</param>
        /// <returns>An object of type <see typeparamref="T"/> if found, or <c>null</c> if not.</returns>
        /// <remarks>Needs the <see cref="TypeTrackingExtension"/> installed.</remarks>        
        public static T TryResolve<T>(this IUnityContainer container, string name)
        {
            return container.CheckConfigure<TypeTrackingExtension>().TryResolve<T>(name);
        }

        /// <summary>
        /// Tries to resolve the type, returning the passed in defaultValue if not found.
        /// </summary>
        /// <typeparam name="T">The type to try and resolve</typeparam>
        /// <param name="container">The unity container.</param>
        /// <param name="name">The name associated with the type.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>An object of type <see typeparamref="T"/> if found, or the <see paramref="defaultValue"/> if not.</returns>
        /// <remarks>Needs the <see cref="TypeTrackingExtension"/> installed.</remarks>          
        public static T TryResolve<T>(this IUnityContainer container, T defaultValue, string name = null)
        {
            return container.CheckConfigure<TypeTrackingExtension>().TryResolve<T>(defaultValue, name);
        }

        /// <summary>
        /// Tries to resolve the type, returning the passed in defaultValue if not found.
        /// </summary>
        /// <typeparam name="T">The type to try and resolve</typeparam>
        /// <param name="container">The unity container.</param>
        /// <param name="name">The name associated with the type.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>An object of type <see typeparamref="T"/> if found, or the <see paramref="defaultValue"/> if not.</returns>
        /// <remarks>Needs the <see cref="TypeTrackingExtension"/> installed.</remarks>     
        [Obsolete("Use the TryResolve(container, T, string) overload")]     
        public static T TryResolve<T>(this IUnityContainer container, string name, T defaultValue)
        {
            return container.CheckConfigure<TypeTrackingExtension>().TryResolve<T>(defaultValue, name);
        }

        /// <summary>
        /// Tries to resolve the type, returning the passed in defaultValue if not found.
        /// </summary>
        /// <param name="type">The type to try and resolve</param>
        /// <param name="container">The unity container.</param>
        /// <param name="name">The name associated with the type.</param>
        /// <returns>An object of the type if found, or the <see paramref="defaultValue"/> if not.</returns>
        /// <remarks>Needs the <see cref="TypeTrackingExtension"/> installed.</remarks>          
        public static object TryResolve(this IUnityContainer container, Type type, string name)
        {
            return container.CheckConfigure<TypeTrackingExtension>().TryResolve(type, name);
        }

        /// <summary>
        /// Resolves all registered T in the container, conditionally including the default unnamed
        /// registered T. When includeDefault is false, this is the same as the normal Unity
        /// ResolveAll.
        /// </summary>
        /// <typeparam name="T">The type to resolve</typeparam>
        /// <param name="container">The unity container.</param>
        /// <param name="includeDefault">if set to <c>true</c> include default value, else do not include default.</param>
        /// <returns><see cref="IEnumerable{T}"/></returns>
        /// <remarks>Needs the <see cref="TypeTrackingExtension"/> installed.</remarks>          
        public static IEnumerable<T> ResolveAllToEnumerable<T>(this IUnityContainer container, bool includeDefault = true)
        {
            return container.CheckConfigure<TypeTrackingExtension>().ResolveAll<T>(includeDefault);
        }

        /// <summary>
        /// Resolves all registered T in the container, conditionally including the default unnamed
        /// registered T. When includeDefault is false, this is the same as the normal Unity
        /// ResolveAll.
        /// </summary>
        /// <param name="container">The unity container.</param>
        /// <param name="type">The type to resolve</param>
        /// <param name="includeDefault">if set to <c>true</c> include default value, else do not include default.</param>
        /// <returns><see cref="IEnumerable{T}"/></returns>
        /// <remarks>Needs the <see cref="TypeTrackingExtension"/> installed.</remarks>          
        public static IEnumerable<object> ResolveAllToEnumerable(this IUnityContainer container, Type type, bool includeDefault = true)
        {
            return container.CheckConfigure<TypeTrackingExtension>().ResolveAll(type, includeDefault);
        }

        /// <summary>
        /// Resolves all registered T in the container, conditionally including the default unnamed
        /// registered T. When includeDefault is false, this is the same as the normal Unity
        /// ResolveAll.
        /// </summary>
        /// <typeparam name="T">The type to resolve</typeparam>
        /// <param name="container">The unity container.</param>
        /// <param name="includeDefault">if set to <c>true</c> include default value, else do not include default.</param>
        /// <returns>Array of T</returns>
        /// <remarks>Needs the <see cref="TypeTrackingExtension"/> installed.</remarks>          
        public static T[] ResolveAllToArray<T>(this IUnityContainer container, bool includeDefault = true)
        {
            return container.CheckConfigure<TypeTrackingExtension>().ResolveAllToArray<T>(includeDefault);
        }

        /// <summary>
        /// Resolve the type from the container.
        /// </summary>
        /// <param name="container">Container to use.</param>
        public static T Resolve<T>(this IUnityContainer container)
        {
            return (T)container.Resolve(typeof(T));
        }

        /// <summary>
        /// Resolve the named typed from the container.
        /// </summary>
        /// <param name="container">Container to use.</param>
        /// <param name="name">Name of the registration</param>
        public static T Resolve<T>(this IUnityContainer container, string name)
        {
            return (T)container.Resolve(typeof(T), name);
        }

        /// <summary>
        /// Installs the core extensions and self-registers the service locator.
        /// </summary>
        /// <param name="container">Container to use.</param>
        public static void StandardConfiguration(this IUnityContainer container)
        {
            container.StandardConfiguration(false);
        }

        /// <summary>
        /// Installs the core extensions and self-registers the service locator.
        /// </summary>
        /// <param name="container">Container to use.</param>
        /// <param name="useTryResolve">Whether to use the type tracking extension or unity directly</param>
        public static void StandardConfiguration(this IUnityContainer container, bool useTryResolve)
        {
            // Tidies up the standard behaviour
            container.InstallCoreExtensions();

            // Locator self-registers, don't register again as it cause stackoverflow - http://unity.codeplex.com/workitem/10197
            new EnergyTradingUnityServiceLocator(container, useTryResolve);
        }

        /// <summary>
        /// Installs a pre-configured standard set of core extensions to the container after clearing any existing extensions.
        /// </summary>
        /// <remarks>
        /// Will also install <see cref="InitialisedExtension" /> so that we only run this once per container.
        /// </remarks>
        /// <param name="container">Container to use.</param>
        public static void InstallCoreExtensions(this IUnityContainer container)
        {
            if (container == null) { throw new ArgumentNullException("container"); }

            // Track if we've done this before.
            var initialised = container.Configure<InitialisedExtension>();
            if (initialised != null)
            {
                return;
            }

            container.RemoveAllExtensions();
            container.AddExtension(new InitialisedExtension());
            container.InstallExtensions(container.Parent == null ? CoreExtensions() : ChildCoreExtensions());
        }

        /// <summary>
        /// Installs extensions to the container.
        /// </summary>
        /// <param name="container">Container to modify</param>
        /// <param name="extensions">Extensions to install</param>
        public static void InstallExtensions(this IUnityContainer container, IEnumerable<UnityContainerExtension> extensions)
        {
            foreach (var extension in extensions)
            {
                container.AddExtension(extension);
            }
        }

        /// <summary>
        /// Configure any container registrars in the container
        /// </summary>
        /// <param name="container">Container to use.</param>
        public static void ConfigureRegistrars(this IUnityContainer container)
        {
            var registrars = container.ResolveAll<IContainerRegistrar>();
            foreach (var registrar in registrars)
            {
                registrar.Register(container);
            }
        }

        /// <summary>
        /// Creates a child container
        /// Loads from the config file
        /// Installs the core extensions.
        /// Initializes any registrars
        /// Registers in the parent 
        /// </summary>
        /// <param name="container">Container to use.</param>
        /// <param name="name">Name of the child container in the parent.</param>
        public static IUnityContainer ConfigureChildContainer(this IUnityContainer container, string name)
        {
            var child = container.CreateChildContainer();

            // NB Important to install extensions before loading.
            child.StandardConfiguration();

            // Register in the parent under the appropriate name.
            container.RegisterInstance(name, child);

            // Now do the core load/config - after service locator in case we need it in a registrar
            child.LoadConfiguration(name);
            child.ConfigureRegistrars();

            return child;
        }

        /// <summary>
        /// Replaces the Unity resolve with a safe version - only needed for Unity 5.1.505.0 and below.
        /// </summary>
        /// <param name="container">Container to use.</param>
        public static void ReplaceBehaviorExtensionsWithSafeExtension(this IUnityContainer container)
        {
            var extensionsField = container.GetType().GetField("extensions", BindingFlags.Instance | BindingFlags.NonPublic);
            if (extensionsField == null)
            {
                return;
            }

            var extensionsList = (List<UnityContainerExtension>)extensionsField.GetValue(container);
            var existingExtensions = extensionsList.ToArray();

            container.InstallCoreExtensions();

            var coreExtensions = CoreExtensions().ToList();
            foreach (var extension in existingExtensions.Where(extension => !coreExtensions.Contains(extension)))
            {
                container.AddExtension(extension);
            }
        }

        /// <summary>
        /// Attempt to verify a container by iterating over all registrations and attempting to resolve them
        /// </summary>
        /// <param name="container">Container to use.</param>
        public static void VerifyContainer(this IUnityContainer container)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Processing " + container.Registrations.Count() + " registrations");
            }

            foreach (var registration in container.Registrations.Where(registration => !registration.RegisteredType.ContainsGenericParameters))
            {
                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("Resolving: " + registration.RegisteredType.Name + " to " + registration.MappedToType.Name);
                }
                container.Resolve(registration.RegisteredType, registration.Name);
            }
        }
    }
}