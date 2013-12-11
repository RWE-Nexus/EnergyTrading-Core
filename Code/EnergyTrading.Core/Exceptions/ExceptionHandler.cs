namespace EnergyTrading.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    /// <summary>
    /// Handles exceptions of a particular class
    /// </summary>
    public class ExceptionHandler : IExceptionHandler
    {
        public ExceptionHandler()
        {
            Handlers = new List<IExceptionHandler>();
        }

        public List<IExceptionHandler> Handlers { get; private set; }

        /// <summary>
        /// See if the exception is one we can ever possibly handle.
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static bool IsCritical(Exception ex)
        {
            if (ex is DataException)
            {
                return false;
            }

            return ex is SystemException;

            //if (ex is OutOfMemoryException) return true;
            //if (ex is AppDomainUnloadedException) return true;
            //if (ex is BadImageFormatException) return true;
            //if (ex is CannotUnloadAppDomainException) return true;
            //if (ex is ExecutionEngineException) return true;
            //if (ex is InvalidProgramException) return true;
            //if (ex is System.Threading.ThreadAbortException) return true;
        }

        public void AddHandler(IExceptionHandler handler)
        {
            Handlers.Add(handler);
        }

        public bool Rethrow
        {
            get { throw new NotImplementedException(); }
        }

        public bool Handle(Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}
