namespace EnergyTrading.Services
{
    using global::System;

    /// <summary>
    /// Resolves instances of .NET types, allows us to abstract from <see cref="Activator" /> to
    /// potentially use a factory or some other means.
    /// </summary>
    public interface ITypeResolver
    {
        /// <summary>
        /// Locate an object based on a type name
        /// </summary>
        /// <typeparam name="T">Type we are resolving</typeparam>
        /// <param name="typeName"></param>
        /// <returns></returns>
        T Resolve<T>(string typeName) where T : class;

        /// <summary>
        /// Locate an object based on a type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        T Resolve<T>(Type type) where T : class;
    }
}
