namespace EnergyTrading.Threading
{
    using System;
    using System.Reflection;
    using System.Threading;

    using EnergyTrading.Logging;

    /// <summary>
    /// Generic code to retry calls to functions or methods
    /// </summary>
    public static class TaskExtensions
    {
        private static readonly ILogger Logger = LoggerFactory.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Retry calls to a function.
        /// </summary>
        /// <typeparam name="T">Type returned from the task.</typeparam>
        /// <param name="task">Task to execute.</param>
        /// <param name="maxRetries">Number of times to attempt the task.</param>
        /// <param name="interval">Time in milliseconds to wait between attempts.</param>
        /// <returns></returns>
        public static T Retry<T>(this Func<T> task, int maxRetries = 3, int interval = 500)
        {
            var tries = 0;
            while (true)
            {
                try
                {
                    return task();
                }
                catch (Exception)
                {
                    if (++tries > maxRetries)
                    {
                        Logger.Warn("Not succeeded after " + tries  + " attempts. Raising Exception");
                        throw;
                    }
                    Thread.Sleep(interval);
                }
            }
        }

        /// <summary>
        /// Retry calls to a action.
        /// </summary>
        /// <param name="task">Task to execute.</param>
        /// <param name="maxRetries">Number of times to attempt the task.</param>
        /// <param name="interval">Time in milliseconds to wait between attempts.</param>
        /// <returns></returns>
        public static void Retry(this Action task, int maxRetries = 3, int interval = 500)
        {
            var tries = 0;
            while (true)
            {
                try
                {
                    task();
                    return;
                }
                catch (Exception)
                {
                    if (++tries > maxRetries)
                    {
                        Logger.Warn("Not succeeded after " + tries + " attempts. Raising Exception");
                        throw;
                    }
                    Thread.Sleep(interval);
                }
            }
        }
    }
}