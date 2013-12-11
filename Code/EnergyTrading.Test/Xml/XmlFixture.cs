namespace EnergyTrading.Test.Xml
{
    using System;
    using System.IO;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Schema;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Test.Mapping;

    /// <summary>
    /// Base class for XML comparison tests
    /// </summary>
    public abstract class XmlFixture : Fixture
    {
        /// <summary>
        /// Gets or set the diff file path.
        /// <para>
        /// If set, calls to CheckXml will emit the HTML difference file.
        /// </para>
        /// </summary>
        protected string DiffFilePath { get; set; }

        protected static void WriteXmlToConsole(string xml)
        {
            var doc = new XmlDocument();
            doc.Load(new StringReader(xml));
            doc.Save(Console.Out);
        }

        protected static string LoadXmlFromFile(string fileName)
        {
            var fileInfo = new FileInfo(fileName);
            using (var reader = fileInfo.OpenText())
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Check a expected XML against some candidate XML.
        /// </summary>
        /// <param name="expectedXml">Original XML</param>        
        /// <param name="candidateXml">XML to compare against.</param>
        /// <param name="fullComparison">Whether to do a full comparison using prefix/namespace</param>
        protected void CheckXml(string expectedXml, XElement candidateXml, bool fullComparison = true)
        {
            var result = expectedXml.CheckXml(candidateXml, fullComparison, DiffFilePath);
            if (!string.IsNullOrEmpty(result))
            {
                Assert.Fail(result);
            }
        }

        /// <summary>
        /// Check a expected XML against some candidate XML.
        /// </summary>
        /// <param name="expectedXml">Original XML</param>        
        /// <param name="candidateXml">XML to compare against.</param>
        /// <param name="fullComparison">Whether to do a full comparison using prefix/namespace</param>
        protected void CheckXml(string expectedXml, string candidateXml, bool fullComparison = true)
        {
            var result = expectedXml.CheckXml(candidateXml, fullComparison, DiffFilePath);
            if (!string.IsNullOrEmpty(result))
            {
                Assert.Fail(result);
            }
        }

        /// <summary>
        /// Check an expected XElement against some candidate XML.
        /// </summary>
        /// <param name="expectedNode">XElement to use</param>
        /// <param name="candidateXml">XML to compare against.</param>
        /// <param name="fullComparison">Whether to do a full comparison using prefix/namespace</param>
        protected void CheckXml(XElement expectedNode, string candidateXml, bool fullComparison = true)
        {
            var result = expectedNode.CheckXml(candidateXml, fullComparison, DiffFilePath);
            if (!string.IsNullOrEmpty(result))
            {
                Assert.Fail(result);
            }
        }

        /// <summary>
        /// Check an expected XElement against some candidate XML.
        /// </summary>
        /// <param name="expected">XElement to use</param>
        /// <param name="candidate">XML to compare against.</param>
        /// <param name="fullComparison">Whether to do a full comparison using prefix/namespace</param>
        protected void CheckXml(XElement expected, XElement candidate, bool fullComparison = true)
        {
            var result = expected.CheckXml(candidate, fullComparison, DiffFilePath);
            if (!string.IsNullOrEmpty(result))
            {
                Assert.Fail(result);
            }
        }

        public void VerifyXmlFile(string path)
        {
            // Configure the xmlreader validation to use inline schema.
            var config = new XmlReaderSettings { ValidationType = ValidationType.Schema };
            config.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            config.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
            config.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
            config.ValidationEventHandler += ValidationCallBack;

            // Get the XmlReader object with the configured settings.
            var reader = XmlReader.Create(path, config);

            // Parsing the file will cause the validation to occur.
            while (reader.Read())
            {
            }
        }

        private void ValidationCallBack(object sender, ValidationEventArgs vea)
        {
            if (vea.Severity == XmlSeverityType.Warning)
            {
                Console.WriteLine("\tWarning: Matching schema not found.  No validation occurred. {0}", vea.Message);
            }
            else
            {
                Console.WriteLine("\tValidation error: {0}", vea.Message);
            }
        }
    }
}