namespace EnergyTrading.UnitTest.Container.Unity.AutoRegistration.Contract
{
    using System.Diagnostics;

    [Logger]
    public class TraceLogger : ILogger
    {
        public void Log(string message)
        {
            Trace.WriteLine(message);
        }
    }
}