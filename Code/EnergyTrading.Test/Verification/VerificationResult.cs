namespace EnergyTrading.Test.Verification
{
    using System;

    public class VerificationResult<T>
    {
        public string Code { get; set; }

        public T TestId { get; set; }

        public string MessageResult { get; set; }
    }
}