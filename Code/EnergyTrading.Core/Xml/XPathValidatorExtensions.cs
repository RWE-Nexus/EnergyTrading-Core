namespace EnergyTrading.Xml
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Linq;

    /// <summary>
    /// Extension methods for <see cref="IXPathValidator" />.
    /// </summary>
    public static class XPathValidatorExtensions
    {
        public static IList<string> Validate(this IXPathValidator validator, string xml)
        {
            var doc = Load(xml);

            return validator.Validate(doc);
        }

        /// <summary>
        /// Converts XPaths into validation items.
        /// </summary>
        /// <param name="xpaths"></param>
        /// <returns></returns>
        public static XPathValidator ToXPathValidator(this IEnumerable<string> xpaths)
        {
            // Make the parent the root node, should always exist
            var parent = new XPathValidator { XPath = "/" };

            foreach (var xpath in xpaths)
            {
                parent.AddChild(xpath);
            }

            return parent;
        }

        /// <summary>
        /// Add a child validation item.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="xpath"></param>
        /// <param name="children"></param>
        /// <param name="relative">Is the xpath relative to the child xpath</param>
        /// <returns></returns>
        public static XPathValidator AddChild(this XPathValidator item, string xpath, IList<IXPathValidator> children = null, bool relative = false)
        {
            if (relative)
            {
                xpath = item.XPath + "/" + xpath;
            }
            var child = new XPathValidator { XPath = xpath, Children = children };
            return item.AddChild(child);
        }

        /// <summary>
        /// Add a child validation item.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="child"></param>
        /// <returns></returns>
        public static XPathValidator AddChild(this XPathValidator item, XPathValidator child)
        {
            item.Children.Add(child);

            return item;
        }

        private static XDocument Load(string xml)
        {
            // TODO: Use Settings here
            return XDocument.Load(new StringReader(xml));
        }
    }
}
