namespace EnergyTrading.Services
{
    using System;

    /// <summary>
    /// Simple type resolver that uses a functor to resolve types.
    /// </summary>
    public class TypeResolver : ITypeResolver
    {
        private readonly Func<string, Type> getType;

        public TypeResolver() : this(Type.GetType)
        {
        }

        public TypeResolver(Func<string, Type> getType)
        {
            this.getType = getType;
        }

        /// <inheritdoc />
        public T Resolve<T>(string typeName) where T : class
        {
            var type = getType(typeName);
            if (type == null)
            {
                throw new ArgumentException("Failed to resolve the type: " + typeName);
            }

            return Resolve<T>(type);
        }

        /// <inheritdoc />
        public T Resolve<T>(Type type) where T : class
        {
            var constructorInfo = type.GetConstructor(new Type[0]);
            if (constructorInfo == null)
            {
                throw new ArgumentException("Cannot resolve types without a nullary constructor");
            }

            var instance = constructorInfo.Invoke(new object[0]) as T;
            if (instance == null)
            {
                throw new ArgumentException("Type must implement " + typeof(T).Name);
            }

            return instance;
        }
    }
}