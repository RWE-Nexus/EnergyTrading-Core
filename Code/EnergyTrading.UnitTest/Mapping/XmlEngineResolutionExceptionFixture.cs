namespace EnergyTrading.UnitTest.Mapping
{
    using EnergyTrading.Mapping;

    using NUnit.Framework;

    [TestFixture]
    public class XmlEngineResolutionExceptionFixture
    {
        [Test]
        public void TestProperties()
        {
            var exception = new XmlEngineResolutionException(XmlEngineResolutionErrorCode.UnexpectedSchema, "asmVersion");
            Assert.AreEqual("asmVersion", exception.AsmVersion);
            Assert.AreEqual(XmlEngineResolutionErrorCode.UnexpectedSchema, exception.Code);
        }

        [Test]
        public void TestUndeterminedErrorMessage()
        {
            var exception = new XmlEngineResolutionException(XmlEngineResolutionErrorCode.Undetermined, "asmVersion");
            Assert.IsTrue(exception.Message.Contains("asmVersion"));
            Assert.IsTrue(exception.Message.Contains("Undetermined"));
        }

        [Test]
        public void TestUnexpectedSchemaErrorMessage()
        {
            var exception = new XmlEngineResolutionException(XmlEngineResolutionErrorCode.UnexpectedSchema, "asmVersion");
            Assert.IsTrue(exception.Message.Contains("asmVersion"));
            Assert.IsTrue(exception.Message.Contains("Unexpected schema"));
            Assert.IsFalse(exception.Message.Contains("Undetermined"));
        }

        [Test]
        public void TestHigherErrorMessage()
        {
            var exception = new XmlEngineResolutionException(XmlEngineResolutionErrorCode.MessageVersionTooHigh, "admVersion");
            Assert.IsTrue(exception.Message.Contains("admVersion"));
            Assert.IsTrue(exception.Message.Contains("higher"));
            Assert.IsFalse(exception.Message.Contains("lower"));
        }

        [Test]
        public void TestLowerErrorMessage()
        {
            var exception = new XmlEngineResolutionException(XmlEngineResolutionErrorCode.MessageVersionTooLow, "admVersion");
            Assert.IsTrue(exception.Message.Contains("admVersion"));
            Assert.IsTrue(exception.Message.Contains("lower"));
            Assert.IsFalse(exception.Message.Contains("higher"));
        }

        [Test]
        public void TestHigherAndLowerErrorMessage()
        {
            var exception = new XmlEngineResolutionException(XmlEngineResolutionErrorCode.MessageVersionTooHigh | XmlEngineResolutionErrorCode.MessageVersionTooLow, "admVersion");
            Assert.IsTrue(exception.Message.Contains("admVersion"));
            Assert.IsTrue(exception.Message.Contains("lower"));
            Assert.IsTrue(exception.Message.Contains("higher"));
        }
    }
}