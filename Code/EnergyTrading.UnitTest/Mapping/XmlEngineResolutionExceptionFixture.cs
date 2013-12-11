namespace EnergyTrading.UnitTest.Mapping
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Mapping;

    [TestClass]
    public class XmlEngineResolutionExceptionFixture
    {
        [TestMethod]
        public void TestProperties()
        {
            var exception = new XmlEngineResolutionException(XmlEngineResolutionErrorCode.UnexpectedSchema, "asmVersion");
            Assert.AreEqual("asmVersion", exception.AsmVersion);
            Assert.AreEqual(XmlEngineResolutionErrorCode.UnexpectedSchema, exception.Code);
        }

        [TestMethod]
        public void TestUndeterminedErrorMessage()
        {
            var exception = new XmlEngineResolutionException(XmlEngineResolutionErrorCode.Undetermined, "asmVersion");
            Assert.IsTrue(exception.Message.Contains("asmVersion"));
            Assert.IsTrue(exception.Message.Contains("Undetermined"));
        }

        [TestMethod]
        public void TestUnexpectedSchemaErrorMessage()
        {
            var exception = new XmlEngineResolutionException(XmlEngineResolutionErrorCode.UnexpectedSchema, "asmVersion");
            Assert.IsTrue(exception.Message.Contains("asmVersion"));
            Assert.IsTrue(exception.Message.Contains("Unexpected schema"));
            Assert.IsFalse(exception.Message.Contains("Undetermined"));
        }

        [TestMethod]
        public void TestHigherErrorMessage()
        {
            var exception = new XmlEngineResolutionException(XmlEngineResolutionErrorCode.MessageVersionTooHigh, "admVersion");
            Assert.IsTrue(exception.Message.Contains("admVersion"));
            Assert.IsTrue(exception.Message.Contains("higher"));
            Assert.IsFalse(exception.Message.Contains("lower"));
        }

        [TestMethod]
        public void TestLowerErrorMessage()
        {
            var exception = new XmlEngineResolutionException(XmlEngineResolutionErrorCode.MessageVersionTooLow, "admVersion");
            Assert.IsTrue(exception.Message.Contains("admVersion"));
            Assert.IsTrue(exception.Message.Contains("lower"));
            Assert.IsFalse(exception.Message.Contains("higher"));
        }

        [TestMethod]
        public void TestHigherAndLowerErrorMessage()
        {
            var exception = new XmlEngineResolutionException(XmlEngineResolutionErrorCode.MessageVersionTooHigh | XmlEngineResolutionErrorCode.MessageVersionTooLow, "admVersion");
            Assert.IsTrue(exception.Message.Contains("admVersion"));
            Assert.IsTrue(exception.Message.Contains("lower"));
            Assert.IsTrue(exception.Message.Contains("higher"));
        }
    }
}