namespace EnergyTrading.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    using EnergyTrading.Xml.Linq;

    /// <summary>
    /// Extends <see cref="SimpleMapper{TSource, TDestionation}" /> to handle mapping from an entity to <see cref="System.Xml.Linq.XElement" />
    /// </summary>
    /// <typeparam name="TSource">Entity to map from</typeparam>
    public abstract class XElementMapper<TSource> : SimpleMapper<TSource, XElement>, IXmlMapper<TSource, XElement>
        where TSource : class, new()
    {
        private const string XsiNamespace = "http://www.w3.org/2001/XMLSchema-instance";

        /// <summary>
        /// Creates a new instance of the <see cref="XElementMapper{T}" /> class.
        /// </summary>
        /// <param name="nodeName">Node name to use</param>
        protected XElementMapper(string nodeName)
        {
            this.NodeName = nodeName;
            this.Namespace = string.Empty;
            this.XmlType = string.Empty;
        }

        /// <summary>
        /// Gets the namespace for the element.
        /// </summary>
        public string Namespace { get; protected set; }

        /// <summary>
        /// Gets the namespace for the element.
        /// </summary>
        public string NamespacePrefix { get; protected set; }

        /// <summary>
        /// Gets the node name for the element.
        /// </summary>
        public string NodeName { get; protected set; }

        /// <summary>
        /// Gets the namespace of the XML type.
        /// </summary>
        public string XmlTypeNamespace { get; private set; }

        /// <summary>
        /// Gets the namespace prefix of the XML type.
        /// </summary>
        public string XmlTypeNamespacePrefix { get; private set; }

        /// <summary>
        /// Gets the underlying XmlType, used to emit into an xsi:Type attribute
        /// </summary>
        public string XmlType { get; protected set; }

        /// <summary>
        /// Map from an entity to the XML node.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public override XElement Map(TSource source)
        {
            return Map(source, NodeName);
        }

        /// <summary>
        /// Maps the source to an XElement.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="nodeName"></param>
        /// <param name="xmlNamespace"></param>
        /// <param name="outputDefault"></param>
        /// <returns></returns>
        public XElement Map(TSource source, string nodeName, string xmlNamespace = null, bool outputDefault = false)
        {
            if (source == null && !outputDefault)
            {
                return null;
            }

            var destination = CreateElement(nodeName, xmlNamespace);

            // Needed for any possible default values in the entity
            if (source == null)
            {
                source = new TSource();
            }

            Map(source, destination);

            // Only emit the type if we have content
            if (destination.HasElements)
            {
                if (!string.IsNullOrEmpty(XmlType))
                {
                    if (!string.IsNullOrEmpty(NamespacePrefix))
                    {
                        // Make sure we have a namespace attribute for our XML types prefix
                        destination.Add(new XAttribute(XNamespace.Xmlns + XmlTypeNamespacePrefix, XmlTypeNamespace));
                    }

                    destination.Add(
                        new XAttribute(
                        QualifiedName("type", XsiNamespace),
                        QualifiedPrefixName(XmlType, XmlTypeNamespacePrefix)));
                }
            }

            return destination;
        }

        public List<XElement> MapList(TSource source, string collectionNode, bool outputDefault = false)
        {
            return MapList(source, collectionNode, NodeName, outputDefault);
        }

        public List<XElement> MapList(TSource source, string collectionNode, string nodeName, bool outputDefault = false)
        {
            var list = new List<XElement>();

            return list;
        }

        public List<XElement> MapList(TSource source, string collectionNode, string nodeName, string collectionNodeNamespacePrefix = "", string collectionItemNodeNamespacePrefix = "", bool outputDefault = false)
        {
            return new List<XElement>();
        }

        public XElement MapList(IEnumerable<TSource> source, string collectionNode, bool outputDefault = false)
        {
            return MapList(source, collectionNode, NodeName, outputDefault);
        }

        public virtual XElement MapList(IEnumerable<TSource> source, string collectionNode, string nodeName, bool outputDefault = false)
        {
            return MapList(source, collectionNode, nodeName, Namespace, Namespace, outputDefault);
        }

        public virtual XElement MapList(IEnumerable<TSource> source, string collectionNode, string collectionItemNodeName, string collectionNodeNamespace = "", string collectionItemNodeNamespace = "", bool outputDefault = false)
        {
            var values = source == null ? new List<TSource>() : source.ToList();
            if (values.Count == 0 && outputDefault == false)
            {
                return null;
            }

            return new XElement(QualifiedName(collectionNode, collectionNodeNamespace), ElementList(values, collectionItemNodeName, collectionItemNodeNamespace));
        }

        protected override XElement CreateDestination()
        {
            return new XElement(this.QualifiedName(NodeName, Namespace));
        }

        protected List<XElement> ElementList(IEnumerable<TSource> source, string nodeName, string nodeNamespace = "")
        {
            return source.Select(c => Map(c, nodeName, string.IsNullOrWhiteSpace(nodeNamespace) ? Namespace : nodeNamespace)).ToList();
        }

        protected XElement XElement(string name, bool value, string xmlNamespace = null, bool outputDefault = false)
        {
            return value.ToXElement(name, this.CanonicalNamespace(xmlNamespace), outputDefault);
        }

        protected XElement XElement(string name, string value, string xmlNamespace = null, bool outputDefault = false)
        {
            return value.ToXElement(name, this.CanonicalNamespace(xmlNamespace), outputDefault);
        }

        protected XElement XElement(string name, int value, string xmlNamespace = null, bool outputDefault = false)
        {
            return value.ToXElement(name, this.CanonicalNamespace(xmlNamespace), outputDefault);
        }

        protected XElement XElement(string name, decimal value, string xmlNamespace = null, bool outputDefault = false)
        {
            return value.ToXElement(name, this.CanonicalNamespace(xmlNamespace), outputDefault);
        }

        protected XElement XElement(string name, DateTime value, string xmlNamespace = null, bool outputDefault = false, string format = null)
        {
            if (value == DateTime.MinValue && outputDefault == false)
            {
                return null;
            }

            var formattedValue = string.IsNullOrEmpty(format) ? (object)value : value.ToString(format);
            return XElement(name, formattedValue, xmlNamespace, outputDefault);
        }

        protected XElement XElement(string name, Enum value, string xmlNamespace = null, bool outputDefault = false)
        {
            return value.ToXElement(name, this.CanonicalNamespace(xmlNamespace), outputDefault);
        }

        protected XElement XElement(string name, object value = null, string xmlNamespace = null, bool outputDefault = false)
        {
            return value.ToXElement(name, this.CanonicalNamespace(xmlNamespace), outputDefault);
        }

        /// <summary>
        /// Work out whether to deliver the supplied or default namespace
        /// </summary>
        /// <param name="xmlNamespace"></param>
        /// <returns></returns>
        private string CanonicalNamespace(string xmlNamespace)
        {
            return !string.IsNullOrEmpty(xmlNamespace) ? xmlNamespace : Namespace;
        }

        private XElement CreateElement(string nodeName, string xmlNamespace)
        {
            return new XElement(QualifiedName(nodeName, CanonicalNamespace(xmlNamespace)));
        }

        private XName QualifiedName(string name, string uri)
        {
            if (string.IsNullOrEmpty(uri))
            {
                return name;
            }

            XNamespace xns = uri;
            return xns + name;
        }

        private string QualifiedPrefixName(string name, string prefix)
        {
            if (string.IsNullOrEmpty(prefix))
            {
                return name;
            }

            return prefix + ":" + name;
        }
    }
}