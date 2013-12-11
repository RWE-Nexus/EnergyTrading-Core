namespace EnergyTrading.Mapping
{
    using System;
    using System.Data;
    using System.Linq;
    using System.Xml;
    using System.Xml.XPath;

    public static class XPathExtensions
    {
        /// <summary>
        /// Get the value of the node.
        /// </summary>
        /// <typeparam name="T">Type of value to return</typeparam>
        /// <param name="navigator">Navigator to use</param>
        /// <param name="manager">Namespace manager to use</param>
        /// <param name="xpath">Relative Xpath to use</param>
        /// <param name="valueSelector">Value selector to use</param>
        /// <param name="defaultValue">Default value to use</param>
        /// <returns></returns>
        public static T ToValue<T>(this XPathNavigator navigator, XmlNamespaceManager manager,
            string xpath,
            Func<XPathNodeIterator, T> valueSelector,
            T defaultValue = default(T))
        {
            var node = navigator.Select(xpath, manager);
            var exists = node.MoveNext();
            if (exists)
            {
                return node.Current == null || node.Current.IsEmptyElement
                    ? defaultValue
                    : valueSelector(node);
            }

            return defaultValue;
        }

        public static string NsPrefix(this XmlNamespaceManager manager, string currentNamespace, string currentPath)
        {
            var prefix = string.Empty;

            // Apply the namespace to the node if a namespace is defined and we don't have one yet
            if (!string.IsNullOrEmpty(currentNamespace))
            {
                prefix = manager.LookupPrefix(currentNamespace);
                if (string.IsNullOrEmpty(prefix))
                {
                    throw new ObjectNotFoundException(
                        string.Concat(
                            "No prefix for namespace: " + currentNamespace,
                            Environment.NewLine,
                            "Current path: " + currentPath,
                            Environment.NewLine,
                            "Registered namespaces: " + manager.GetRegisteredNamespaces()));
                }
            }

            return prefix;
        }

        public static string GetRegisteredNamespaces(this XmlNamespaceManager manager)
        {
            var namespacesInScope = manager.GetNamespacesInScope(XmlNamespaceScope.Local);
            if (namespacesInScope == null)
            {
                return null;
            }

            var registeredNamespaces = namespacesInScope
                .Select(x => string.Format("[{0}: '{1}']", x.Key, x.Value)).ToArray();
            return string.Join(", ", registeredNamespaces);
        }
    }
}