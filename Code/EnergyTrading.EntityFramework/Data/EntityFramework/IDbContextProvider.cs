namespace EnergyTrading.Data.EntityFramework
{
    using System.Data.Entity;

    /// <summary>
    /// Delivers a <see cref="DbContext" /> to consumers.
    /// </summary>
    public interface IDbContextProvider
    {
        /// <summary>
        /// Return the current context
        /// </summary>
        /// <returns></returns>
        DbContext CurrentContext();

        /// <summary>
        /// Release the current context
        /// </summary>
        void Close();
    }
}
