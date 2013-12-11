namespace EnergyTrading.Container.Unity.AutoRegistration
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Microsoft.Practices.Unity;

    /// <summary>
    /// Auto Registration contract
    /// </summary>
    public interface IAutoRegistration
    {
        /// <summary>
        /// Adds rule to include certain types that satisfy specified type filter 
        /// and register them using specified registrator function
        /// </summary>
        /// <param name="typeFilter">Type filter.</param>
        /// <param name="registrator">Registrator function.</param>
        /// <returns>Auto registration</returns>
        IAutoRegistration Include(Predicate<Type> typeFilter, Action<Type, IUnityContainer> registrator);

        /// <summary>
        /// Adds rule to include certain types that satisfy specified type filter 
        /// and register them using specified registration options
        /// </summary>
        /// <param name="typeFilter">Type filter.</param>
        /// <param name="registrationOptions">RegistrationOptions options.</param>
        /// <returns>Auto registration</returns>
        IAutoRegistration Include(Predicate<Type> typeFilter, IRegistrationOptions registrationOptions);

        /// <summary>
        /// Adds rule to exclude certain types that satisfy specified type filter and not register them
        /// </summary>
        /// <param name="filter">Type filter.</param>
        /// <returns>Auto registration</returns>
        IAutoRegistration Exclude(Predicate<Type> filter);

        /// <summary>
        /// Adds rule to exclude certain assemblies that satisfy specified assembly filter 
        /// and not consider their types
        /// </summary>
        /// <param name="filter">Type filter.</param>
        /// <returns>Auto registration</returns>
        IAutoRegistration ExcludeAssemblies(Predicate<Assembly> filter);

        /// <summary>
        /// Applies auto registration - scans loaded assemblies, 
        /// check specified rules and register types that satisfy these rules
        /// </summary>
        void ApplyAutoRegistration();

        /// <summary>
        /// Applies auto registration for the specified assembiles, 
        /// check specified rules and register types that satisfy these rules
        /// </summary>
        void ApplyAutoRegistration(params Assembly[] assemlies);
    }
}