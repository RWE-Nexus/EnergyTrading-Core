namespace EnergyTrading.Test.Checking
{
    using System;

    /// <summary>
    /// Allows the <see cref="PropertyCheck" /> to defer what supports identity and how it is acquired.
    /// </summary>
    public interface IIdentityChecker
    {
        /// <summary>
        /// Determines if a type supports identity.
        /// <para>
        /// Allows us to provide earlier error detection when a checker is being initialized.
        /// </para>
        /// </summary>
        /// <param name="type">Type to use</param>
        /// <returns>true if it supports an id, otherwise false.</returns>
        bool SupportsId(Type type);

        /// <summary>
        /// Extracts an id from an entity.
        /// </summary>
        /// <param name="entity">Entity to use</param>
        /// <returns>Id if present, otherwise null.</returns>
        object ExtractId(object entity);
    }
}