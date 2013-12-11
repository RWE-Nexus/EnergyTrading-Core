namespace EnergyTrading.Test
{
    using System;

    /// <summary>
    /// Builds a checker for a type.
    /// </summary>
    public interface ICheckerBuilder
    {
        /// <summary>
        /// Build a <see cref="IChecker"/> for a type.
        /// </summary>
        /// <param name="type">Type to use</param>
        /// <returns>A <see cref="IChecker" /> for the type.</returns>
        IChecker Build(Type type);
    }
}
