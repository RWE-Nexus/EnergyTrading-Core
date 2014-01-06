namespace EnergyTrading.Mapping
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides a context for the mapping.
    /// </summary>
    public class Context : ICloneable
    {
        private readonly Dictionary<string, object> values;

        public Context()
        {
            this.values = new Dictionary<string, object>();
        }

        private Context(IDictionary<string, object> values)
        {
            this.values = new Dictionary<string, object>(values);
        }

        /// <summary>
        /// Determine whether a named value exists in the context
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Exists(string name)
        {
            return this.values.ContainsKey(name);
        }

        /// <summary>
        /// Set a named value in the context
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void Set<T>(string name, T value)
        {
            this.values[name] = value;
        }

        /// <summary>
        /// Get a named value in the context
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T Value<T>(string name)
        {
            object value;
            return this.values.TryGetValue(name, out value) ? (T)value : default(T);
        }

        /// <summary>
        /// Clone the context, copying current values.
        /// <para>
        /// This is a deep copy, i.e. the values are independent after the clone operation
        /// though if reference types are stored the values will be shared between contexts
        /// </para>
        /// </summary>
        /// <returns></returns>
        public Context Clone()
        {
            // Clone a copy of current values
            return new Context(this.values);
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }
    }
}