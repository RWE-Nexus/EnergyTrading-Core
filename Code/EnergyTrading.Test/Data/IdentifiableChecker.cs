namespace EnergyTrading.Test.Data
{
    using System;

    using EnergyTrading.Data;
    using EnergyTrading.Test.Checking;

    /// <summary>
    /// Implements an identity checker for <see cref="IIdentifiable" />.
    /// </summary>
    public class IdentifiableChecker : IIdentityChecker
    {
        /// <copydocfrom cref="IIdentityChecker.SupportsId" />
        public bool SupportsId(Type type)
        {
            return typeof(IIdentifiable).IsAssignableFrom(type);
        }

        /// <copydocfrom cref="IIdentityChecker.ExtractId" />
        public object ExtractId(object value)
        {
            var x = value as IIdentifiable;
            return x == null ? null : x.Id;
        }
    }
}