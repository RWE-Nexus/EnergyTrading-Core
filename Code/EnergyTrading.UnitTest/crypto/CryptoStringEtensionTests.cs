namespace EnergyTrading.UnitTest.crypto
{
    using System;

    using EnergyTrading.Crypto;

    using NUnit.Framework;

    [TestFixture]
    public class CryptoStringEtensionTests
    {
        [Test]
        public void CanDecryptConnectionString()
        {
            var password = "Test";
            var connectionString = "Data Source=.;Initial Catalog=EventMonitor;UserName=A;Password=" + password.EncryptString();
            var expected = "Data Source=.;Initial Catalog=EventMonitor;UserName=A;Password=Test";
            var candidate = connectionString.DecryptConnectionString();
            Assert.AreEqual(expected, candidate);
        }

        [Test]
        public void StringIsReversedProperly()
        {
            var decrypted = "t0SB6d3NCBGFiaGG69v4vrX9uDDgJs7R9iynWIyon40=".DecryptString();
            const string Start = "testdata";
            var end = Start.EncryptString().DecryptString();
            Assert.AreEqual(Start, end);
        }

        [Test]
        public void SubsequentEncodingsAreDifferent()
        {
            const string Data = "testdata";
            var first = Data.EncryptString();
            var second = Data.EncryptString();
            Assert.AreNotEqual(first, second);
            Assert.AreEqual(Data, first.DecryptString());
            Assert.AreEqual(Data, second.DecryptString());
        }

        [Test]
        public void CanEncryptEmptyStrings()
        {
            var end = string.Empty.EncryptString().DecryptString();
            Assert.AreEqual(end, string.Empty);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CannotDecryptEmptyStrings()
        {
            string.Empty.DecryptString();
        }
    }
}
