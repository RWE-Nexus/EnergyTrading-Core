namespace EnergyTrading.Exceptions
{
    using System;

    public abstract class ExceptionFactory<T> : IExceptionFactory
        where T : Exception
    {
        public Exception Convert(Exception exception)
        {
            var t = exception as T;
            if (t != null)
            {
                return this.Process(t);
            }

            return exception.InnerException == null ? null : this.Convert(exception.InnerException);
        }

        protected abstract Exception Process(T ex);
    }
}