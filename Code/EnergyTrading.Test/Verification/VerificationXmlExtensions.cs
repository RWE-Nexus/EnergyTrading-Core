namespace EnergyTrading.Test.Verification
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public static class VerificationXmlExtensions
    {
        private static string GetSystemIdRegex(string systemNameRegex, string originatingSourceRegex = "([^<]*)")
        {
            return "<[common:]*SystemId([^>]*)>([^<]*)<[common:]*SystemID>" + systemNameRegex + "</[common:]*SystemID>([^<]*)<[common:]*Identifier>([^<]*)</[common:]*Identifier>([^<]*)<[common:]*OriginatingSourceIND>" + originatingSourceRegex + "</[common:]*OriginatingSourceIND>([^<]*)</[common:]*SystemId>";
        }

        public static string RemoveAllNonOriginatingSystemIds(this string source)
        {
            return Regex.Replace(source, GetSystemIdRegex("([^<]*)", "[Ff][Aa][Ll][Ss][Ee]"), string.Empty);
        }

        public static string RemoveIdEntriesForSystem(this string source, string system)
        {
            return Regex.Replace(source, GetSystemIdRegex(system), string.Empty);
        }

        public static string RemoveIdEntriesForSystems(this string source, IEnumerable<string> systems)
        {
            return systems.Aggregate(source, (current, system) => current.RemoveIdEntriesForSystem(system));
        }

        public static bool HasIdEntriesForSystem(this string source, string system)
        {
            return Regex.IsMatch(source, GetSystemIdRegex(system));
        }

        public static string RemoveElementValue(this string source, string elementName)
        {
            var elementRegex = "<" + elementName + ">([^<]*)</" + elementName + ">";
            var emptyElement = "<" + elementName + "></" + elementName + ">";
            return Regex.Replace(source, elementRegex, emptyElement);
        }

        public static string RemoveElementValues(this string source, IEnumerable<string> elementNames)
        {
            return elementNames.Aggregate(source, (current, elementName) => current.RemoveElementValue(elementName));
        }
    }
}