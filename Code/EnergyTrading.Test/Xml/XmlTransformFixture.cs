namespace EnergyTrading.Test.Xml
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;

    using EnergyTrading.Container.Unity;
    using EnergyTrading.Mapping;
    using EnergyTrading.Test;
    using EnergyTrading.Xml;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Test class for proving XML transformations/mapping.
    /// </summary>
    /// <example>
    /// Derive from this class settings the parameters in the overridden <see cref="Fixture.OnSetup" /> method
    /// and adding individual files as test methods.   
    /// <code>
    /// [TestClass]
    /// public class CtsRoundTripFixture : SamplesTransformFixture{TradePayload}
    /// {
    ///     [TestMethod]
    ///     [TestCategory("BioFuels"), TestCategory("Swap")]
    ///     public virtual void BioFuelsSwapFixedFloat_10121947()
    ///     {
    ///         SourceFile = "BioFuels_Swap_FixedFloat.xml";
    ///         RunTest();
    ///     }
    /// 
    ///     protected override void OnSetup()
    ///     {
    ///         // Set if the samples are embedded resources
    ///         //ResourceNamespace = "RegulatoryReporting.Core.IntegrationTest.Data";
    ///         Schema = "Cts";
    ///         Version = "2_3";
    /// 
    ///         DiffFilePath = "Diffs";
    ///         GenerateDiffFile = true;
    ///         ValidateOutput = true;
    ///
    ///         // Need to register the ADM mappers we need
    ///         new XmlSchemaRegistryRegistrar().Register(Container);
    ///         new XmlVersionDetectorRegistrar().Register(Container);
    ///
    ///         new CtsMapperRegistrar().Register(Container);
    ///     }
    /// }
    /// </code>
    /// </example>
    /// <typeparam name="T">Type of entity to use.</typeparam>
    public abstract class XmlTransformFixture<T> : XmlFixture
    {
        private string schema;
        private string version;
        private string targetFile;

        /// <summary>
        /// Gets whether the test is resource rather than file based.
        /// <para>
        /// Note that if resource based, both the source and target must be resources
        /// </para>
        /// </summary>
        protected bool IsResourceBased
        {
            get { return !string.IsNullOrEmpty(ResourceNamespace); }
        }

        /// <summary>
        /// Whether to generate the HTML difference file.
        /// </summary>
        protected bool GenerateDiffFile { get; set; }

        /// <summary>
        /// Whether to clear difference file directory
        /// </summary>
        protected bool ClearDiffPath { get; set; }

        /// <summary>
        /// Whether to generate the output file
        /// </summary>
        protected bool GenerateOutputFile { get; set; }

        /// <summary>
        /// Directory used for output files
        /// </summary>
        protected string OutputPath { get; set; }

        /// <summary>
        /// Whether to clear output file directory
        /// </summary>
        protected bool ClearOutputPath { get; set; }

        /// <summary>
        /// Gets or sets the source schema.
        /// </summary>
        protected string Schema
        {
            get { return schema; }
            set
            {
                schema = value;
                TargetSchema = value;
            }
        }

        /// <summary>
        /// Gets or sets the source version.
        /// </summary>
        protected string Version
        {
            get { return version; }
            set
            {
                version = value;
                TargetVersion = value;
            }
        }

        /// <summary>
        /// Gets or sets whether we validate the output according to the schema.
        /// <para>
        /// The XmlSchemaSet must have been registered for this to apply.
        /// </para>
        /// </summary>
        protected bool ValidateOutput { get; set; }

        /// <summary>
        /// Gets or sets the resource namespace, the prefix to the resource files.
        /// </summary>
        protected string ResourceNamespace { get; set; }

        /// <summary>
        /// Get or set the source file excluding schema/version.
        /// </summary>
        protected string SourceFile { get; set; }

        /// <summary>
        /// Get or set the source file excluding schema/version.
        /// <para>
        /// If empty, defaults to <see cref="SourceFile" />
        /// </para>
        /// </summary>
        protected virtual string TargetFile
        {
            get { return string.IsNullOrEmpty(targetFile) ? SourceFile : targetFile; }
            set
            {
                if (value == SourceFile)
                {
                    value = null;
                }
                targetFile = value;
            }
        }

        /// <summary>
        /// Gets or sets the target schema.
        /// </summary>
        protected string TargetSchema { get; set; }

        /// <summary>
        /// Gets or sets the target version.
        /// </summary>
        protected string TargetVersion { get; set; }

        /// <summary>
        /// Run a test using the current parameters.
        /// </summary>
        public void RunTest()
        {
            var sourceXml = Extract();
            var candidate = Transform(sourceXml);
            Check(candidate);
        }

        /// <summary>
        /// Check the source XML against the transformed candidate value.
        /// </summary>
        /// <param name="candidate">XElement to compare.</param>
        protected virtual void Check(XElement candidate)
        {
            if (string.IsNullOrEmpty(TargetFile))
            {
                Assert.Fail("No TargetFile specified");
            }

            if (string.IsNullOrEmpty(TargetSchema))
            {
                Assert.Fail("No TargetSchema specified");
            }

            if (string.IsNullOrEmpty(TargetVersion))
            {
                Assert.Fail("No TargetVersion specified");
            }

            var targetXml = GetXml(TargetFile, TargetSchema, TargetVersion);
            if (GenerateDiffFile)
            {
                if (!DiffFilePath.EndsWith("\\"))
                {
                    DiffFilePath += "\\";
                }
                var ext = Schema == TargetSchema ? Schema : Schema + TargetSchema;
                DiffFilePath = DiffFilePath + TargetFile.Substring(0, TargetFile.IndexOf(".", StringComparison.InvariantCultureIgnoreCase)) + "_" + ext + ".html";
            }
            else
            {
                DiffFilePath = string.Empty;
            }

            if (ValidateOutput)
            {
                SchemaValidate(targetXml, TargetSchema, TargetVersion);
            }

            CheckXml(targetXml, candidate);
        }

        /// <summary>
        /// Extract the source XML using the current parameters.
        /// </summary>
        /// <returns>String value containing the source XML.</returns>
        protected virtual string Extract()
        {
            if (string.IsNullOrEmpty(SourceFile))
            {
                Assert.Fail("No SourceFile specified");
            }

            if (string.IsNullOrEmpty(Schema))
            {
                Assert.Fail("No Schema specified");
            }

            if (string.IsNullOrEmpty(Version))
            {
                Assert.Fail("No Version specified");
            }

            return GetXml(SourceFile, Schema, Version);
        }

        /// <summary>
        /// Transform the source XML. 
        /// </summary>
        /// <param name="sourceXml">XML to use.</param>
        /// <returns>Transformed XML as an XElement.</returns>
        protected virtual XElement Transform(string sourceXml)
        {
            var entity = GetEntity(sourceXml);
            if (entity == null)
            {
                Assert.Fail("Could not deserialize source");
            }

            var output = SerializeEntity(entity);
            if (output == null)
            {
                Assert.Fail("Could not serialize entity");
            }

            if (GenerateOutputFile)
            {
                var path = OutputPath + "\\" + TargetFile;
                output.Save(path);
            }

            return output;
        }

        /// <summary>
        /// Convert XML into an entity using the <see cref="Schema"/> engine.
        /// </summary>
        /// <param name="xml">XML to use</param>
        /// <returns>A converted entity.</returns>
        protected virtual T GetEntity(string xml)
        {
            var sourceEngine = Container.Resolve<IXmlMappingEngine>(Version.ToAsmVersion(Schema));

            // NOTE: Should be LinqXPathProcessor but getting weird serialization errors
            var processor = new XPathProcessor();
            processor.Initialize(xml);
            return sourceEngine.Map<XPathProcessor, T>(processor);
        }

        /// <summary>
        /// Serialize an entity to XML using the <see cref="TargetSchema" /> engine.
        /// </summary>
        /// <param name="entity">Entity to use</param>
        /// <returns>Serialized XML as an XElement.</returns>
        protected virtual XElement SerializeEntity(T entity)
        {
            var targetEngine = Container.Resolve<IXmlMappingEngine>(TargetVersion.ToAsmVersion(TargetSchema));

            return targetEngine.CreateDocument(entity);
        }

        /// <summary>
        /// Get XML from a file or embedded resource.
        /// </summary>
        /// <param name="fileName">Filename to use</param>
        /// <param name="schema">Schema to use.</param>
        /// <param name="version">Version to use.</param>
        /// <returns>Contents of the file</returns>
        protected string GetXml(string fileName, string schema, string version)
        {
            return IsResourceBased 
                ? GetEmbeddedResource(fileName, schema, version) 
                : GetFileResource(fileName, schema, version);
        }

        /// <summary>
        /// Get XML from an embedded resource.
        /// </summary>
        /// <param name="fileName">Filename to use</param>
        /// <param name="schema">Schema to use.</param>
        /// <param name="version">Version to use.</param>
        /// <returns>Contents of the file</returns>
        protected string GetEmbeddedResource(string fileName, string schema, string version)
        {
            var resourceName = string.Format("{0}.{1}.V{2}.{3}", ResourceNamespace, schema, version, fileName);
            using (var stream = GetType().Assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new ArgumentException(string.Format("Could not find embedded resouce '{0}'.", resourceName));
                }

                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Get XML from a file.
        /// </summary>
        /// <param name="fileName">Filename to use</param>
        /// <param name="schema">Schema to use.</param>
        /// <param name="version">Version to use.</param>
        /// <returns>Contents of the file</returns>
        protected string GetFileResource(string fileName, string schema, string version)
        {
            var resourceName = string.Format(@"Data\{0}\V{1}\{2}", schema, version, fileName);

            return LoadXmlFromFile(resourceName);
        }

        /// <summary>
        /// Write content to a file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="content"></param>
        protected static void WriteToFile(string filePath, string content)
        {
            var dirPath = Path.GetDirectoryName(filePath);
            if (dirPath != null && !Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            File.WriteAllText(filePath, content);
            Console.Out.WriteLine("Finished writing file {0}", filePath);
        }

        /// <summary>
        /// Validate XML against a schema.
        /// </summary>
        /// <param name="xml"></param>
        protected virtual void SchemaValidate(string xml, string schema, string version)
        {
            var validator = Container.Resolve<IXmlSchemaValidator>();
            var schemaVersion = schema + ".V" + version;
            if (validator.GetSchemaSet(schemaVersion) == null)
            {
                return;
            }

            try
            {
                var errors = string.Empty;
                var validation = validator.Validate(xml, schemaVersion);
                if (validation.Count != 0)
                {
                    errors += validation.Aggregate(
                        string.Empty,
                        (current, error) => current + (error.Message + Environment.NewLine));
                }

                var xpaths = validator.GetXPathValidator(schemaVersion);
                if (xpaths != null)
                {
                    var xpathErrors = validator.ValidateXPaths(xml, schemaVersion);
                    if (validation.Count != 0)
                    {
                        errors += xpathErrors.Aggregate(
                            string.Empty,
                            (current, error) => current + (error + Environment.NewLine));
                    }
                }

                if (!string.IsNullOrEmpty(errors))
                {
                    Assert.Fail(errors);
                }
            }
            catch (AssertFailedException)
            {
                throw;
            }
            catch (Exception)
            {
                // Swallow
            }
        }
    }
}