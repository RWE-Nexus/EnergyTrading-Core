namespace EnergyTrading.UnitTest.Logging
{
    using EnergyTrading.Logging.Log4Net;

    using NUnit.Framework;

    [TestFixture]
    public class Log4NetLoggerFixture
    {
        [Test]
        public void ResolveTheLog4NetConfig()
        {
            var log4NetConfiguration = new Log4NetConfiguration();
            Assert.IsNotNull(log4NetConfiguration);            
        }
    }
}