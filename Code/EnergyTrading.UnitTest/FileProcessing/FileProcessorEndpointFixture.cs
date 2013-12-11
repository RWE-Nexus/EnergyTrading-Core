namespace EnergyTrading.UnitTest.FileProcessing
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.FileProcessing;

    [TestClass]
    public class FileProcessorEndpointFixture
    {
        public void TestValidate(string name, string processConfigurator, string inProgressPath, bool expectException)
        {
            var failSuffix = name + ", " + processConfigurator + ", " + inProgressPath;
            try
            {
                var endpoint = new FileProcessorEndpoint { Name = name, ProcessorConfigurator = processConfigurator, InProgressPath = inProgressPath };
                var candidate = endpoint.Validate();
                if (expectException)
                {
                    Assert.Fail("expected Exception was not thrown : " + failSuffix);
                }
                Assert.IsTrue(candidate);
            }
            catch (NotSupportedException)
            {
                if (!expectException)
                {
                    Assert.Fail("Exception was thrown but not expected : " + failSuffix);
                }
            }
        }

        [TestMethod]
        public void PerformValidateTests()
        {
            this.TestValidate(null, null, null, true);
            this.TestValidate(string.Empty, null, null, true);
            this.TestValidate("   ", null, null, true);
            this.TestValidate("   ", "EventBased", null, true);
            this.TestValidate("name", null, null, true);
            this.TestValidate("name", string.Empty, null, true);
            this.TestValidate("name", "    ", null, true);
            this.TestValidate("name", "PollingBased", null, false); // by the time we're at the endpoint PollingBased should have already been converted to the type
            this.TestValidate("name", "EventBased", null, false);
            this.TestValidate("name", FileProcessorEndpoint.EventBasedConfiguratorType, null, false);
            this.TestValidate("name", FileProcessorEndpoint.PollingBasedConfiguratorType, null, true);
            this.TestValidate("name", FileProcessorEndpoint.PollingBasedConfiguratorType, string.Empty, true);
            this.TestValidate("name", FileProcessorEndpoint.PollingBasedConfiguratorType, "   ", true);
            this.TestValidate("name", FileProcessorEndpoint.PollingBasedConfiguratorType, "inprogress", false);
            this.TestValidate("name", "   ", "inprogress", true);
        }
    }
}