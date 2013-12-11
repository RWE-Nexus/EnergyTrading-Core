namespace EnergyTrading.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;
    using System.Xml.XPath;

    using EnergyTrading.Logging;
    using EnergyTrading.Xml.Linq;

    /// <summary>
    /// Uses LINQ to XML to query an XDocument.
    /// </summary>
    public class LinqXPathProcessor : XDocumentXPathProcessor
    {
        private static readonly ILogger Logger = LoggerFactory.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Stack<Tuple<string, int>> pathStack;
        private readonly Stack<XElement> elementStack;
        private readonly List<XElement>[] nodesStack;
        private readonly CultureInfo culture;

        /// <summary>
        /// Creates a new instance of the <see cref="LinqXPathProcessor" /> class.
        /// </summary>
        public LinqXPathProcessor()
        {
            pathStack = new Stack<Tuple<string, int>>();
            elementStack = new Stack<XElement>();
            nodesStack = new List<XElement>[100];  // TODO: Allocation policy if size too small

            // Pre-allocate culture due to number of times used.
            culture = CultureInfo.InvariantCulture;
        }

        /// <summary>
        /// Gets or sets the current element we are on.
        /// </summary>
        public XElement CurrentElement { get; set; }

        /// <summary>
        /// Gets or sets the current node set we are on.
        /// </summary>
        public List<XElement> CurrentNodes { get; set; }

        /// <inheritdoc />
        public override bool IsNull(string xpath, string prefix = "")
        {
            var node = ToXElement(xpath, prefix);
            return node == null || node.IsEmpty;
        }

        /// <inheritdoc />
        public override int ToInt(string xpath, string prefix = "", bool isAttribute = false, int defaultValue = 0)
        {
            return isAttribute ? AttributeToValue(xpath, x => (int) x, prefix, defaultValue)
                               : ToValue(xpath, x => (int) x, prefix, defaultValue);
        }

        /// <inheritdoc />
        public override bool ToBool(string xpath, string prefix = "", bool isAttribute = false, bool defaultValue = false)
        {
            return isAttribute ? AttributeToValue(xpath, x => ((string) x).ParseXmlBool(), prefix, defaultValue)
                               : ToValue(xpath, x => ((string) x).ParseXmlBool(), prefix, defaultValue);
        }

        /// <inheritdoc />
        public override decimal ToDecimal(string xpath, string prefix = "", bool isAttribute = false, decimal defaultValue = 0)
        {
            return isAttribute
                       ? AttributeToValue(
                           xpath,
                           x =>
                               {
                                   Logger.CheckNumericFormat(x, typeof(decimal));
                                   return decimal.Parse(
                                       (string)x, NumberStyles.Number | NumberStyles.AllowExponent, culture);
                               },
                           prefix,
                           defaultValue)
                       : ToValue(
                           xpath,
                           x =>
                               {
                                   Logger.CheckNumericFormat(x, typeof(decimal));
                                   return decimal.Parse(
                                       (string)x, NumberStyles.Number | NumberStyles.AllowExponent, culture);
                               },
                           prefix,
                           defaultValue);
        }

        /// <inheritdoc />
        public override float ToFloat(string xpath, string prefix = "", bool isAttribute = false, float defaultValue = 0)
        {
            return isAttribute
                       ? AttributeToValue(
                           xpath,
                           x =>
                               {
                                   Logger.CheckNumericFormat(x, typeof(float));
                                   return float.Parse((string)x, culture);
                               },
                           prefix,
                           defaultValue)
                       : ToValue(
                           xpath,
                           x =>
                               {
                                   Logger.CheckNumericFormat(x, typeof(float));
                                   return float.Parse((string)x, culture);
                               },
                           prefix,
                           defaultValue);
        }

        /// <inheritdoc />
        public override double ToDouble(string xpath, string prefix = "", bool isAttribute = false, float defaultValue = 0)
        {
            return isAttribute
                       ? AttributeToValue(
                           xpath,
                           x =>
                               {
                                   Logger.CheckNumericFormat(x, typeof(double));
                                   return double.Parse((string)x, culture);
                               },
                           prefix,
                           defaultValue)
                       : ToValue(
                           xpath,
                           x =>
                               {
                                   Logger.CheckNumericFormat(x, typeof(double));
                                   return double.Parse((string)x, culture);
                               },
                           prefix,
                           defaultValue);
        }

        /// <inheritdoc />
        public override long ToLong(string xpath, string prefix = "", bool isAttribute = false, long defaultValue = 0)
        {
            return isAttribute ? AttributeToValue(xpath, x => (long) x, prefix, defaultValue)
                               : ToValue(xpath, x => (long) x, prefix, defaultValue);
        }

        /// <inheritdoc />
        public override string ToString(string xpath, string prefix = "", bool isAttribute = false, string defaultValue = null)
        {
            return isAttribute ? AttributeToValue(xpath, x => (string) x, prefix, defaultValue)
                               : ToValue(xpath, x => (string) x, prefix, defaultValue);
        }

        /// <inheritdoc />
        public override T ToEnum<T>(string xpath, string prefix = "", bool isAttribute = false)
        {
            return isAttribute ? AttributeToValue(xpath, x => (T)Enum.Parse(typeof(T), (string) x), prefix)
                               : ToValue(xpath, x => (T)Enum.Parse(typeof(T), (string) x), prefix);
        }

        /// <inheritdoc />
        public override DateTime ToDateTime(string xpath, string prefix = "", bool isAttribute = false)
        {
            return isAttribute ? AttributeToValue(xpath, x => (DateTime) x, prefix)
                               : ToValue(xpath, x => (DateTime) x, prefix);
        }

        /// <inheritdoc />
        public override DateTimeOffset ToDateTimeOffset(string xpath, string prefix = "", bool isAttribute = false)
        {
            return isAttribute ? AttributeToValue(xpath, x => (DateTimeOffset) x, prefix)
                               : ToValue(xpath, x => (DateTimeOffset) x, prefix);
        }

        /// <inheritdoc />
        public override bool CurrentNode()
        {
            return CurrentElement != null;
        }

        /// <inheritdoc />
        public override bool HasElement(string xpath, string prefix = "", int index = -1)
        {
            return ToXElement(xpath, prefix, index) != null;
        }

        /// <inheritdoc />
        public override bool HasAttribute(string xpath, string prefix = "")
        {
            return ToXAttribute(xpath, prefix) != null;
        }

        /// <inheritdoc />
        protected override string DetermineCurrentPath()
        {
            var path = string.Empty;
            foreach (var xp in pathStack.ToList())
            {
                var index = xp.Item2;
                var xn = XName.Get(xp.Item1);
                var pf = LookupPrefix(xn.NamespaceName);
                var frag = pf + ":" + xn.LocalName;
                if (index > 0)
                {
                    frag = frag + "[" + index + "]";
                }
                path = frag  + "/" + path;
            }

            return "/" + path;
        }

        /// <inheritdoc />
        protected override void InitializeManagers(XPathNavigator navigator)
        {
            if (NamespaceManager == null)
            {
                NamespaceManager = new NamespaceManager();
            }

            if (XPathManager == null)
            {
                XPathManager = new LinqXPathManager(NamespaceManager);
            }
        }

        /// <inheritdoc />
        public override void Push(string xpath, string xmlNamespace = null, string prefix = "", int index = -1)
        {
            PushNamespace(xmlNamespace, prefix);

            xpath = QualifyXPath(xpath, prefix, xmlNamespace, index);

            pathStack.Push(Tuple.Create(xpath, index));

            XElement q;
            List<XElement> nodes = null;
            if (elementStack.Count == 0)
            {
                q = (from x in RootElement.Elements(xpath) select x).FirstOrDefault();
            }
            else if (CurrentElement == null)
            {
                q = null;
            }
            else if (index == -1)
            {
                q = (from x in CurrentElement.Elements(xpath) select x).FirstOrDefault();
            }
            else
            {
                // Get the collection and work out which one we want.
                nodes = XElements(xpath);
                q = index > nodes.Count ? null : nodes[index - 1];
            }

            elementStack.Push(CurrentElement);

            CurrentElement = q;
            CurrentNodes = nodes;
        }

        /// <inheritdoc />
        public override void Pop()
        {
            PopNamespace();

            pathStack.Pop();
            CurrentElement = elementStack.Pop();
            CurrentNodes = nodesStack[elementStack.Count];

            // Wipe down children's node collection
            nodesStack[elementStack.Count + 1] = null;
        }

        /// <inheritdoc />
        protected override string QualifyXPath(string xpath, string prefix, string uri = null, int index = -1, bool isAttribute = false)
        {
            // Determine namespace if not provided.
            var ns = uri ?? CurrentNamespace;
            if (isAttribute && string.IsNullOrEmpty(prefix))
            {
                // Don't qualify the default namespace for attributes
                ns = string.Empty;
            }

            return XPathManager.QualifyXPath(xpath, prefix, ns, index, isAttribute);
        }

        private List<XElement> XElements(string xpath)
        {
            List<XElement> nodes;
            var ns = nodesStack[elementStack.Count];

            // Need to query
            if (ns == null)
            {
                nodes = (from x in CurrentElement.Elements(xpath) select x).ToList();

                // Locally cache nodes until the next pop^2, i.e. acquire on Jim[1] and don't 
                // remove until pop Jim's parent.
                nodesStack[elementStack.Count] = nodes;
            }
            else
            {
                nodes = ns;
            }

            return nodes;
        }

        private T AttributeToValue<T>(
            string xpath,
            Func<XAttribute, T> valueSelector,
            string prefix = "",
            T defaultValue = default(T))
        {
            var q = ToXAttribute(xpath, prefix);

            return q == null ? defaultValue : valueSelector(q);
        }

        private T ToValue<T>(
            string xpath,
            Func<XElement, T> valueSelector,
            string prefix = "",
            T defaultValue = default(T))
        {
            var q = ToXElement(xpath, prefix);
            return q == null || q.IsEmpty ? defaultValue : valueSelector(q);
        }

        private XAttribute ToXAttribute(string xpath, string prefix = "")
        {
            xpath = QualifyXPath(xpath, prefix, isAttribute: true);

            var target = CurrentElement != null
                             ? (from x in CurrentElement.Attributes(xpath) select x).FirstOrDefault()
                             : null;
            return target;
        }

        private XElement ToXElement(string xpath, string prefix = "", int index = -1)
        {
            if (string.IsNullOrEmpty(xpath) || CurrentElement == null)
            {
                return CurrentElement;
            }

            xpath = QualifyXPath(xpath, prefix);

            if (index == -1)
            {
                return CurrentElement.Elements(xpath).FirstOrDefault();
            }

            var nodes = CurrentElement.Elements(xpath).ToList();
            return nodes.Count >= index ? nodes[index - 1] : null;
        }
    }
}