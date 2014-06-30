namespace EnergyTrading.Test
{
    using System;

    /// <summary>
    /// Use reflection to construct a checker for a type
    /// </summary>
    [Obsolete("Use NCheck.CheckerBuilder, also CheckerBuilder is now assigned in default CheckerFactory")]
    public class CheckerBuilder : NCheck.CheckerBuilder
    {
        /// <summary>
        /// Create a new instance of the <see cref="CheckerBuilder" /> class.
        /// </summary>
        /// <param name="factory"></param>
        public CheckerBuilder(NCheck.ICheckerFactory factory) : base(factory)
        {
        }
    }
}