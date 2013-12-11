namespace EnergyTrading.UnitTest.Container.Unity.AutoRegistration
{
    using System;

    using EnergyTrading.UnitTest.Container.Unity.AutoRegistration.Contract;

    [Logger]
    public class TestLogger : ILogger, IDisposable
    {
        public void Log(string message)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}