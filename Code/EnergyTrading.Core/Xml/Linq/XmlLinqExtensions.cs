namespace EnergyTrading.Xml.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Schema;
    using System.Xml.XPath;

    using EnergyTrading.Logging;

    /// <summary>
    /// Extensions to System.Xml.Linq
    /// </summary>
    public static class XmlLinqExtensions
    {
        private static readonly DateTimeFormatInfo dtf = CultureInfo.InvariantCulture.DateTimeFormat;

        // Refer: http://blogs.msdn.com/b/ericwhite/archive/2009/01/28/equality-semantics-of-linq-to-xml-trees.aspx
        internal static class Xsi
        {
            internal static readonly XNamespace XsiNamespace = "http://www.w3.org/2001/XMLSchema-instance";
            internal static readonly XName SchemaLocation = XsiNamespace + "schemaLocation";
            internal static readonly XName NoNamespaceSchemaLocation = XsiNamespace + "noNamespaceSchemaLocation";
        }

        public static string ToStringAlignAttributes(this XDocument document)
        {
            var settings = new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true, NewLineOnAttributes = true };
            var stringBuilder = new StringBuilder();

            using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
            {
                document.WriteTo(xmlWriter);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Creates and returns a new, cloned, normalized XDocument.
        /// <para>
        /// If no schema is passed, the method will only perform normalizations that are possible without using PSVI.
        /// </para>
        /// <para>
        /// If there is a valid schema, then it will validate and normalize. During validation, it throws errors if XDocument is not valid against schema.
        /// </para>
        /// </summary>
        /// <param name="source">Source document to check</param>
        /// <param name="schema">(Optional) Schema to validate against</param>
        /// <returns></returns>
        public static XDocument Normalize(this XDocument source, XmlSchemaSet schema = null)
        {
            var havePsvi = false;

            // validate, throw errors, add PSVI information
            if (schema != null)
            {
                source.Validate(
                    schema,
                    (sender, e) =>
                    {
                        Console.WriteLine(e.Message);
                        throw e.Exception;
                    },
                    true);

                havePsvi = true;
            }

            return new XDocument(
                source.Declaration,
                source.Nodes().Select(
                    n =>
                    {
                        // Remove comments, processing instructions, and text nodes that are
                        // children of XDocument.  Only white space text nodes are allowed as
                        // children of a document, so we can remove all text nodes.
                        if (n is XComment || n is XProcessingInstruction || n is XText)
                        {
                            return null;
                        }

                        var e = n as XElement;

                        return e != null ? NormalizeElement(e, havePsvi) : n;
                    }));
        }

        /// <summary>
        /// This method compares two XDocument objects after normalization.
        /// It is valid to pass null for the schema parameter, in which case the method will only do the normalizations that are possible without using PSVI.
        /// </summary>
        /// <param name="doc1"></param>
        /// <param name="doc2"></param>
        /// <param name="schemaSet"></param>
        /// <returns></returns>
        public static bool DeepEqualsWithNormalization(this XDocument doc1, XDocument doc2, XmlSchemaSet schemaSet)
        {
            var d1 = Normalize(doc1, schemaSet);
            var d2 = Normalize(doc2, schemaSet);
            return XNode.DeepEquals(d1, d2);
        }

        /// <summary>
        /// Get namespaces.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static IEnumerable<Tuple<string, string>> Namespaces(this XPathDocument document)
        {
            var navigator = document.CreateNavigator();
            var iter = navigator.Select("//namespace::*");
            IDictionary<string, string> d = new Dictionary<string, string>();
            while (iter.MoveNext())
            {
                d[iter.Current.Name] = iter.Current.Value;
            }

            return d.Select(x => new Tuple<string, string>(x.Key, x.Value));
        }

        /// <summary>
        /// Get namespaces.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static IEnumerable<Tuple<string, string>> Namespaces2(this XPathDocument document)
        {
            var navigator = document.CreateNavigator();
            var iter = navigator.Select("//namespace::*[not(. = ../../namespace::*)]");
            while (iter.MoveNext())
            {
                yield return new Tuple<string, string>(iter.Current.Name, iter.Current.Value);
            }
        }

        /// <summary>
        /// Get namespaces.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static IEnumerable<Tuple<string, string>> Namespaces(this XDocument document)
        {
            return document.Root.Namespaces();
        }

        /// <summary>
        /// Get namespaces.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static IEnumerable<Tuple<string, string>> Namespaces(this XmlDocument document)
        {
            var manager = new XmlNamespaceManager(document.NameTable);
            var ns = manager.GetNamespacesInScope(XmlNamespaceScope.All);

            return ns.Select(x => new Tuple<string, string>(x.Key, x.Value));
        }

        /// <summary>
        /// Check whether the formatting of a XAttribute is compatible with a numeric value.
        /// </summary>
        /// <param name="logger">Logger to log issues against.</param>
        /// <param name="attribute">Attribute to check</param>
        /// <param name="numericType">Type to check against.</param>
        public static void CheckNumericFormat(this ILogger logger, XAttribute attribute, Type numericType)
        {
            if (attribute == null)
            {
                return;
            }

            logger.CheckNumericFormat(attribute.Name.ToString(), (string)attribute, numericType);
        }

        /// <summary>
        /// Check whether the formatting of a XElement is compatible with a numeric value.
        /// </summary>
        /// <param name="logger">Logger to log issues against.</param>
        /// <param name="element">Element to check</param>
        /// <param name="numericType">Type to check against.</param>
        public static void CheckNumericFormat(this ILogger logger, XElement element, Type numericType)
        {
            if (element == null)
            {
                return;
            }

            logger.CheckNumericFormat(element.Name.ToString(), (string)element, numericType);
        }

        /// <summary>
        /// Check whether the formatting of a node iterator is compatible with a numeric value.
        /// </summary>
        /// <param name="logger">Logger to log issues against.</param>
        /// <param name="nodeIterator">Value to check</param>
        /// <param name="numericType">Type to check against.</param>
        public static void CheckNumericFormat(this ILogger logger, XPathNodeIterator nodeIterator, Type numericType)
        {
            if (nodeIterator.Current == null)
            {
                return;
            }

            logger.CheckNumericFormat(nodeIterator.Current.Name, nodeIterator.Current.Value, numericType);
        }

        private static void CheckNumericFormat(this ILogger logger, string name, string value, Type numericType)
        {
            if (value == null)
            {
                return;
            }

            if (value.Contains(","))
            {
                logger.Warn(name + " numeric value " + value + " contains a comma");
            }
            if (numericType == typeof(decimal) && value.ToUpperInvariant().Contains("E"))
            {
                logger.Warn(name + " numeric value " + value + " contains an exponent value");
            }
        }

        /// <summary>
        /// Registers the namespace against the namespace manager if it doesn't exist
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="xmlNamespace"></param>
        /// <param name="xmlPrefix"></param>
        /// <returns>Empty string if no namespace, existing prefix if already defined, namespacePrefix otherwise</returns>
        public static void RegisterNamespace(this XmlNamespaceManager manager, string xmlPrefix, string xmlNamespace)
        {
            if (string.IsNullOrEmpty(xmlNamespace))
            {
                return;
            }

            var pf2 = manager.LookupNamespace(xmlNamespace);
            if (string.IsNullOrEmpty(pf2))
            {
                manager.AddNamespace(xmlPrefix, xmlNamespace);
            }
        }

        private static IEnumerable<XAttribute> NormalizeAttributes(XElement element, bool havePsvi)
        {
            return element.Attributes()
                    .Where(a => !a.IsNamespaceDeclaration &&
                        a.Name != Xsi.SchemaLocation &&
                        a.Name != Xsi.NoNamespaceSchemaLocation)
                    .OrderBy(a => a.Name.NamespaceName)
                    .ThenBy(a => a.Name.LocalName)
                    .Select(
                        a =>
                        {
                            var dt = a.GetSchemaInfo();
                            if (havePsvi && dt != null && dt.SchemaType != null)
                            {
                                switch (dt.SchemaType.TypeCode)
                                {
                                    case XmlTypeCode.Boolean:
                                        return new XAttribute(a.Name, (bool)a);
                                    case XmlTypeCode.DateTime:
                                        return new XAttribute(a.Name, (DateTime)a);
                                    case XmlTypeCode.Decimal:
                                        return new XAttribute(a.Name, (decimal)a);
                                    case XmlTypeCode.Double:
                                        return new XAttribute(a.Name, (double)a);
                                    case XmlTypeCode.Float:
                                        return new XAttribute(a.Name, (float)a);
                                    case XmlTypeCode.HexBinary:
                                    case XmlTypeCode.Language:
                                        return new XAttribute(a.Name, ((string)a).ToLower());
                                }
                            }
                            return a;
                        });
        }

        private static XElement NormalizeElement(XElement element, bool havePsvi)
        {
            var dt = element.GetSchemaInfo();

            if (havePsvi && dt != null && dt.SchemaType != null)
            {
                switch (dt.SchemaType.TypeCode)
                {
                    case XmlTypeCode.Boolean:
                        return new XElement(element.Name, NormalizeAttributes(element, havePsvi), (bool)element);
                    case XmlTypeCode.DateTime:
                        return new XElement(element.Name, NormalizeAttributes(element, havePsvi), (DateTime)element);
                    case XmlTypeCode.Decimal:
                        return new XElement(element.Name, NormalizeAttributes(element, havePsvi), (decimal)element);
                    case XmlTypeCode.Double:
                        return new XElement(element.Name, NormalizeAttributes(element, havePsvi), (double)element);
                    case XmlTypeCode.Float:
                        return new XElement(element.Name, NormalizeAttributes(element, havePsvi), (float)element);
                    case XmlTypeCode.HexBinary:
                    case XmlTypeCode.Language:
                        return new XElement(element.Name, NormalizeAttributes(element, havePsvi), ((string)element).ToLower());
                    default:
                        return new XElement(element.Name, NormalizeAttributes(element, havePsvi), element.Nodes().Select(n => NormalizeNode(n, havePsvi)));
                }
            }

            return new XElement(element.Name, NormalizeAttributes(element, havePsvi), element.Nodes().Select(n => NormalizeNode(n, havePsvi)));
        }

        private static XNode NormalizeNode(XNode node, bool havePsvi)
        {
            // trim comments and processing instructions from normalized tree
            if (node is XComment || node is XProcessingInstruction)
            {
                return null;
            }

            // Only thing left is XCData and XText, so clone them
            var e = node as XElement;
            return e != null ? NormalizeElement(e, havePsvi) : node;
        }

        /// <summary>
        /// Parse a potential boolean value from an XML string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ParseXmlBool(this string value)
        {
            switch (value)
            {
                case "0":
                case "false":
                case "False":
                case "No":
                case "no":
                    return false;

                case "1":
                case "true":
                case "True":
                case "Yes":
                case "yes":
                    return true;

                default:
                    return bool.Parse(value);
            }
        }

        /// <summary>
        /// Get namespaces from an element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static IEnumerable<Tuple<string, string>> Namespaces(this XElement element)
        {
            if (element == null)
            {
                return null;
            }

            var d = element.DescendantsAndSelf().Attributes().
                Where(a => a.IsNamespaceDeclaration).
                GroupBy(a => a.Name.Namespace == XNamespace.None ? string.Empty : a.Name.LocalName,
                    a => XNamespace.Get(a.Value)).
                ToDictionary(g => g.Key,
                    g => g.First());

            return d.Select(x => new Tuple<string, string>(x.Key, x.Value.NamespaceName));
        }

        /// <summary>
        /// Construct a new XAttribute for a value.
        /// </summary>
        /// <param name="name">Name of the XAttribute</param>
        /// <param name="value">Value of the XAttribute</param>
        /// <param name="xmlNamespace">XML namespace to use.</param>
        /// <param name="outputDefault">Whether to output the XAttribute if the value is the default value of the type.</param>
        /// <param name="defaultValue">Default value to compare to.</param>        
        /// <returns>A new XAttribute or null depending on value/outputDefault.</returns>
        public static XAttribute ToXAttribute(this bool value, string name, string xmlNamespace = null, bool outputDefault = false, bool defaultValue = false)
        {
            if (value == defaultValue && outputDefault == false)
            {
                return null;
            }

            object x = value;
            return x.ToXAttribute(name, xmlNamespace, outputDefault);
        }

        /// <summary>
        /// Construct a new XAttribute for a value.
        /// </summary>
        /// <param name="name">Name of the XAttribute</param>
        /// <param name="value">Value of the XAttribute</param>
        /// <param name="xmlNamespace">XML namespace to use.</param>
        /// <param name="outputDefault">Whether to output the XAttribute if the value is the default value of the type.</param>
        /// <param name="defaultValue">Default value to compare to.</param>        
        /// <returns>A new XAttribute or null depending on value/outputDefault.</returns>
        public static XAttribute ToXAttribute(this string value, string name, string xmlNamespace = null, bool outputDefault = false, string defaultValue = null)
        {
            if (value == defaultValue && outputDefault == false)
            {
                return null;
            }

            object x = value;
            return x.ToXAttribute(name, xmlNamespace, outputDefault);
        }

        /// <summary>
        /// Construct a new XAttribute for a value.
        /// </summary>
        /// <param name="name">Name of the XAttribute</param>
        /// <param name="value">Value of the XAttribute</param>
        /// <param name="xmlNamespace">XML namespace to use.</param>
        /// <param name="outputDefault">Whether to output the XAttribute if the value is the default value of the type.</param>
        /// <param name="defaultValue">Default value to compare to.</param>        
        /// <returns>A new XAttribute or null depending on value/outputDefault.</returns>
        public static XAttribute ToXAttribute(this int value, string name, string xmlNamespace = null, bool outputDefault = false, int defaultValue = 0)
        {
            if (value == defaultValue && outputDefault == false)
            {
                return null;
            }

            object x = value;
            return x.ToXAttribute(name, xmlNamespace, outputDefault);
        }

        /// <summary>
        /// Construct a new XAttribute for a value.
        /// </summary>
        /// <param name="name">Name of the XAttribute</param>
        /// <param name="value">Value of the XAttribute</param>
        /// <param name="xmlNamespace">XML namespace to use.</param>
        /// <param name="outputDefault">Whether to output the XAttribute if the value is the default value of the type.</param>
        /// <param name="defaultValue">Default value to compare to.</param>        
        /// <returns>A new XAttribute or null depending on value/outputDefault.</returns>
        public static XAttribute ToXAttribute(this decimal value, string name, string xmlNamespace = null, bool outputDefault = false, decimal defaultValue = 0)
        {
            if (value == defaultValue && outputDefault == false)
            {
                return null;
            }

            object x = value;
            return x.ToXAttribute(name, xmlNamespace, outputDefault);
        }

        /// <summary>
        /// Construct a new XAttribute for a value.
        /// </summary>
        /// <param name="name">Name of the XAttribute</param>
        /// <param name="value">Value of the XAttribute</param>
        /// <param name="xmlNamespace">XML namespace to use.</param>
        /// <param name="outputDefault">Whether to output the XAttribute if the value is the default value of the type.</param>
        /// <param name="format">Format to use, defaults to a XML date time format with UTC time zone.</param>
        /// <returns>A new XAttribute or null depending on value/outputDefault.</returns>
        public static XAttribute ToXAttribute(this DateTime value, string name, string xmlNamespace = null, bool outputDefault = false, string format = XmlExtensions.UtcDateTimeSecondFormat)
        {
            if (value == DateTime.MinValue && outputDefault == false)
            {
                return null;
            }

            var x = string.IsNullOrEmpty(format) ? (object)value : value.ToString(format, dtf);
            return x.ToXAttribute(name, xmlNamespace, outputDefault);
        }

        /// <summary>
        /// Construct a new XAttribute for a value.
        /// </summary>
        /// <param name="name">Name of the XAttribute</param>
        /// <param name="value">Value of the XAttribute</param>
        /// <param name="xmlNamespace">XML namespace to use.</param>
        /// <param name="outputDefault">Whether to output the XAttribute if the value is the default value of the type.</param>
        /// <param name="format">Format to use, if null uses the invariant culture DateTimeFormatInfo</param>         
        /// <returns>A new XAttribute or null depending on value/outputDefault.</returns>
        public static XAttribute ToXAttribute(this DateTimeOffset value, string name, string xmlNamespace = null, bool outputDefault = false, string format = null)
        {
            if (value == DateTime.MinValue && outputDefault == false)
            {
                return null;
            }

            var x = string.IsNullOrEmpty(format) ? (object)value : value.ToString(format, dtf);
            return x.ToXAttribute(name, xmlNamespace, outputDefault);
        }

        /// <summary>
        /// Construct a new XAttribute for a value.
        /// </summary>
        /// <param name="name">Name of the XAttribute</param>
        /// <param name="value">Value of the XAttribute</param>
        /// <param name="xmlNamespace">XML namespace to use.</param>
        /// <param name="outputDefault">Whether to output the XAttribute if the value is the default value of the type.</param>
        /// <returns>A new XAttribute or null depending on value/outputDefault.</returns>
        public static XAttribute ToXAttribute(this Enum value, string name, string xmlNamespace = null, bool outputDefault = false)
        {
            // Enum value "Unknown" is the default value by convention. Note that it does not exist in the Schema.
            if (Enum.GetName(value.GetType(), value) == "Unknown" && outputDefault == false)
            {
                return null;
            }

            object x = value;
            return x.ToXAttribute(name, xmlNamespace, outputDefault);
        }

        /// <summary>
        /// Construct a new XAttribute for a value.
        /// </summary>
        /// <param name="name">Name of the XAttribute</param>
        /// <param name="value">Value of the XAttribute</param>
        /// <param name="xmlNamespace">XML namespace to use.</param>
        /// <param name="outputDefault">Whether to output the XAttribute if the value is the default value of the type.</param>
        /// <param name="defaultValue">Default value to compare to.</param>        
        /// <returns>A new XAttribute or null depending on value/outputDefault.</returns>
        public static XAttribute ToXAttribute(this object value, string name, string xmlNamespace = null, bool outputDefault = false, object defaultValue = null)
        {
            if (value == defaultValue && outputDefault == false)
            {
                return null;
            }

            return new XAttribute(name.QualifiedName(xmlNamespace), value);
        }

        /// <summary>
        /// Construct a new XElement for a value.
        /// </summary>
        /// <param name="name">Name of the XElement</param>
        /// <param name="value">Value of the XElement</param>
        /// <param name="xmlNamespace">XML namespace to use.</param>
        /// <param name="outputDefault">Whether to output the XElement if the value is the default value of the type.</param>
        /// <param name="defaultValue">Default value to compare to.</param>
        /// <returns>A new XElement or null depending on value/outputDefault.</returns>
        public static XElement ToXElement(this bool value, string name, string xmlNamespace = null, bool outputDefault = false, bool defaultValue = false)
        {
            if (value == defaultValue && outputDefault == false)
            {
                return null;
            }

            object x = value;
            return x.ToXElement(name, xmlNamespace, outputDefault);
        }

        /// <summary>
        /// Construct a new XElement for a value.
        /// </summary>
        /// <param name="name">Name of the XElement</param>
        /// <param name="value">Value of the XElement</param>
        /// <param name="xmlNamespace">XML namespace to use.</param>
        /// <param name="outputDefault">Whether to output the XElement if the value is the default value of the type.</param>
        /// <param name="defaultValue">Default value to compare to.</param>        
        /// <returns>A new XElement or null depending on value/outputDefault.</returns>
        public static XElement ToXElement(this string value, string name, string xmlNamespace = null, bool outputDefault = false, string defaultValue = null)
        {
            if (value == defaultValue && outputDefault == false)
            {
                return null;
            }

            object x = value;
            return x.ToXElement(name, xmlNamespace, outputDefault);
        }

        /// <summary>
        /// Construct a new XElement for a value.
        /// </summary>
        /// <param name="name">Name of the XElement</param>
        /// <param name="value">Value of the XElement</param>
        /// <param name="xmlNamespace">XML namespace to use.</param>
        /// <param name="outputDefault">Whether to output the XElement if the value is the default value of the type.</param>
        /// <param name="defaultValue">Default value to compare to.</param>
        /// <returns>A new XElement or null depending on value/outputDefault.</returns>
        public static XElement ToXElement(this int value, string name, string xmlNamespace = null, bool outputDefault = false, int defaultValue = 0)
        {
            if (value == defaultValue && outputDefault == false)
            {
                return null;
            }

            object x = value;
            return x.ToXElement(name, xmlNamespace, outputDefault);
        }

        /// <summary>
        /// Construct a new XElement for a value.
        /// </summary>
        /// <param name="name">Name of the XElement</param>
        /// <param name="value">Value of the XElement</param>
        /// <param name="xmlNamespace">XML namespace to use.</param>
        /// <param name="outputDefault">Whether to output the XElement if the value is the default value of the type.</param>
        /// <param name="defaultValue">Default value to compare to.</param>       
        /// <returns>A new XElement or null depending on value/outputDefault.</returns>
        public static XElement ToXElement(this decimal value, string name, string xmlNamespace = null, bool outputDefault = false, decimal defaultValue = 0)
        {
            if (value == defaultValue && outputDefault == false)
            {
                return null;
            }

            object x = value;
            return x.ToXElement(name, xmlNamespace, outputDefault);
        }

        /// <summary>
        /// Construct a new XElement for a value.
        /// </summary>
        /// <param name="name">Name of the XElement</param>
        /// <param name="value">Value of the XElement</param>
        /// <param name="xmlNamespace">XML namespace to use.</param>
        /// <param name="outputDefault">Whether to output the XElement if the value is the default value of the type.</param>
        /// <param name="format">Format to use, defaults to a XML date time format with UTC time zone.</param>
        /// <returns>A new XElement or null depending on value/outputDefault.</returns>
        public static XElement ToXElement(this DateTime value, string name, string xmlNamespace = null, bool outputDefault = false, string format = XmlExtensions.UtcDateTimeSecondFormat)
        {
            if (value == DateTime.MinValue && outputDefault == false)
            {
                return null;
            }

            var x = string.IsNullOrEmpty(format) ? (object)value : value.ToString(format, dtf);
            return x.ToXElement(name, xmlNamespace, outputDefault);
        }

        /// <summary>
        /// Construct a new XElement for a value.
        /// </summary>
        /// <param name="name">Name of the XElement</param>
        /// <param name="value">Value of the XElement</param>
        /// <param name="xmlNamespace">XML namespace to use.</param>
        /// <param name="outputDefault">Whether to output the XElement if the value is the default value of the type.</param>
        /// <param name="format">Format to use, if null uses the invariant culture DateTimeFormatInfo</param>
        /// <returns>A new XElement or null depending on value/outputDefault.</returns>
        public static XElement ToXElement(this DateTimeOffset value, string name, string xmlNamespace = null, bool outputDefault = false, string format = null)
        {
            if (value == DateTimeOffset.MinValue && outputDefault == false)
            {
                return null;
            }

            var x = string.IsNullOrEmpty(format) ? (object)value : value.ToString(format, dtf);
            return x.ToXElement(name, xmlNamespace, outputDefault);
        }

        /// <summary>
        /// Construct a new XElement for a value.
        /// </summary>
        /// <param name="name">Name of the XElement</param>
        /// <param name="value">Value of the XElement</param>
        /// <param name="xmlNamespace">XML namespace to use.</param>
        /// <param name="outputDefault">Whether to output the XElement if the value is the default value of the type.</param>
        /// <returns>A new XElement or null depending on value/outputDefault.</returns>
        public static XElement ToXElement(this Enum value, string name, string xmlNamespace = null, bool outputDefault = false)
        {
            // Enum value "Unknown" is the default value by convention. Note that it does not exist in the Schema.
            if (Enum.GetName(value.GetType(), value) == "Unknown" && outputDefault == false)
            {
                return null;
            }

            object x = value;
            return x.ToXElement(name, xmlNamespace, outputDefault);
        }

        /// <summary>
        /// Construct a new XElement for a value.
        /// </summary>
        /// <param name="name">Name of the XElement</param>
        /// <param name="value">Value of the XElement</param>
        /// <param name="xmlNamespace">XML namespace to use.</param>
        /// <param name="outputDefault">Whether to output the XElement if the value is the default value of the type.</param>
        /// <param name="defaultValue">Default value to compare to.</param>
        /// <returns>A new XElement or null depending on value/outputDefault.</returns>
        public static XElement ToXElement(this object value, string name, string xmlNamespace = null, bool outputDefault = false, object defaultValue = null)
        {
            if (value == defaultValue && outputDefault == false)
            {
                return null;
            }

            return new XElement(name.QualifiedName(xmlNamespace), value);
        }

        /// <summary>
        /// Qualify a name with an XML namespace.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="xmlNamespace"></param>
        /// <returns></returns>
        public static XName QualifiedName(this string name, string xmlNamespace = null)
        {
            if (string.IsNullOrEmpty(xmlNamespace))
            {
                return name;
            }

            XNamespace xns = xmlNamespace;
            return xns + name;
        }

        public static string GetChildElementValue(this XElement source, XName childName)
        {
            if (source == null)
            {
                return null;
            }

            var childElement = source.Elements(childName).FirstOrDefault();
            return childElement != null ? childElement.Value : null;
        }
    }
}