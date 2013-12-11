namespace EnergyTrading.Test
{
    using System.Collections.Generic;
    using System.Reflection;

    using EnergyTrading.Test.Checking;

    /// <summary>
    /// Initialization methods for a checker
    /// </summary>
    public interface ICheckerCompare
    {
        /// <summary>
        /// Collection of <see cref="PropertyCheck"/>s to use.
        /// </summary>
        ICollection<PropertyCheck> Properties { get; }

        /// <summary>
        /// Property to include in the collection.
        /// </summary>
        /// <param name="propertyInfo">PropertyInfo to use</param>
        /// <returns>A new <see cref="PropertyCheckExpression" /> created from the <see cref="PropertyInfo" /></returns>
        PropertyCheckExpression Compare(PropertyInfo propertyInfo);

        /// <summary>
        /// Property to exclude from the collection.
        /// </summary>
        /// <param name="propertyInfo">PropertyInfo to use</param>        
        void Exclude(PropertyInfo propertyInfo);
    }
}