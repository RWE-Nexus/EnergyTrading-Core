namespace EnergyTrading.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Translates exceptions using individual factories
    /// </summary>
    public class ExceptionTranslator : IExceptionFactory
    {
        private readonly List<IExceptionFactory> factories;

        public ExceptionTranslator()
        {
            this.factories = new List<IExceptionFactory>();
        }

        public Exception Convert(Exception exception)
        {
            return ExceptionHandler.IsCritical(exception) 
                 ? null 
                 : this.factories.Select(f => f.Convert(exception)).FirstOrDefault(x => x != null);
        }

        public void AddFactory(IExceptionFactory factory)
        {
            factories.Add(factory);
        }
    }
}