namespace EnergyTrading.Container.Unity.AutoRegistration
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;

    using Microsoft.Practices.Unity;

    /// <summary>
    /// Auto Registration extends popular Unity IoC container 
    /// and provides nice fluent syntax to configure rules for automatic types registration
    /// </summary>
    public class AutoRegistration : IAutoRegistration
    {
        private readonly List<RegistrationEntry> registrationEntries;
        private readonly List<Predicate<Assembly>> excludedAssemblyFilters;
        private readonly List<Predicate<Type>> excludedTypeFilters;
        private readonly IUnityContainer container;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoRegistration"/> class.
        /// </summary>
        /// <param name="container">Unity container.</param>
        public AutoRegistration(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            this.container = container;
            this.registrationEntries = new List<RegistrationEntry>();
            this.excludedAssemblyFilters = new List<Predicate<Assembly>>();
            this.excludedTypeFilters = new List<Predicate<Type>>();
        }

        /// <summary>
        /// Adds rule to include certain types that satisfy specified type filter
        /// and register them using specified registrator function
        /// </summary>
        /// <param name="typeFilter">Type filter.</param>
        /// <param name="registrator">Registrator function.</param>
        /// <returns>Auto registration</returns>
        public virtual IAutoRegistration Include(Predicate<Type> typeFilter, Action<Type, IUnityContainer> registrator)
        {
            if (typeFilter == null)
            {
                throw new ArgumentNullException("typeFilter");
            }
            if (registrator == null)
            {
                throw new ArgumentNullException("registrator");
            }

            registrationEntries.Add(new RegistrationEntry(typeFilter, registrator, container));
            return this;
        }

        /// <summary>
        /// Adds rule to include certain types that satisfy specified type filter
        /// and register them using specified registration options
        /// </summary>
        /// <param name="typeFilter">Type filter.</param>
        /// <param name="registrationOptions">RegistrationOptions options.</param>
        /// <returns>Auto registration</returns>
        public virtual IAutoRegistration Include(Predicate<Type> typeFilter, IRegistrationOptions registrationOptions)
        {
            if (typeFilter == null)
            {
                throw new ArgumentNullException("typeFilter");
            }

            if (registrationOptions == null)
            {
                throw new ArgumentNullException("registrationOptions");
            }

            registrationEntries.Add(new RegistrationEntry(
                                         typeFilter,
                                         (t, c) =>
                                         {
                                             registrationOptions.Type = t;
                                             foreach (var contract in registrationOptions.Interfaces)
                                             {
                                                 c.RegisterType(
                                                     contract,
                                                     t,
                                                     registrationOptions.Name,
                                                     registrationOptions.LifetimeManager);
                                             }
                                         },
                                         container));
            return this;
        }

        /// <summary>
        /// Adds rule to exclude certain assemblies that satisfy specified assembly filter
        /// and not consider their types
        /// </summary>
        /// <param name="filter">Type filter.</param>
        /// <returns>Auto registration</returns>
        public virtual IAutoRegistration ExcludeAssemblies(Predicate<Assembly> filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }

            excludedAssemblyFilters.Add(filter);
            return this;
        }

        /// <summary>
        /// Adds rule to exclude certain types that satisfy specified type filter and not register them
        /// </summary>
        /// <param name="filter">Type filter.</param>
        /// <returns>Auto registration</returns>
        public virtual IAutoRegistration Exclude(Predicate<Type> filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }

            excludedTypeFilters.Add(filter);
            return this;
        }

        /// <summary>
        /// Applies auto registration - scans loaded assemblies,
        /// check specified rules and register types that satisfy these rules
        /// </summary>
        public virtual void ApplyAutoRegistration()
        {
            ApplyAutoRegistration(AppDomain.CurrentDomain.GetAssemblies());
        }

        /// <summary>
        /// Applies auto registration - scans supplied assemblies,
        /// check specified rules and register types that satisfy these rules
        /// </summary>
        public virtual void ApplyAutoRegistration(params Assembly[] assemblies)
        {
            if (!this.registrationEntries.Any())
            {
                return;
            }

            foreach (var type in assemblies
                .Where(a => !this.excludedAssemblyFilters.Any(f => f(a)))
                .SelectMany(a => a.GetTypes())
                .Where(t => !this.excludedTypeFilters.Any(f => f(t))))
            {
                foreach (var entry in this.registrationEntries)
                {
                    entry.RegisterIfSatisfiesFilter(type);
                }
            }
        }

        private class RegistrationEntry
        {
            private readonly Predicate<Type> typeFilter;
            private readonly Action<Type, IUnityContainer> registrator;
            private readonly IUnityContainer container;

            public RegistrationEntry(Predicate<Type> typeFilter, Action<Type, IUnityContainer> registrator, IUnityContainer container)
            {
                this.typeFilter = typeFilter;
                this.registrator = registrator;
                this.container = container;
            }

            public void RegisterIfSatisfiesFilter(Type type)
            {
                if (!this.typeFilter(type))
                {
                    return;
                }

                Debug.WriteLine("Registering " + type.FullName);
                this.registrator(type, this.container);
            }
        }
    }
}