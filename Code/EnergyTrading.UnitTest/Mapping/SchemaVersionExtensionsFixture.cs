namespace EnergyTrading.UnitTest.Mapping
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Mapping;

    [TestClass]
    public class SchemaVersionExtensionsFixture : Fixture
    {        
        [TestMethod]
        public void FromNumberToVersionString()
        {
            var value = "10";
            var candidate = value.ToVersionString();
            Assert.AreEqual("10.0", candidate);
        }

        [TestMethod]
        public void FromNumberToVersionStringWithMinorValue()
        {
            var value = "10.4";
            var candidate = value.ToVersionString();
            Assert.AreEqual("10.4", candidate);
        }

        [TestMethod]
        public void FromAsmToVersionString()
        {
            var value = "V10";
            var candidate = value.ToVersionString();
            Assert.AreEqual("10.0", candidate);
        }

        [TestMethod]
        public void FromAsmToVersionStringWithMinorValue()
        {
            var value = "V10_4";
            var candidate = value.ToVersionString();
            Assert.AreEqual("10.4", candidate);
        }

        [TestMethod]
        public void FromAsmToVersion()
        {
            var value = "V10";
            var candidate = value.ToVersion();
            Assert.IsNotNull(candidate, "Conversion failed");
            Assert.AreEqual(candidate.Major, 10);
            Assert.AreEqual(candidate.Minor, 0);
        }

        [TestMethod]
        public void FromAsmToVersionWithMinorValue()
        {
            var value = "V10_4";
            var candidate = value.ToVersion();
            Assert.IsNotNull(candidate, "Conversion failed");
            Assert.AreEqual(candidate.Major, 10);
            Assert.AreEqual(candidate.Minor, 4);
        }

        [TestMethod]
        public void FromNumberToAsmVersionString()
        {
            var value = "10";
            var candidate = value.ToAsmVersion("Cts");
            Assert.AreEqual("Cts.V10", candidate);
        }

        [TestMethod]
        public void FromNumberWithMinorValueToAsmVersionString()
        {
            var value = "10.2";
            var candidate = value.ToAsmVersion("Cts");
            Assert.AreEqual("Cts.V10_2", candidate);
        }

        [TestMethod]
        public void FromNumberToAsmVersionStringNoSchema()
        {
            var value = "10";
            var candidate = value.ToAsmVersion(null);
            Assert.AreEqual("V10", candidate);
        }

        [TestMethod]
        public void FromNumberWithMinorValueToAsmVersionStringNoSchema()
        {
            var value = "10.2";
            var candidate = value.ToAsmVersion(null);
            Assert.AreEqual("V10_2", candidate);
        }

        [TestMethod]
        public void RoundTripAsmVersion()
        {
            var value = "Cts.V10_2";
            var candidate = value.ToAsmVersion("Cts");
            Assert.AreEqual("Cts.V10_2", candidate);
        }

        [TestMethod]
        public void NullAsmVersion()
        {
            string value = null;
            var candidate = value.ToAsmVersion("Cts");
            Assert.IsNull(candidate);
        }

        [TestMethod]
        public void NullAsmVersionFromEmptyString()
        {
            string value = string.Empty;
            var candidate = value.ToAsmVersion(null);
            Assert.IsNull(candidate);
        }

        [TestMethod]
        public void GetCorrectVersionStringFromAsmVersion()
        {
            var asm = "V10_4";
            var candidate = asm.ToVersionString();
            Assert.AreEqual("10.4", candidate);

            asm = "V10";
            candidate = asm.ToVersionString();
            Assert.AreEqual("10.0", candidate);
        }

        [TestMethod]
        public void NullVersionStringFromEmptyString()
        {
            var asm = string.Empty;
            var candidate = asm.ToVersionString();
            Assert.AreEqual(null, candidate);
        }

        [TestMethod]
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

        [TestMethod]
        public void CorrectAsmVersionFromVersionObject()
        {
            var candidate = new Version(10, 4).ToAsmVersion();
            Assert.AreEqual("V10_4", candidate);

            candidate = new Version(10, 0).ToAsmVersion();
            Assert.AreEqual("V10", candidate);
        }

        [TestMethod]
        public void ToAsmSchemaWithNull()
        {
            string source = null;
            var candidate = source.ToAsmSchema();
            Assert.AreEqual(null, candidate);
        }

        [TestMethod]
        public void ToAsmSchemaWithEmptyString()
        {
            string source = string.Empty;
            var candidate = source.ToAsmSchema();
            Assert.AreEqual(null, candidate);
        }

        [TestMethod]
        public void ToAsmSchemaNoSchema()
        {
            var source = "V1_1";
            var candidate = source.ToAsmSchema();
            Assert.AreEqual(null, candidate);
        }

        [TestMethod]
        public void ToAsmSchemaWithMinorVersion()
        {
            var source = "Cts.V1_1";
            var candidate = source.ToAsmSchema();
            Assert.AreEqual("Cts", candidate);
        }

        [TestMethod]
        public void ToAsmSchemaNoMinorVersion()
        {
            var source = "Cts.V1";
            var candidate = source.ToAsmSchema();
            Assert.AreEqual("Cts", candidate); 
        }
    }
}