namespace EnergyTrading.Xml
{
    using System;

    /// <summary>
    /// Extensions for XML.
    /// </summary>
    public static class XmlExtensions
    {
        /// <summary>
        /// Standard XML date format.
        /// </summary>
        public const string DateFormat = "yyyy-MM-dd";

        /// <summary>
        /// Standard XML time format to second precision.
        /// </summary>
        public const string TimeFormat = "HH:mm:ss";

        /// <summary>
        /// Standard XML time format to millisecond precision.
        /// </summary>
        public const string TimeFormatMillisecond = "HH:mm:ss.fff";

        /// <summary>
        /// Standard XML UTC date time format to millisecond precision.
        /// </summary>
        public const string UtcDateTimeMillisecondFormat = DateFormat + "'T'" + TimeFormatMillisecond + "'Z'";

        /// <summary>
        /// Standard XML UTC date time format to second precision.
        /// </summary>
        public const string UtcDateTimeSecondFormat = DateFormat + "'T'" + TimeFormat + "'Z'";

        /// <summary>
        /// Determine whether we should include milliseconds in the format.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string UtcDateTimeFormat(this DateTime value)
        {
            return value.Millisecond != 0 ? UtcDateTimeMillisecondFormat : UtcDateTimeSecondFormat;
        }

        /// <summary>
        /// Qualify elements of an XPath with a prefix.
        /// </summary>
        /// <param name="xpath">XPath to use</param>
        /// <param name="prefix">Prefix to use</param>
        /// <param name="qualifyAttributes">Whether to qualify attributes</param>
        /// <returns>Returns the qualified string</returns>
        public static string QualifyXPath(this string xpath, string prefix, bool qualifyAttributes = false)
        {
            if (string.IsNullOrEmpty(prefix))
            {
                return xpath;
            }

            var sp = xpath.Split('/');
            var nsxpath = string.Empty;
            foreach (var item in sp)
            {
                if (string.IsNullOrEmpty(item))
                {
                    nsxpath += "/";
                }
                else if (item.Contains(":"))
                {
                    nsxpath += item + "/";
                }
                else if (item.StartsWith("@"))
                {
                    nsxpath += qualifyAttributes ? "@" + prefix + ":" + item.Substring(1) : item;
                }
                else
                {
                    nsxpath += prefix + ":" + item + "/";
                }
            }

            while (nsxpath.EndsWith("/"))
            {
                nsxpath = nsxpath.Substring(0, nsxpath.Length - 1);
            }

            // Axis fixup
            nsxpath = nsxpath.Replace("preceding-sibling::", "preceding-sibling::" + prefix + ":");

            if (string.IsNullOrEmpty(nsxpath))
            {
                nsxpath = "/";
            }

            return nsxpath;
        }        
    }
}
