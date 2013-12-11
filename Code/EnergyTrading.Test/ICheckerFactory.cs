namespace EnergyTrading.Test
{
    using System.Collections.Generic;

    /// <summary>
    /// Delivers comparators used to verify that two instances of <see typeparamref="T" /> are 
    /// the same on a per property basis.
    /// </summary>
    public interface ICheckerFactory : IChecker
    {
        /// <summary>
        /// Check that the properties of two instances of <see typeparamref="T" /> are equal
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expected"></param>
        /// <param name="candidate"></param>
        void Check<T>(T expected, T candidate);

        /// <summary>
        /// Check that the properties of two instances of <see typeparamref="T" /> are equal
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expected"></param>
        /// <param name="candidate"></param>
        /// <param name="objectName"></param>
        void Check<T>(T expected, T candidate, string objectName);

        /// <summary>
        /// Check if two collections of <see typeparamref="T" /> are equal.
        /// </summary>
        /// <remarks>
        /// The parameters are first checked for null, an exception is thrown if only one is null.
        /// Second, the cardinalities of the collections are checked if the <see cref="IEnumerable{T}" /> is
        /// also <see cref="ICollection{T}" /> which means that it supports <see cref="ICollection{T}.Count" />
        /// If these checks are passed, each item is compared in turn.
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="expectedList"></param>
        /// <param name="candidateList"></param>
        /// <param name="objectName"></param>
        void Check<T>(IEnumerable<T> expectedList, IEnumerable<T> candidateList, string objectName);

        /// <summary>
        /// Check that the properties of the parent class of <see typeparamref="T" /> are equal.
        /// </summary>
        /// <remarks>
        /// Only used when testing a class which is part of a hiearchy
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="expected"></param>
        /// <param name="candidate"></param>
        /// <param name="objectName"></param>
        void CheckParent<T>(T expected, T candidate, string objectName);
    }
}