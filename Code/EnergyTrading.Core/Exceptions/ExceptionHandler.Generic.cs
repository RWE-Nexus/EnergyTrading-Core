namespace EnergyTrading.Exceptions
{
    using System;

    /// <summary>
    /// Handles exceptions of a particular class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ExceptionHandler<T> : IExceptionHandler<T>, IExceptionHandler
        where T : Exception
    {
        /// <summary>
        /// Get whether we should rethrow the exception, use when we handled the exception here
        /// </summary>
        public bool Rethrow { get; set; }

        public bool Handle(Exception ex)
        {
            var t = ex as T;
            if (t != null)
            {
                return this.Process(t);
            }

            return ex.InnerException != null && this.Handle(ex.InnerException);
        }

        protected abstract bool Process(T ex);
    }
}
