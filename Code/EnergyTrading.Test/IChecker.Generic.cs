namespace EnergyTrading.Test
{
    /// <summary>
    /// Comparators used to verify that two instances of <see typeparamref="T" /> are 
    /// the same on a per property basis.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IChecker<in T>
    {
        /// <summary>
        /// Check that the properties of two instances of <see typeparamref="T" /> are equal
        /// </summary>
        /// <param name="expected">Expected object to use</param>
        /// <param name="candidate">Candidate object to use</param>
        /// <param name="objectName">Name to use, displayed in error messages to disambiguate</param>
        void Check(T expected, T candidate, string objectName = "");

        /// <summary>
        /// Check the base properties of <see typeparameref="T" />
        /// </summary>
        /// <param name="expected">Expected object to use</param>
        /// <param name="candidate">Candidate object to use</param>
        /// <param name="objectName">Name to use, displayed in error messages to disambiguate</param>
        void CheckBase(T expected, T candidate, string objectName = "");
    }
}