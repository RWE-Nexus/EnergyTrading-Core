namespace EnergyTrading.UnitTest.Crypto
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Crypto;

    [TestClass]
    public class CryptoStringEtensionTests
    {
        [TestMethod]
        public void CanDecryptConnectionString()
        {
            var password = "Test";
            var connectionString = "Data Source=.;Initial Catalog=EventMonitor;UserName=A;Password=" + password.EncryptString();
            var expected = "Data Source=.;Initial Catalog=EventMonitor;UserName=A;Password=Test";
            var candidate = connectionString.DecryptConnectionString();
            Assert.AreEqual(expected, candidate);
        }

        [TestMethod]
        public void StringIsReversedProperly()
        {
            const string Start = "testdata";
            var end = Start.EncryptString().DecryptString();
            Assert.AreEqual(Start, end);
        }

        [TestMethod]
        public void SubsequentEncodingsAreDifferent()
        {
            const string Data = "testdata";
            var first = Data.EncryptString();
            var second = Data.EncryptString();
            Assert.AreNotEqual(first, second);
            Assert.AreEqual(Data, first.DecryptString());
            Assert.AreEqual(Data, second.DecryptString());
        }

        [TestMethod]
        public void CanEncryptEmptyStrings()
        {
            var end = string.Empty.EncryptString().DecryptString();
            Assert.AreEqual(end, string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CannotDecryptEmptyStrings()
        {
            string.Empty.DecryptString();
        }
    }
}
