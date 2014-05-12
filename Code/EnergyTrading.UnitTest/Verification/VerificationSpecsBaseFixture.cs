﻿namespace EnergyTrading.UnitTest.Verification
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using EnergyTrading.Test.Verification;

    using NUnit.Framework;

    [TestFixture]
    public class VerificationSpecsBaseFixture
    {
        private const string TestInputXml = "<LegalEntity><Id xmlns=\"http://rwe.com/schema/common/3\"><SystemId><SystemID>EnergyTrading</SystemID><Identifier>1872</Identifier><OriginatingSourceIND>true</OriginatingSourceIND></SystemId><SystemId xmlns=\"http://rwe.com/schema/common/3\"><SystemID>Moff</SystemID><Identifier>1872</Identifier><OriginatingSourceIND>false</OriginatingSourceIND></SystemId></Id><Details xmlns=\"http://rwe.com/schema/party/2\"><ShortName>RWE TRADING</ShortName><LongName>RWE TRADING</LongName><PartyType>Counterparty</PartyType></Details></LegalEntity>";
        private const string TestInputWithWhiteSpace = "<LegalEntity><Id xmlns=\"http://rwe.com/schema/common/3\"><SystemId>  <SystemID>EnergyTrading</SystemID>\n<Identifier>1872</Identifier>   \n<OriginatingSourceIND>true</OriginatingSourceIND></SystemId><SystemId xmlns=\"http://rwe.com/schema/common/3\"><SystemID>Moff</SystemID><Identifier>1872</Identifier><OriginatingSourceIND>false</OriginatingSourceIND></SystemId></Id><Details xmlns=\"http://rwe.com/schema/party/2\"><ShortName>RWE TRADING</ShortName><LongName>RWE TRADING</LongName><PartyType>Counterparty</PartyType></Details></LegalEntity>";
        private const string TestInputWithCommonPrefix = "<LegalEntity><Id xmlns=\"http://rwe.com/schema/common/3\"><SystemId><SystemID>EnergyTrading</SystemID><Identifier>1872</Identifier><OriginatingSourceIND>true</OriginatingSourceIND></SystemId><common:SystemId xmlns=\"http://rwe.com/schema/common/3\"><common:SystemID>Moff</common:SystemID><common:Identifier>1872</common:Identifier><common:OriginatingSourceIND>false</common:OriginatingSourceIND></common:SystemId></Id><Details xmlns=\"http://rwe.com/schema/party/2\"><ShortName>RWE TRADING</ShortName><LongName>RWE TRADING</LongName><PartyType>Counterparty</PartyType></Details></LegalEntity>";

        private const string XmlNoEnergyTradingId = "<LegalEntity><Id xmlns=\"http://rwe.com/schema/common/3\"><SystemId xmlns=\"http://rwe.com/schema/common/3\"><SystemID>Moff</SystemID><Identifier>1872</Identifier><OriginatingSourceIND>false</OriginatingSourceIND></SystemId></Id><Details xmlns=\"http://rwe.com/schema/party/2\"><ShortName>RWE TRADING</ShortName><LongName>RWE TRADING</LongName><PartyType>Counterparty</PartyType></Details></LegalEntity>";

        private const string XmlNoMoffId = "<LegalEntity><Id xmlns=\"http://rwe.com/schema/common/3\"><SystemId><SystemID>EnergyTrading</SystemID><Identifier>1872</Identifier><OriginatingSourceIND>true</OriginatingSourceIND></SystemId></Id><Details xmlns=\"http://rwe.com/schema/party/2\"><ShortName>RWE TRADING</ShortName><LongName>RWE TRADING</LongName><PartyType>Counterparty</PartyType></Details></LegalEntity>";

        private const string XmlNoIds = "<LegalEntity><Id xmlns=\"http://rwe.com/schema/common/3\"></Id><Details xmlns=\"http://rwe.com/schema/party/2\"><ShortName>RWE TRADING</ShortName><LongName>RWE TRADING</LongName><PartyType>Counterparty</PartyType></Details></LegalEntity>";

        private const string XmlNoPartyType = "<LegalEntity><Id xmlns=\"http://rwe.com/schema/common/3\"><SystemId><SystemID>EnergyTrading</SystemID><Identifier>1872</Identifier><OriginatingSourceIND>true</OriginatingSourceIND></SystemId><SystemId xmlns=\"http://rwe.com/schema/common/3\"><SystemID>Moff</SystemID><Identifier>1872</Identifier><OriginatingSourceIND>false</OriginatingSourceIND></SystemId></Id><Details xmlns=\"http://rwe.com/schema/party/2\"><ShortName>RWE TRADING</ShortName><LongName>RWE TRADING</LongName><PartyType></PartyType></Details></LegalEntity>";

        private const string XmlNoPartyTypeOrLongName = "<LegalEntity><Id xmlns=\"http://rwe.com/schema/common/3\"><SystemId><SystemID>EnergyTrading</SystemID><Identifier>1872</Identifier><OriginatingSourceIND>true</OriginatingSourceIND></SystemId><SystemId xmlns=\"http://rwe.com/schema/common/3\"><SystemID>Moff</SystemID><Identifier>1872</Identifier><OriginatingSourceIND>false</OriginatingSourceIND></SystemId></Id><Details xmlns=\"http://rwe.com/schema/party/2\"><ShortName>RWE TRADING</ShortName><LongName></LongName><PartyType></PartyType></Details></LegalEntity>";

        private const string TestWithOriginatingId = "<LegalEntity><Id xmlns=\"http://rwe.com/schema/common/3\"><SystemId><SystemID>EnergyTrading</SystemID><Identifier>1872</Identifier><OriginatingSourceIND>true</OriginatingSourceIND></SystemId></Id><Details xmlns=\"http://rwe.com/schema/party/2\"><ShortName>RWE TRADING</ShortName><LongName>RWE TRADING</LongName><PartyType>Counterparty</PartyType></Details></LegalEntity>";

        private class VerificationSpecsBaseDerived : VerificiationSpecsBase
        {
            public string CallRemoveIdEntriesForSystem(string system, string source)
            {
                return this.RemoveIdEntriesForSystem(system, source);
            }

            public string CallRemoveIdEntriesForSystems(IEnumerable<string> systems, string source)
            {
                return this.RemoveIdEntriesForSystems(systems, source);
            }

            public string CallRemoveElementValue(string element, string source)
            {
                return this.RemoveElementValue(element, source);
            }

            public string CallRemoveElementValues(IEnumerable<string> elementNames, string source)
            {
                return this.RemoveElementValues(elementNames, source);
            }

            protected override string RemoveDynamicValues(string source)
            {
                return string.Empty;
            }

            public string CallLoadEmbeddedResource(string file)
            {
                return this.LoadEmbeddedResource(file);
            }

            public byte[] CallLoadEmbeddedResourceInBytes(string file)
            {
                return this.LoadEmbeddedResourceInBytes(file);
            }

            public bool CallHasIdForSystem(string system, string source)
            {
                return this.HasIdForSystem(system, source);
            }

            public string RemoveAllNonOriginatingIds(string source)
            {
                return this.RemoveAllNonOriginatingSystemIds(source);
            }

            public bool CheckReceviedResults(IDictionary<Guid, VerificationResult<Guid>> resultSet, Guid testId)
            {
                return this.VerifyReceviedResults(resultSet, testId);
            }

            public bool CheckReceviedResults(IDictionary<string, VerificationResult<string>> resultSet, string testId)
            {
                return this.VerifyReceviedResults(resultSet, testId);
            }
        }

        [Test]
        public void HasIdReturnsFalseForNonExistingSystem()
        {
            var candidate = new VerificationSpecsBaseDerived().CallHasIdForSystem("testsystem", TestInputXml);
            Assert.IsFalse(candidate);
        }

        [Test]
        public void HasIdReturnsTrueForExistingSystem()
        {
            var candidate = new VerificationSpecsBaseDerived().CallHasIdForSystem("Moff", TestInputXml);
            Assert.IsTrue(candidate);
        }

        [Test]
        public void RemoveAllNonOriginatingIds()
        {
            var candidate = new VerificationSpecsBaseDerived().RemoveAllNonOriginatingIds(TestInputXml);
            Assert.AreEqual(TestWithOriginatingId, candidate);
        }

        [Test]
        public void RemoveAllNonOriginatingIdsWithCommonPrefix()
        {
            var candidate = new VerificationSpecsBaseDerived().RemoveAllNonOriginatingIds(TestInputWithCommonPrefix);
            Assert.AreEqual(TestWithOriginatingId, candidate);
        }

        [Test]
        public void RemoveIdEntriesForNonExistingSystem()
        {
            var candidate = new VerificationSpecsBaseDerived().CallRemoveIdEntriesForSystem("notExisting", TestInputXml);
            Assert.AreEqual(TestInputXml, candidate);
        }

        [Test]
        public void RemoveIdForSingleSystemNoNamespace()
        {
            var candidate = new VerificationSpecsBaseDerived().CallRemoveIdEntriesForSystem("EnergyTrading", TestInputXml);
            Assert.AreEqual(XmlNoEnergyTradingId, candidate);
        }

        [Test]
        public void CanCopeWithWhiteSpaceBetweenElements()
        {
            var candidate = new VerificationSpecsBaseDerived().CallRemoveIdEntriesForSystem("EnergyTrading", TestInputWithWhiteSpace);
            Assert.AreEqual(XmlNoEnergyTradingId, candidate);
        }

        [Test]
        public void RemoveIdForSingleSystemWithNamespace()
        {
            var candidate = new VerificationSpecsBaseDerived().CallRemoveIdEntriesForSystem("Moff", TestInputXml);
            Assert.AreEqual(XmlNoMoffId, candidate);
        }

        [Test]
        public void RemoveMultipleSystemIdEntries()
        {
            var candidate = new VerificationSpecsBaseDerived().CallRemoveIdEntriesForSystems(new[] { "EnergyTrading", "Moff" }, TestInputXml);
            Assert.AreEqual(XmlNoIds, candidate);
        }

        [Test]
        public void RemoveElementValueforNonExistingElement()
        {
            var candidate = new VerificationSpecsBaseDerived().CallRemoveElementValue("noElementHere", TestInputXml);
            Assert.AreEqual(TestInputXml, candidate);
        }

        [Test]
        public void RemoveElementValueNoNamespace()
        {
            var candidate = new VerificationSpecsBaseDerived().CallRemoveElementValue("PartyType", TestInputXml);
            Assert.AreEqual(XmlNoPartyType, candidate);
        }

        [Test]
        public void RemoveMultipleElementValues()
        {
            var candidate = new VerificationSpecsBaseDerived().CallRemoveElementValues(new[] { "PartyType", "LongName" }, TestInputXml);
            Assert.AreEqual(XmlNoPartyTypeOrLongName, candidate);
        }

        [Test]
        public void CanLoadAnEmbeddedResource()
        {
            var candidate = new VerificationSpecsBaseDerived().CallLoadEmbeddedResource("EnergyTrading.UnitTest.Verification.Test.txt");
            Assert.AreEqual("hello world", candidate);
        }

        [Test]
        public void CanLoadAnEmbeddedResourceInBytes()
        {
            var candidate = new VerificationSpecsBaseDerived().CallLoadEmbeddedResourceInBytes("EnergyTrading.UnitTest.Verification.Test.txt");
            var stream = new MemoryStream(candidate);
            var actual = new StreamReader(stream).ReadToEnd();
            Assert.AreEqual("hello world", actual);
        }

        [Test]
        public void CanVerifyReceviedResultsGuid()
        {
            var guid = Guid.NewGuid();
            var testResult = new VerificationResult<Guid>() { TestId = guid };

            var resultSet = new Dictionary<Guid, VerificationResult<Guid>> { { guid, testResult } };
            var isResultExist = new VerificationSpecsBaseDerived().CheckReceviedResults(resultSet, guid);

            Assert.IsTrue(isResultExist);
        }

        [Test]
        public void CanVerifyReceviedResultsString()
        {
            var id = "testid";
            var testResult = new VerificationResult<string>() { TestId = id };

            var resultSet = new Dictionary<string, VerificationResult<string>> { { id, testResult } };
            var isResultExist = new VerificationSpecsBaseDerived().CheckReceviedResults(resultSet, id);

            Assert.IsTrue(isResultExist);
        }
    }
}