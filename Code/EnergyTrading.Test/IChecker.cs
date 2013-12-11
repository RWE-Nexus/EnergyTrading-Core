namespace EnergyTrading.Test
{
    /// <summary>
    /// Checks two objects to see if they are the same on a per property basis.
    /// </summary>
    public interface IChecker
    {
        /// <summary>
        /// Compare two instances to see if they are the same on a per property basis.
        /// <para>
        /// Will throw an exception on the first non-matching property.
        /// </para>
        /// </summary>
        /// <param name="expected">Object containing expected values</param>
        /// <param name="candidate">Object containing values to check</param>
        /// <param name="objectName">Name of the object, used to disambiguate error messages e.g. SalePrice vs CostPrice</param>
        void Check(object expected, object candidate, string objectName);
    }
}