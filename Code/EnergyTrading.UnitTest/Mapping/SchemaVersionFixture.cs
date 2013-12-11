namespace EnergyTrading.UnitTest.Mapping
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Mapping;

    [TestClass]
    public class SchemaVersionFixture : Fixture
    {
        [TestMethod]
        public void FromNullToSchemaVersion()
        {
            string value = null;
            var candidate = value.ToSchemaVersion();
            Assert.IsNull(candidate, "Candidate not null");
        }

        [TestMethod]
        public void FromSchemaMajorToSchemaVersion()
        {
            string value = "Cts.V2";
            var expected = new SchemaVersion { Schema = "Cts", Version = new Version(2, 0) };
            var candidate = value.ToSchemaVersion();
            Check(expected, candidate);
        }

        [TestMethod]
        public void FromSchemaMajorMinorToSchemaVersion()
        {
            string value = "Cts.V2_1";
            var expected = new SchemaVersion { Schema = "Cts", Version = new Version(2, 1) };
            var candidate = value.ToSchemaVersion();
            Check(expected, candidate);
        }

        [TestMethod]
        public void FromMajorToSchemaVersion()
        {
            string value = "V2";
            var expected = new SchemaVersion { Version = new Version(2, 0) };
            var candidate = value.ToSchemaVersion();
            Check(expected, candidate);
        }

        [TestMethod]
        public void FromSchemaVersionToNull()
        {
            SchemaVersion value = null;
            var candidate = value.ToAsmVersion();
            Assert.IsNull(candidate, "Candidate not null");
        }

        [TestMethod]
        public void FromSSchemaVersionTochemaMajor()
        {
            string expected = "Cts.V2";
            var value = new SchemaVersion { Schema = "Cts", Version = new Version(2, 0) };
            var candidate = value.ToString();
            Check(expected, candidate);
        }

        [TestMethod]
        public void FromSchemaVersionToSchemaMajorMinor()
        {
            string expected = "Cts.V2_1";
            var value = new SchemaVersion { Schema = "Cts", Version = new Version(2, 1) };
            var candidate = value.ToString();
            Check(expected, candidate);
        }

        [TestMethod]
        public void FromSchemaVersionToMajor()
        {
            string expected = "V2";
            var value = new SchemaVersion { Version = new Version(2, 0) };
            var candidate = value.ToString();
            Check(expected, candidate);
        }
    }
}
