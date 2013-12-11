namespace EnergyTrading.Mapping
{
    using System;
    using System.Reflection;
    using System.Xml.Linq;

    using EnergyTrading.Logging;

    /// <summary>
    /// Decorator version of an <see cref="IXmlVersionDetector" />.
    /// Takes an array of version detectors and asks each one until it finds one 
    /// who responds or the list is exhausted.
    /// </summary>
    public class XmlVersionDetector : IXmlVersionDetector
    {
        private static readonly ILogger Logger = LoggerFactory.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IXmlVersionDetector[] detectors;

        /// <summary>
        /// Creates a new instance of the XmlVersionDetector class.
        /// </summary>
        /// <param name="detectors"></param>
        public XmlVersionDetector(IXmlVersionDetector[] detectors)
        {
            this.detectors = detectors;
        }

        /// <copydocfrom cref="IXmlVersionDetector.DetectSchemaVersion(string)" />
        public string DetectSchemaVersion(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml))
            {
                return string.Empty;
            }

            XDocument document;
            try
            {
                document = XDocument.Parse(xml); 
            }
            catch (Exception ex)
            {
                Logger.Error("Invalid XML", ex);
                throw new NotSupportedException("Invalid XML", ex);
            }

            return DetectSchemaVersion(document.Root);
        }

        /// <copydocfrom cref="IXmlVersionDetector.DetectSchemaVersion(XElement)" />
        public string DetectSchemaVersion(XElement element)
        {
            if (element == null)
            {
                return string.Empty;
            }

            foreach (var detector in detectors)
            {
                string version;
                try
                {
                    version = detector.DetectSchemaVersion(element);
                }
                catch (Exception ex)
                {
                    Logger.Error("Detector failure: " + detector.GetType().Name, ex);
                    continue;
                }

                // Didn't respond or said a blanket unknown
                if (string.IsNullOrEmpty(version) || version == "Unknown")
                {
                    continue;
                }

                // Handles Schema.Unknown
                var parts = version.Split('.');
                if (parts.GetUpperBound(0) > 0 && parts[1] == "Unknown")
                {
                    continue;
                }

                return version;
            }

            // No-one liked it.
            return string.Empty;
        }
    }
}