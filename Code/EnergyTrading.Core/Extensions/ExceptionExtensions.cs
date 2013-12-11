namespace EnergyTrading.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class ExceptionExtensions
    {
        public static string AllExceptionMessages(this Exception exception)
        {
            var sb = new StringBuilder();

            if (!string.IsNullOrEmpty(exception.Message))
            {
                sb.AppendLine(exception.Message);
            }

            AppendException(exception.InnerException, sb);

            return sb.ToString();
        }

        private static void AppendException(Exception ex, StringBuilder sb)
        {
            if (ex == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(ex.Message))
            {
                sb.AppendLine(ex.Message);
            }

            AppendException(ex.InnerException, sb);
        }

        public static void AddThrown(this IList<Exception> list, Action action, IList<Type> typesToCatch = null)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (list != null && (typesToCatch == null || typesToCatch.Count == 0 || typesToCatch.Contains(ex.GetType())))
                {
                    list.Add(ex);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}