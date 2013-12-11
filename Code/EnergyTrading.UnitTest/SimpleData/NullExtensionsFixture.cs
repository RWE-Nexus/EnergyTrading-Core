namespace RWEST.Nexus.UnitTest.SimpleData
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using global::Nexus.Data.SimpleData;

    [TestClass]
    public class NullExtensionsFixture
    {
        [TestMethod]
        public void IsDbNullReturnsTrueIfIsDbNullValue()
        {
            Assert.IsTrue(DBNull.Value.IsDbNull());
        }

        [TestMethod]
        public void IsDbNullReturnsTrueForOtherValues()
        {
            Assert.IsFalse(1.IsDbNull());
        }

        [TestMethod]
        public void DefaultIfDbNullReturnsValueIfNotNull()
        {
            Assert.AreEqual(1, 1.DefaultIfDbNull(3));
        }

        [TestMethod]
        public void DefaultIfDbNullReturnsDefaultIfDbNull()
        {
            Assert.AreEqual(null, DBNull.Value.DefaultIfDbNull((string)null));
            Assert.AreEqual(3, DBNull.Value.DefaultIfDbNull(3));
        }
    }
}