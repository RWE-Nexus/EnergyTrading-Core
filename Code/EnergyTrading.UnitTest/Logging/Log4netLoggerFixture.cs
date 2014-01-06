namespace EnergyTrading.UnitTest.Logging
{
    using EnergyTrading.Logging.Log4Net;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

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