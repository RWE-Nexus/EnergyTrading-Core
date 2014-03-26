namespace EnergyTrading.UnitTest.Mapping
{
    using System;

    using EnergyTrading.Mapping;

    using NUnit.Framework;

    [TestFixture]
    public class SchemaVersionExtensionsFixture : Fixture
    {        
        [Test]
        public void FromNumberToVersionString()
        {
            var value = "10";
            var candidate = value.ToVersionString();
            Assert.AreEqual("10.0", candidate);
        }

        [Test]
        public void FromNumberToVersionStringWithMinorValue()
        {
            var value = "10.4";
            var candidate = value.ToVersionString();
            Assert.AreEqual("10.4", candidate);
        }

        [Test]
        public void FromAsmToVersionString()
        {
            var value = "V10";
            var candidate = value.ToVersionString();
            Assert.AreEqual("10.0", candidate);
        }

        [Test]
        public void FromAsmToVersionStringWithMinorValue()
        {
            var value = "V10_4";
            var candidate = value.ToVersionString();
            Assert.AreEqual("10.4", candidate);
        }

        [Test]
        public void FromAsmToVersion()
        {
            var value = "V10";
            var candidate = value.ToVersion();
            Assert.IsNotNull(candidate, "Conversion failed");
            Assert.AreEqual(candidate.Major, 10);
            Assert.AreEqual(candidate.Minor, 0);
        }

        [Test]
        public void FromAsmToVersionWithMinorValue()
        {
            var value = "V10_4";
            var candidate = value.ToVersion();
            Assert.IsNotNull(candidate, "Conversion failed");
            Assert.AreEqual(candidate.Major, 10);
            Assert.AreEqual(candidate.Minor, 4);
        }

        [Test]
        public void FromNumberToAsmVersionString()
        {
            var value = "10";
            var candidate = value.ToAsmVersion("Cts");
            Assert.AreEqual("Cts.V10", candidate);
        }

        [Test]
        public void FromNumberWithMinorValueToAsmVersionString()
        {
            var value = "10.2";
            var candidate = value.ToAsmVersion("Cts");
            Assert.AreEqual("Cts.V10_2", candidate);
        }

        [Test]
        public void FromNumberToAsmVersionStringNoSchema()
        {
            var value = "10";
            var candidate = value.ToAsmVersion(null);
            Assert.AreEqual("V10", candidate);
        }

        [Test]
        public void FromNumberWithMinorValueToAsmVersionStringNoSchema()
        {
            var value = "10.2";
            var candidate = value.ToAsmVersion(null);
            Assert.AreEqual("V10_2", candidate);
        }

        [Test]
        public void RoundTripAsmVersion()
        {
            var value = "Cts.V10_2";
            var candidate = value.ToAsmVersion("Cts");
            Assert.AreEqual("Cts.V10_2", candidate);
        }

        [Test]
        public void NullAsmVersion()
        {
            string value = null;
            var candidate = value.ToAsmVersion("Cts");
            Assert.IsNull(candidate);
        }

        [Test]
        public void NullAsmVersionFromEmptyString()
        {
            string value = string.Empty;
            var candidate = value.ToAsmVersion(null);
            Assert.IsNull(candidate);
        }

        [Test]
        public void GetCorrectVersionStringFromAsmVersion()
        {
            var asm = "V10_4";
            var candidate = asm.ToVersionString();
            Assert.AreEqual("10.4", candidate);

            asm = "V10";
            candidate = asm.ToVersionString();
            Assert.AreEqual("10.0", candidate);
        }

        [Test]
        public void NullVersionStringFromEmptyString()
        {
            var asm = string.Empty;
            var candidate = asm.ToVersionString();
            Assert.AreEqual(null, candidate);
        }

        [Test]
        public void CorrectVersionObjectFromAsmVersion()
        {
            var asm = "V10_4";
            var candidate = asm.ToVersion();
            Assert.AreEqual(candidate.Major, 10);
            Assert.AreEqual(candidate.Minor, 4);

            asm = "V10";
            candidate = asm.ToVersion();
            Assert.AreEqual(candidate.Major, 10);
            Assert.AreEqual(candidate.Minor, 0);
        }

        [Test]
        public void CorrectAsmVersionFromVersionObject()
        {
            var candidate = new Version(10, 4).ToAsmVersion();
            Assert.AreEqual("V10_4", candidate);

            candidate = new Version(10, 0).ToAsmVersion();
            Assert.AreEqual("V10", candidate);
        }

        [Test]
        public void ToAsmSchemaWithNull()
        {
            string source = null;
            var candidate = source.ToAsmSchema();
            Assert.AreEqual(null, candidate);
        }

        [Test]
        public void ToAsmSchemaWithEmptyString()
        {
            string source = string.Empty;
            var candidate = source.ToAsmSchema();
            Assert.AreEqual(null, candidate);
        }

        [Test]
        public void ToAsmSchemaNoSchema()
        {
            var source = "V1_1";
            var candidate = source.ToAsmSchema();
            Assert.AreEqual(null, candidate);
        }

        [Test]
        public void ToAsmSchemaWithMinorVersion()
        {
            var source = "Cts.V1_1";
            var candidate = source.ToAsmSchema();
            Assert.AreEqual("Cts", candidate);
        }

        [Test]
        public void ToAsmSchemaNoMinorVersion()
        {
            var source = "Cts.V1";
            var candidate = source.ToAsmSchema();
            Assert.AreEqual("Cts", candidate); 
        }
    }
}