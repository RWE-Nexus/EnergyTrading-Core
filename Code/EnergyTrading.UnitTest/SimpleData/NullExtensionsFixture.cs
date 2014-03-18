namespace EnergyTrading.UnitTest.SimpleData
{
    using System;

    using EnergyTrading.Data.SimpleData;

    using NUnit.Framework;

    [TestFixture]
    public class NullExtensionsFixture
    {
        [Test]
        public void IsDbNullReturnsTrueIfIsDbNullValue()
        {
            Assert.IsTrue(DBNull.Value.IsDbNull());
        }

        [Test]
        public void IsDbNullReturnsTrueForOtherValues()
        {
            Assert.IsFalse(1.IsDbNull());
        }

        [Test]
        public void DefaultIfDbNullReturnsValueIfNotNull()
        {
            Assert.AreEqual(1, 1.DefaultIfDbNull(3));
        }

        [Test]
        public void DefaultIfDbNullReturnsDefaultIfDbNull()
        {
            Assert.AreEqual(null, DBNull.Value.DefaultIfDbNull((string)null));
            Assert.AreEqual(3, DBNull.Value.DefaultIfDbNull(3));
        }
    }
}