namespace EnergyTrading.Test.Checking
{
    using System;

    using EnergyTrading.Data;

    /// <summary>
    /// Types of comparison we can perform.
    /// </summary>
    [Flags]
    public enum CompareTarget
    {
        /// <summary>
        /// Compare the value.
        /// </summary>
        Value = 0x1,

        /// <summary>
        /// Compare the identity
        /// </summary>
        /// <remarks>Checker must have implemented <see cref="IIdentityChecker" /></remarks>
        Id = 0x2,

        /// <summary>
        /// Compare the entity, invokes the checker for the type.
        /// </summary>
        Entity = 0x4,

        /// <summary>
        /// Compare the collection
        /// </summary>
        Collection = 0x8,

        /// <summary>
        /// Compare a collection count
        /// </summary>
        Count = 0x16
    }
}