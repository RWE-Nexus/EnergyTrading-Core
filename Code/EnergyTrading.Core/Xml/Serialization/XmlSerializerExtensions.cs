namespace EnergyTrading.Xml.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    using EnergyTrading.Extensions;
    using EnergyTrading.IO;
    using EnergyTrading.Logging;

    /// <summary>
    /// Extensions for XML serialization
    /// </summary>
    public static class XmlSerializerExtensions
    {
        private static readonly ILogger Logger = LoggerFactory.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Load an instance of <see typeref="T" />.
        /// </summary>
        /// <typeparam name="T">The type to load</typeparam>
        /// <param name="fileName">Name of file, can include root relative paths if in a web application.</param>
        /// <returns>Instance of T.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Polymorphic method")]
        public static T LoadXmlDocument<T>(this string fileName)
        {
            return LoadXmlDocument<T>(() => new StreamReader(fileName.MapPath()));
        }

        /// <summary>
        /// Load an instance of <see typeref="T" />.
        /// </summary>
        /// <typeparam name="T">The type to load</typeparam>
        /// <param name="stream">Stream to load from.</param>
        /// <returns>Instance of T.</returns>
        public static T LoadXmlDocument<T>(this Stream stream)
        {
            return LoadXmlDocument<T>(() => new StreamReader(stream));
        }

        /// <summary>
        /// Load an instance of <see typeref="T" />.
        /// </summary>
        /// <typeparam name="T">The type to load</typeparam>
        /// <param name="func">Function to provide a <see cref="StreamReader" /></param>
        /// <returns>Instance of T.</returns>
        public static T LoadXmlDocument<T>(Func<TextReader> func)
        {
            var serializer = new XmlSerializer(typeof(T));
            TextReader reader = null;

            try
            {
                reader = func();
                return (T)serializer.Deserialize(reader);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        [Obsolete("Use XmlDeserializer<T>")]
        public static T XmlDeserilizer<T>(this string xml)
        {
            return LoadXmlDocument<T>(() => new StringReader(xml));
        }

        /// <summary>
        /// Deserialize an entity from XML.
        /// </summary>
        /// <typeparam name="T">Type to deserialize</typeparam>
        /// <param name="xml">XML to parse.</param>
        /// <returns></returns>
        public static T XmlDeserializer<T>(this string xml)
        {
            return LoadXmlDocument<T>(() => new StringReader(xml));
        }

        /// <summary>
        /// Serialize an entity to XML.
        /// </summary>
        /// <typeparam name="T">Type to serialize</typeparam>
        /// <param name="entity">Entity to use</param>
        /// <param name="fileName">File to create</param>
        /// <param name="types">Subtypes required for serialization</param>
        /// <param name="settings">Parameters to be used for the XML</param>
        public static void XmlSerialize<T>(this T entity, string fileName, Type[] types = null, XmlWriterSettings settings = null)
        {
            using (var stream = new StreamWriter(fileName))
            {
                stream.TextWriterXmlSerialize(entity, types, settings);
            }
        }

        /// <summary>
        /// Serialize an entity to XML
        /// </summary>
        /// <typeparam name="T">Type to serialize</typeparam>
        /// <param name="entity">Entity to use</param>
        /// <param name="types">Subtypes required for serialization</param>
        /// <param name="settings">Parameters to be used for the XML</param>        
        /// <returns></returns>
        public static string XmlSerialize<T>(this T entity, Type[] types = null, XmlWriterSettings settings = null)
        {
            using (var stream = new StringWriter())
            {
                stream.TextWriterXmlSerialize(entity, types, settings);
                return stream.ToString();
            }
        }

        /// <summary>
        /// XML serialize an entity to a TextWriter.
        /// </summary>
        /// <param name="textWriter"></param>
        /// <typeparam name="T">Type to serialize</typeparam>
        /// <param name="entity">Entity to use</param>
        /// <param name="types">Subtypes required for serialization</param>
        /// <param name="settings">Parameters to be used for the XML</param>    
        public static void TextWriterXmlSerialize<T>(this TextWriter textWriter, T entity, Type[] types = null, XmlWriterSettings settings = null)
        {
            if (settings == null)
            {
                settings = DefaultXmlWriterSettings();
            }

            using (var writer = XmlWriter.Create(textWriter, settings))
            {
                var serializer = XmlSerializer<T>(types);

                serializer.Serialize(writer, entity);
                writer.Close();
            }
        }

        /// <summary>     
        /// Perform a deep copy of the object using the DataContractSerializer
        /// </summary>  
        /// <typeparam name="T">The type of object being copied.</typeparam> 
        /// <param name="source">The object instance to copy.</param>    
        /// <returns>The copied object.</returns>     
        public static T DataContractDeepCopy<T>(this T source)
        {
            if (!typeof(T).DecoratedWith<DataContractAttribute>())
            {
                throw new ArgumentException("The type must be have a DataContract attribute.", "source");
            }

            // Don't serialize a null object, simply return the default for that object 
            if (ReferenceEquals(source, null))
            {
                return default(T);
            }

            using (var stream = new MemoryStream())
            {
                var formatter = new DataContractSerializer(typeof(T));
                formatter.WriteObject(stream, source);
                stream.Seek(0, SeekOrigin.Begin);

                using (var reader = XmlDictionaryReader.CreateTextReader(stream, new XmlDictionaryReaderQuotas()))
                {
                    return (T)formatter.ReadObject(reader, true);
                }
            }
        }

        public static void DataContractSerialize<T>(this T entity, string fileName, Type[] types = null)
        {
            using (var stream = new StreamWriter(fileName))
            {
                stream.TextWriterDataContractSerialize(entity, types);
            }
        }

        public static string DataContractSerialize<T>(this T entity, Type[] types = null)
        {
            using (var stream = new StringWriter())
            {
                stream.TextWriterDataContractSerialize(entity, types);
                return stream.ToString();
            }
        }

        /// <summary>
        /// DataContract serialize an entity to a TextWriter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="textWriter"></param>
        /// <param name="entity"></param>
        /// <param name="types"></param>
        public static void TextWriterDataContractSerialize<T>(this TextWriter textWriter, T entity, Type[] types = null)
        {
            using (var writer = XmlDictionaryWriter.CreateDictionaryWriter(XmlWriter.Create(textWriter)))
            {
                var serializer = DcSerializer<T>(types);
                serializer.WriteObject(writer, entity);
                writer.Close();
            }            
        }

        public static T DeserializeDataContractXml<T>(this string fileName, Type[] types = null)
        {
            T result;

            using (var fs = new FileStream(fileName, FileMode.Open))
            {
                using (var reader = XmlDictionaryReader.CreateDictionaryReader(XmlReader.Create(fs)))
                {
                    var serializer = DcSerializer<T>(types);
                    result = (T)serializer.ReadObject(reader);
                }
            }

            return result;
        }

        public static T DeserializeDataContractXmlString<T>(this string serializedXml, Type[] types = null)
        {
            T result;

            using (var fs = new StringReader(serializedXml))
            {
                using (var reader = XmlDictionaryReader.CreateDictionaryReader(XmlReader.Create(fs)))
                {
                    var serializer = DcSerializer<T>(types);
                    result = (T)serializer.ReadObject(reader);
                }
            }

            return result;
        }

        public static object DeserializeDataContractXmlString(this string serializedXml, Type type, Type[] types = null)
        {
            object result;
            using (var fs = new StringReader(serializedXml))
            {
                using (var reader = XmlDictionaryReader.CreateDictionaryReader(XmlReader.Create(fs)))
                {
                    var serializer = DcSerializer(type, types);
                    result = serializer.ReadObject(reader);
                }
            }
            return result;
        }

        public static T DeserializeDataContractBinary<T>(this string fileName, Type[] types = null)
        {
            T result;

            using (var fs = new FileStream(fileName, FileMode.Open))
            {
                var reader = XmlDictionaryReader.CreateBinaryReader(fs, XmlDictionaryReaderQuotas.Max);
                {
                    var serializer = DcSerializer<T>(types);
                    result = (T)serializer.ReadObject(reader);
                }
            }

            return result;
        }

        /// <summary>
        /// Convert a <see cref="XDocument" /> to a <see cref="XmlDocument" />
        /// </summary>
        /// <param name="value">XDocument to convert.</param>
        /// <returns>Same content as a XmlDocument.</returns>
        public static XmlDocument ToXmlDocument(this XDocument value)
        {
            var xmlDocument = new XmlDocument();
            using (var xmlReader = value.CreateReader())
            {
                xmlDocument.Load(xmlReader);
            }

            return xmlDocument;
        }

        /// <summary>
        /// Convert a <see cref="XmlDocument" /> to a <see cref="XDocument" />
        /// </summary>
        /// <param name="value">XmlDocument to convert</param>
        /// <returns>Same content as an XDocument.</returns>
        public static XDocument ToXDocument(this XmlDocument value)
        {
            using (var nodeReader = new XmlNodeReader(value))
            {
                nodeReader.MoveToContent();

                return XDocument.Load(nodeReader);
            }
        }

        /// <summary>
        /// Convert a <see cref="XElement" /> into a formatted XML string.
        /// </summary>
        /// <param name="value">XElement to serialize.</param>
        /// <param name="conformance">Configures the XmlWriter to check and guarantee the conformance to XML 1.0</param>        
        /// <returns>A formatted XML string.</returns>
        public static string ToXmlString(this XElement value, ConformanceLevel conformance = ConformanceLevel.Auto)
        {
            // Could also be any other stream    
            var sb = new StringBuilder();

            using (var xw = XmlWriter.Create(sb, DefaultXmlWriterSettings(conformance)))
            {
                // the actual writing takes place
                value.WriteTo(xw);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Convert a <see cref="XmlNode" /> into a formatted XML string.
        /// </summary>
        /// <param name="value">XmlNode to serialize.</param>
        /// <param name="conformance">Configures the XmlWriter to check and guarantee the conformance to XML 1.0</param>
        /// <returns>A formatted XML string.</returns>
        public static string ToXmlString(this XmlNode value, ConformanceLevel conformance = ConformanceLevel.Auto)
        {
            var sb = new StringBuilder();

            using (var xw = XmlWriter.Create(sb, DefaultXmlWriterSettings(conformance)))
            {
                // the actual writing takes place
                value.WriteTo(xw);
            }

            return sb.ToString();
        }

        public static XmlWriterSettings DefaultXmlWriterSettings(ConformanceLevel conformance = ConformanceLevel.Auto)
        {
            var xws = new XmlWriterSettings
            {
                ConformanceLevel = conformance,
                OmitXmlDeclaration = true,
                Indent = true
            };

            return xws;
        }

        /// <summary>
        /// Write a <see cref="XmlDocument" /> to a <see cref="MemoryStream" />
        /// </summary>
        /// <param name="document">Document to use</param>
        /// <param name="writerSettings">XML writer settings</param>
        /// <returns>A new <see cref="MemoryStream" /> containing the XML document.</returns>
        public static MemoryStream WriteTo(this XDocument document, XmlWriterSettings writerSettings)
        {
            var stream = new MemoryStream();
            using (var writer = XmlWriter.Create(stream, writerSettings))
            {
                document.WriteTo(writer);
            }

            return stream;
        }

        private static DataContractSerializer DcSerializer<T>(ICollection<Type> types = null)
        {
            return DcSerializer(typeof(T), types);
        }

        private static DataContractSerializer DcSerializer(Type type, ICollection<Type> types = null)
        {
            return types == null || types.Count == 0 ? new DataContractSerializer(type) : new DataContractSerializer(type, types);
        }

        private static XmlSerializer XmlSerializer<T>(Type[] types = null)
        {
            return types == null || types.Length == 0 ? new XmlSerializer(typeof(T)) : new XmlSerializer(typeof(T), types);
        }
    }
}