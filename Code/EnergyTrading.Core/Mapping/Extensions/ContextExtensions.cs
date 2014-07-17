namespace EnergyTrading.Mapping.Extensions
{
    using System;

    public static class ContextExtensions
    {
        /// <summary>
        /// Returns the value stored in the Context if it exists 
        /// Otherwise attempts to retrieve the value, add it to the Context and then return it
        /// If neither is possible returns default(T)
        /// </summary>
        /// <typeparam name="T">the type of object expected</typeparam>
        /// <param name="context">The Context to check</param>
        /// <param name="name">The name to check for the value within the Context</param>
        /// <param name="retriever">Function used to retrieve the object if it is not already in the context</param>
        /// <returns>the value in the context or the value returned from retriever or default(T) if retriever is null</returns>
        public static T CheckOrRetrieveValue<T>(this Context context, string name, Func<T> retriever = null)
        {
            if (context == null || string.IsNullOrWhiteSpace(name))
            {
                return default(T);
            }

            if (context.Exists(name))
            {
                return context.Value<T>(name);
            }

            if (retriever == null)
            {
                return default(T);
            }

            context.Set(name, retriever());
            return context.Value<T>(name);
        }
    }
}