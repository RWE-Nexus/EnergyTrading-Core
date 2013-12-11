namespace EnergyTrading.Data.Sql
{
    using System;
    using System.Data.SqlClient;
    using System.Runtime.InteropServices;

    using EnergyTrading.Exceptions;

    public class SqlExceptionFactory : ExceptionFactory<SqlException>
    {
        protected override Exception Process(SqlException ex)
        {
            var message = ex.Message.ToLowerInvariant();
            if (message.Contains("invalid column"))
            {
                return new InvalidComObjectException(ex.Message, ex);
            }

            if (message.Contains("cannot insert duplicate key"))
            {
                return new DuplicateKeyException("A record with this key already exists", ex);
            }

            if (message.Contains("delete statment conflicted"))
            {
                return new ConstraintViolationException("Delete not allowed", ex);
            }

            return null;
        }
    }
}
