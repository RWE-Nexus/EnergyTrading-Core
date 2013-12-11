namespace EnergyTrading.Container.Unity
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    using Microsoft.Practices.Unity;

    /// <summary>
    /// Extension class for Unity that tracks types that are registered.
    /// </summary>
    /// <example>
    /// <code>
    /// var container = new UnityContainer();
    /// container.AddNewExtension&lt;TypeTrackingExtension&gt;();
    /// container.RegisterType&lt;ITest, TestClass&gt;();
    /// var obj = container.Configure&lt;TypeTrackingExtension&gt;().TryResolve&lt;Test&gt;();
    /// </code>
    /// </example>
    public class TypeTrackingExtension : UnityContainerExtension
    {
        private readonly ConcurrentDictionary<Type, HashSet<string>> registeredTypes;

        /// <summary>
        /// Initialize a new instance of the <see cref="TypeTrackingExtension" /> class.
        /// </summary>
        public TypeTrackingExtension()
        {
            // TODO: Check if we need to lock on add?
            registeredTypes = new ConcurrentDictionary<Type, HashSet<string>>();
        }

        /// <summary>
        /// Determines whether this type can be resolved with the specified name.
        /// </summary>
        /// <typeparam name="T">The type to test for resolution</typeparam>
        /// <param name="name">The name.</param>
        /// <returns>
        ///     <c>true</c> if this instance can be resolved with the specified name; otherwise, <c>false</c>.
        /// </returns>
        public bool CanResolve<T>(string name = null)
        {
            return CanResolve(typeof(T), name);
        }

        /// <summary>
        /// Determines whether this type can be resolved with the specified name.
        /// </summary>
        /// <param name="type">The type to test for resolution</param>
        /// <param name="name">The name.</param>
        /// <returns>
        ///     <c>true</c> if this instance can be resolved with the specified name; otherwise, <c>false</c>.
        /// </returns>
        public bool CanResolve(Type type, string name = null)
        {
            HashSet<string> names;
            return registeredTypes.TryGetValue(type, out names) && names.Contains(name ?? string.Empty); 
        }

        /// <summary>
        /// Determines whether this instance can be resolved at all with or without a name.
        /// </summary>
        /// <typeparam name="T">The type to test for resolution</typeparam>
        /// <returns>
        ///     <c>true</c> if this instance can be resolved at all; otherwise, <c>false</c>.
        /// </returns>
        public bool CanResolveAny<T>()
        {
            return CanResolve(typeof(T));
        }

        /// <summary>
        /// Determines whether this instance can be resolved at all with or without a name.
        /// </summary>
        /// <param name="type">The type to test for resolution</param>
        /// <returns>
        ///     <c>true</c> if this instance can be resolved at all; otherwise, <c>false</c>.
        /// </returns>
        public bool CanResolveAny(Type type)
        {
            return registeredTypes.ContainsKey(type);
        }

        /// <summary>
        /// Tries to resolve the type with the specified of name, returning default value if not found.
        /// </summary>
        /// <typeparam name="T">The type to try and resolve.</typeparam>
        /// <param name="name">The name associated with the type.</param>
        /// <returns>An object of type <see typeparameref="T"/> if found, or default value if not.</returns>
        public T TryResolve<T>(string name = null)
        {
            return TryResolve(default(T), name);
        }

        /// <summary>
        /// Tries to resolve the type with the specified of name, returning the passed in defaultValue if not found.
        /// </summary>
        /// <typeparam name="T">The type to try and resolve.</typeparam>
        /// <param name="name">The name associated with the type.</param>
        /// <param name="defaultValue">The default value to return if type not found.</param>
        /// <returns>An object of type <see typeparamref="T"/> if found, or the <see paramref="defaultValue"/> if not.</returns>
        [Obsolete("Use TryResolve<T>(T, string)")] 
        public T TryResolve<T>(string name, T defaultValue)
        {
            return !CanResolve<T>(name) ? defaultValue : Container.Resolve<T>(name);
        }

        /// <summary>
        /// Tries to resolve the type with the specified of name, returning the passed in defaultValue if not found.
        /// </summary>
        /// <typeparam name="T">The type to try and resolve.</typeparam>
        /// <param name="name">The name associated with the type.</param>
        /// <param name="defaultValue">The default value to return if type not found.</param>
        /// <returns>An object of type <see typeparamref="T"/> if found, or the <see paramref="defaultValue"/> if not.</returns>
        public T TryResolve<T>(T defaultValue, string name = null)
        {
            return !CanResolve<T>(name) ? defaultValue : Container.Resolve<T>(name);
        }

        /// <summary>
        /// Tries to resolve the type with the specified of name, returning default value if not found.
        /// </summary>
        /// <param name="type">The type to try and resolve.</param>
        /// <param name="name">The name associated with the type.</param>
        /// <returns>An object of the type if found, or default value if not.</returns>
        public object TryResolve(Type type, string name = null)
        {
            return !CanResolve(type, name) ? null : Container.Resolve(type, name);
        }

        /// <summary>
        /// Resolves all registered T in the container, conditionally including the default unnamed
        /// registered T. When includeDefault is false, this is the same as the normal Unity
        /// ResolveAll.
        /// </summary>
        /// <typeparam name="T">The type to resolve</typeparam>
        /// <param name="includeDefault">if set to <c>true</c> include default value, else do not include default.</param>
        /// <returns><see cref="IEnumerable{T}"/></returns>
        public IEnumerable<T> ResolveAll<T>(bool includeDefault = true)
        {
            var elements = new List<T>(Container.ResolveAll<T>());
            if (includeDefault && CanResolve<T>()) //  can resolve default element?
            {
                elements.Add(Container.Resolve<T>()); // then add it
            }
            return elements;
        }

        /// <summary>
        /// Resolves all registered T in the container, conditionally including the default unnamed
        /// registered T. When includeDefault is false, this is the same as the normal Unity
        /// ResolveAll.
        /// </summary>
        /// <param name="type">The type to resolve</param>
        /// <param name="includeDefault">if set to <c>true</c> include default value, else do not include default.</param>
        /// <returns><see cref="IEnumerable{T}"/></returns>
        public IEnumerable<object> ResolveAll(Type type, bool includeDefault = true)
        {
            var elements = new List<object>(Container.ResolveAll(type));
            if (includeDefault && CanResolve(type)) //  can resolve default element?
            {
                elements.Add(Container.Resolve(type)); // then add it
            }
            return elements;
        }

        /// <summary>
        /// Resolves all registered T in the container, conditionally including the default unnamed
        /// registered T. When includeDefault is false, this is the same as the normal Unity
        /// ResolveAll.
        /// </summary>
        /// <typeparam name="T">The type to resolve</typeparam>
        /// <param name="includeDefault">if set to <c>true</c> include default value, else do not include default.</param>
        /// <returns>Array of T</returns>
        public T[] ResolveAllToArray<T>(bool includeDefault = true)
        {
            var elements = new List<T>(Container.ResolveAll<T>());
            if (includeDefault && CanResolve<T>()) //  can resolve default element?
            {
                elements.Add(Container.Resolve<T>()); // then add it
            }

            return elements.ToArray();
        }

        /// <inheritdoc />
        public override void Remove()
        {
            Context.Registering -= OnNewType;
            Context.RegisteringInstance -= OnNewInstance;
        }

        /// <inheritdoc />
        protected override void Initialize()
        {
            Context.RegisteringInstance += OnNewInstance;
            Context.Registering += OnNewType;
        }

        private void OnNewInstance(object sender, RegisterInstanceEventArgs e)
        {
            Register(e.Name, e.RegisteredType);
        }

        private void OnNewType(object sender, RegisterEventArgs e)
        {
            var type = e.TypeFrom ?? e.TypeTo;
            if (type == null)
            {
                throw new NotSupportedException("The TypeFrom/TypeTo argument cannot both be null – use .RegisterType(); instead of …RegisterType(); to avoid this error");
            }

            Register(e.Name, type);
        }

        private void Register(string name, Type type)
        {
            // Find or create the bucket.
            var names = registeredTypes.GetOrAdd(type, (key) => new HashSet<string>());

            // Add the name to the bucket
            name = string.IsNullOrEmpty(name) ? string.Empty : name;

            names.Add(name);
        }
    }
}