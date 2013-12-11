namespace EnergyTrading.Data
{
    using System.Data;

    /// <summary>
    /// Provides calling methods against the database with SQL
    /// </summary>
    public interface IDao
    {
        /// <summary>
        /// Gets underlying connection.
        /// </summary>
        /// <remarks>
        /// By exposing this, we can use the <see cref="IDbConnection" /> factory methods
        /// for creating commands, parameters etc.
        /// </remarks>
        IDbConnection Connection { get; }

        /// <summary>
        /// Execute a non-query action against the database.
        /// </summary>
        /// <param name="sql">SQL command to execute</param>
        /// <param name="timeout">Execution timeout in seconds.</param>
        int ExecuteNonQuery(string sql, int timeout = 30);
    }
}