namespace EnergyTrading.Test.Validation
{
    using System;

    using EnergyTrading.Services;

    /// <summary>
    /// Allows for verification of components/services.
    /// </summary>
    public interface IVerifier : IStartable
    {
        event EventHandler<EventArgs> VerificationPerformed;
    }
}
