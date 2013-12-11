namespace EnergyTrading.Exceptions
{
    using System.Data.SqlClient;

    /// <summary>
    /// Handles SQL Server timeout errors
    /// </summary>
    public class SqlTimeoutExceptionHandler : ExceptionHandler<SqlException>
    {
        private const int TimeoutError = -2;

        protected override bool Process(SqlException ex)
        {
            var found = ex.Number == TimeoutError;

            // Only rethrow if we are not a timeout error
            Rethrow = !found;

            return found;
        }
    }
}