namespace EnergyTrading.ServiceModel
{
    using System;

    public static class ExceptionExtensions
    {
        public static WcfServiceFault ToWcfServiceFault(this Exception ex)
        {
            var fault = ex.ToSimpleFault();
            var te = ex;
            var wrapper = fault;
            while (te.InnerException != null)
            {
                te = te.InnerException;
                wrapper.Inner = te.ToSimpleFault();
                wrapper = wrapper.Inner;
            }

            return fault;
        }

        private static WcfServiceFault ToSimpleFault(this Exception ex)
        {
            return new WcfServiceFault { Message = ex.Message, Source = ex.Source, Target = ex.TargetSite.ToString() };     
        }
    }
}