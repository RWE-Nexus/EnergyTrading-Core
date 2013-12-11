namespace EnergyTrading.Test.Mapping
{
    using System;
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;

    using Microsoft.XmlDiffPatch;

    using EnergyTrading.Xml.Linq;
    using EnergyTrading.Xml.Serialization;

    /// <summary>
    /// Creates <see cref="XmlDiff" /> instances and extension methods for comparisons.
    /// </summary>
    public static class XmlDiffFactory
    {
        /// <summary>
        /// Create a default XML difference engine using the <see cref="DefaultDiffOptions" />
        /// </summary>
        /// <returns></returns>
        public static XmlDiff DefaultDiffEngine()
        {
            var xmlDiff = new XmlDiff
            {
                Options = DefaultDiffOptions(),
                Algorithm = XmlDiffAlgorithm.Auto
            };

            return xmlDiff;
        }

        /// <summary>
        /// Return the default XML difference options to use.
        /// </summary>
        /// <returns></returns>
        public static XmlDiffOptions DefaultDiffOptions()
        {
            var diffOptions = new XmlDiffOptions();
            diffOptions = diffOptions | XmlDiffOptions.IgnorePI;
            diffOptions = diffOptions | XmlDiffOptions.IgnoreChildOrder;
            diffOptions = diffOptions | XmlDiffOptions.IgnoreComments;
            diffOptions = diffOptions | XmlDiffOptions.IgnoreDtd;
            diffOptions = diffOptions | XmlDiffOptions.IgnoreNamespaces;
            diffOptions = diffOptions | XmlDiffOptions.IgnorePrefixes;
            diffOptions = diffOptions | XmlDiffOptions.IgnoreWhitespace;
            diffOptions = diffOptions | XmlDiffOptions.IgnoreXmlDecl;

            return diffOptions;
        }

        /// <summary>
        /// Check an XElement against some expected XML.
        /// </summary>
        /// <param name="expectedXml">Original XML</param>        
        /// <param name="candidateXml">XML to compare against.</param>
        /// <param name="fullComparison">Whether to do a full comparison using prefix/namespace</param>
        /// <param name="diffFilePath">Where/whether to write out the HTML difference</param>
        public static string CheckXml(this string expectedXml, string candidateXml, bool fullComparison = true, string diffFilePath = "")
        {
            var source = XDocument.Parse(expectedXml);

            return source.Root.CheckXml(candidateXml, fullComparison, diffFilePath);
        }

        /// <summary>
        /// Check an XElement against some expected XML.
        /// </summary>
        /// <param name="expectedXml">Original XML</param>        
        /// <param name="candidateXml">XML to compare against.</param>
        /// <param name="fullComparison">Whether to do a full comparison using prefix/namespace</param>
        /// <param name="diffFilePath">Where/whether to write out the HTML difference</param>
        public static string CheckXml(this string expectedXml, XElement candidateXml, bool fullComparison = true, string diffFilePath = "")
        {
            var source = XDocument.Parse(expectedXml);

            return source.Root.CheckXml(candidateXml, fullComparison, diffFilePath);
        }

        /// <summary>
        /// Check an XElement against some expected XML.
        /// <para>
        /// The supplied node is copied and normalized to get actual differences.
        /// </para>
        /// </summary>
        /// <param name="expected">XElement to use</param>
        /// <param name="candidateXml">XML to compare against.</param>
        /// <param name="fullComparison">Whether to do a full comparison using prefix/namespace</param>
        /// <param name="diffFilePath">Where/whether to write out the HTML difference</param>
        public static string CheckXml(this XElement expected, string candidateXml, bool fullComparison = true, string diffFilePath = "")
        {
            var candidate = XDocument.Parse(candidateXml);

            return expected.CheckXml(candidate.Root, fullComparison, diffFilePath);
        }

        /// <summary>
        /// Check an XElement against some expected XML.
        /// <para>
        /// The supplied node is copied and normalized to get actual differences.
        /// </para>
        /// </summary>
        /// <param name="expected">XElement to use</param>
        /// <param name="candidate">XML to compare against.</param>
        /// <param name="fullComparison">Whether to do a full comparison using prefix/namespace</param>
        /// <param name="diffFilePath">Where/whether to write out the HTML difference</param>
        public static string CheckXml(this XElement expected, XElement candidate, bool fullComparison = true, string diffFilePath = "")
        {
            // Xml's are normalized before comparision, so that we can get actual difference when we turn on Namespaces & Prefix's differnces
            var source = new XDocument(expected);
            source = source.Normalize();
            var expectedNode = source.ToXmlDocument().SelectSingleNode("/*");

            var target = new XDocument(candidate);
            target = target.Normalize();
            var candidateNode = target.ToXmlDocument().SelectSingleNode("/*");

            using (var stringWriter = new StringWriter())
            {
                bool areDifferent;
                using (var xmlWriter = new XmlTextWriter(stringWriter) { Formatting = Formatting.Indented })
                {
                    // Default engine ignores prefix/namespace so we need to enable if we are doing a full comparison.
                    var diff = DefaultDiffEngine();
                    diff.IgnorePrefixes = !fullComparison;
                    diff.IgnoreNamespaces = !fullComparison;
                    areDifferent = !diff.Compare(expectedNode, candidateNode, xmlWriter);
                }

                if (areDifferent)
                {
                    if (!string.IsNullOrEmpty(diffFilePath))
                    {
                        var diffgram = stringWriter.GetStringBuilder().ToString();
                        WriteDiffFile(diffFilePath, diffgram, expectedNode);
                    }
                    return string.Format(
                            "Xml differs.{0}Expected:{0}{1}{0}{0}Actual:{0}{2}{0}{0}Diff:{0}{3}",
                            Environment.NewLine,
                            expectedNode.ToXmlString(),
                            candidateNode.ToXmlString(),
                            stringWriter);
                }
            }

            return string.Empty;
        }

        private static string GetHtmlDiff(string sourceXml, string diffgram)
        {
            var diffView = new XmlDiffView();
            diffView.Load(
                XmlReader.Create(new StringReader(sourceXml)),
                XmlReader.Create(new StringReader(diffgram)));

            var builder = new StringBuilder();
            builder.Append("<html><body><table>");
            diffView.GetHtml(new StringWriter(builder));
            builder.Append("</table></body></html>");
            return builder.ToString();
        }

        private static void WriteDiffFile(string diffFilePath, string diffgram, XmlNode expectedNode)
        {
            try
            {
                WriteToFile(diffFilePath, GetHtmlDiff(expectedNode.OuterXml, diffgram));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void WriteToFile(string filePath, string content)
        {
            var dirPath = Path.GetDirectoryName(filePath);
            if (dirPath != null && !Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            File.WriteAllText(filePath, content);
            Console.Out.WriteLine("Finished writing file {0}", filePath);
        }
    }
}