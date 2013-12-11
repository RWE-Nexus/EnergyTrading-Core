namespace EnergyTrading.Logging
{
    using System;

    using EnergyTrading.Extensions;

    public static class ExceptionExtensionsForLogging
    {
        public static string LogMessage(this Exception ex)
        {
            return string.Format("Exception Message: {0}.{1}Stack Trace: {2}.", ex.AllExceptionMessages(), Environment.NewLine, ex.StackTrace);
        }
    }
}