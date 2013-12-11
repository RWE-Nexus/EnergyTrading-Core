namespace EnergyTrading.ServiceModel.Channels
{
    using System;
    using System.IO;
    using System.Net.Mime;
    using System.ServiceModel.Channels;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Xml;

    public static class ChannelsExtensions
    {
        /// <summary>
        /// Write a <see cref="Message" /> to a stream
        /// </summary>
        /// <param name="message"></param>
        /// <param name="writerSettings"></param>
        /// <param name="stream"></param>
        public static Stream XmlToStream(this Message message, XmlWriterSettings writerSettings, Stream stream = null)
        {
            if (stream == null)
            {
                stream = new MemoryStream();
            }

            using (var writer = XmlWriter.Create(stream, writerSettings))
            {
                message.WriteMessage(writer);
            }

            return stream;
        }

        /// <summary>
        /// Get the encoding from the a binary XML message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static Encoding GetXmlDeclEncoding(this byte[] message, Encoding encoding)
        {
            const string Pattern = "<\\?xml\\sversion=\"1.0\"\\sencoding=\"(?<encoding>[\\w|-]+)\"";
            var content = encoding.GetString(message, 0, 100);

            var m = Regex.Match(content, Pattern, RegexOptions.ExplicitCapture);
            return m.Groups["encoding"].Success ? Encoding.GetEncoding(m.Groups["encoding"].Value) : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static Encoding GetEncoding(this byte[] message, string contentType)
        {
            var charSet = new ContentType(contentType).CharSet.ToLower();
            switch (charSet)
            {
                case "utf-8":
                    return new UTF8Encoding();

                case "utf-16":
                    if (message[0] == 0xff && message[1] == 0xfe)
                    {
                        return new UnicodeEncoding(false, true);
                    }
                    if (message[0] == 0xfe && message[1] == 0xff)
                    {
                        return new UnicodeEncoding(true, true);
                    }

                    throw new InvalidOperationException("No byte order mark detected. Byte order mark required when charset=utf-16");

                case "utf-16le":
                    return new UnicodeEncoding(false, false);

                case "utf-16be":
                    return new UnicodeEncoding(true, false);

                default:                  
                    throw new InvalidOperationException("Unrecognised charset: " + charSet);
            }
        }
    }
}