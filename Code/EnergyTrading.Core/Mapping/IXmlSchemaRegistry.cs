namespace EnergyTrading.Mapping
{
    using System.Collections.Generic;

    /// <summary>
    /// Records XML schemas that are registered.
    /// </summary>
    public interface IXmlSchemaRegistry
    {
        /// <summary>
        /// Record a schema in the registry.
        /// </summary>
        /// <param name="schema">Schema to register.</param>
        void RegisterSchema(string schema);

        /// <summary>
        /// Determine whether a schema is registered.
        /// </summary>
        /// <param name="schema">Schema to check</param>
        /// <returns>true if the schema is registered, false otherwise.</returns>
        bool SchemaExists(string schema);

        /// <summary>
        /// Retrieve a copy of the registered schemas.
        /// </summary>
        /// <returns>Enumeration of the current schemas, not linked to the internal representation, so thread-safe.</returns>
        IEnumerable<string> GetSchemas();
    }
}