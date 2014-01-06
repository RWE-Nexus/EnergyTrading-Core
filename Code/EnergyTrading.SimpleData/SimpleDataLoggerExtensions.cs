namespace EnergyTrading.Data.SimpleData
{
    using System;
    using System.Data.Common;

    using EnergyTrading.Logging;

    using Simple.Data;

    public static class SimpleDataLoggerExtensions
    {
        public static void TryDatabaseTask(this ILogger logger, Action task, string message = "", bool rethrow = true)
        {
            TryDatabaseTask(
                logger, 
                () =>
                {
                    task();
                    return 0;
                }, 
                message, 
                rethrow);
        }

        public static T TryDatabaseTask<T>(this ILogger logger, Func<T> task, string message = "", bool rethrow = true)
        {
            try
            {
                logger.Debug("Trying database task");
                return task();
            }
            catch (SimpleDataException e)
            {
                WrapException(logger, message, rethrow, e);
            }
            catch (DbException e)
            {
                WrapException(logger, message, rethrow, e);
            }

            return default(T);
        }

        private static void WrapException(ILogger logger, string message, bool rethrow, Exception e)
        {
            var msg = "Error accessing database: " + message;
            if (rethrow)
            {
                logger.Debug(string.Format("Trying database task failed: {0}", e.Message));
                throw new RepositoryException(msg, e);
            }
            logger.Error(msg, e);
        }
    }
}