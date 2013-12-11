namespace EnergyTrading.UnitTest.Logging
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Logging.Log4Net;

    [TestClass]
    public class Log4NetLoggerFixture
    {
        [TestMethod]
        public void ResolveTheLog4NetConfig()
        {
            var log4NetConfiguration = new Log4NetConfiguration();
            Assert.IsNotNull(log4NetConfiguration);            
        }
    }
}