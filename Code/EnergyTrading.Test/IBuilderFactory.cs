namespace EnergyTrading.Test
{
    /// <summary>
    /// Allocates/modifies instances of objects.
    /// </summary>
    /// <remarks>
    /// Used primarily to supply data for unit testing
    /// </remarks>
    public interface IBuilderFactory
    {
        /// <summary>
        /// Create a default instance of <see typeparamref="T" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Default<T>();

        /// <summary>
        /// Create a default instance of <see typeparamref="T" /> and possibly persist it
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="persist"></param>
        /// <returns></returns>
        T Default<T>(bool persist);

        /// <summary>
        /// Create a default instance of <see typeparamref="T" /> with the specified identity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        T Default<T>(object id);

        /// <summary>
        /// Create a default instance of <see typeparamref="T" /> with the specified identity and possibly persist it
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="persist"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        T Default<T>(bool persist, object id);

        /// <summary>
        /// Create a default instance of <see typeparamref="T" /> with the specified values
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        T Default<T>(params object[] values);

        /// <summary>
        /// Create a default instance of <see typeparamref="T" /> with the specified identity and possibly persist it
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="persist"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        T Default<T>(bool persist, params object[] values);

        /// <summary>
        /// Assign default property value to an instance of <see typeparamref="T" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void Assign<T>(T entity);

        /// <summary>
        /// Change property values of an instance of <see typeparamref="T" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void Change<T>(T entity);

        /// <summary>
        /// Change the parent property values of an instance of <see typeparamref="T" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void ChangeParent<T>(T entity);
    }
}
