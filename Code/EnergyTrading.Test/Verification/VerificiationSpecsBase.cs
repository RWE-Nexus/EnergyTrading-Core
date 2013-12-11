namespace EnergyTrading.Test.Verification
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;

    using EnergyTrading.Test.Mapping;

    public abstract class VerificiationSpecsBase
    {
        protected bool AreDifferentXml(string expected, string candidate, out string differences)
        {
            differences = expected.CheckXml(candidate);
            return !string.IsNullOrEmpty(differences);
        }

        protected string RemoveAllNonOriginatingSystemIds(string source)
        {
            return source.RemoveAllNonOriginatingSystemIds();
        }

        protected string RemoveIdEntriesForSystem(string system, string source)
        {
            return source.RemoveIdEntriesForSystem(system);
        }

        protected string RemoveIdEntriesForSystems(IEnumerable<string> systems, string source)
        {
            return source.RemoveIdEntriesForSystems(systems);
        }

        protected string RemoveElementValue(string element, string source)
        {
            return source.RemoveElementValue(element);
        }

        protected string RemoveElementValues(IEnumerable<string> elementNames, string source)
        {
            return source.RemoveElementValues(elementNames);
        }

        protected bool HasIdForSystem(string system, string source)
        {
            return source.HasIdEntriesForSystem(system);
        }

        protected string LoadEmbeddedResource(string resourceName)
        {
            var stream = Assembly.GetCallingAssembly().GetManifestResourceStream(resourceName);
            return stream == null ? null : new StreamReader(stream).ReadToEnd();
        }

        protected byte[] LoadEmbeddedResourceInBytes(string resourceName)
        {
            var stream = Assembly.GetCallingAssembly().GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                return null;
            }
            var byteArray = new byte[stream.Length];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(byteArray, 0, Convert.ToInt32(stream.Length));
            return byteArray;
        }

        protected abstract string RemoveDynamicValues(string source);
    }
}