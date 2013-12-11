namespace EnergyTrading.Xml
{
    using System;
    using System.IO;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Schema;

    public class SimpleSchemaValidator
    {
        public static bool TryValidate(Stream schema, XElement sourceXml, string defaultNamespace = "")
        {
            try
            {
                Validate(schema, sourceXml, defaultNamespace);
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
            catch (XmlException)
            {
                return false;
            }
            catch (XmlSchemaValidationException)
            {
                return false;
            }
        }

        public static void Validate(Stream schema, XElement sourceElement, string defaultNamespace)
        {
            var booksSettings = new XmlReaderSettings();
            booksSettings.Schemas.Add(defaultNamespace, LoadReaderFromStream(schema));
            booksSettings.ValidationType = ValidationType.Schema;
            using (var elementStream = GetXElementStream(sourceElement))
            {
                var books = XmlReader.Create(elementStream, booksSettings);
                while (books.Read())
                {
                }
            }
        }

        private static XmlReader LoadReaderFromStream(Stream schema)
        {
            if (schema == null)
            {
                throw new ArgumentOutOfRangeException("schema");
            }

            if (!schema.CanRead)
            {
                throw new ArgumentOutOfRangeException("schema", "Cannot read schema stream object");
            }

            if (schema.CanSeek)
            {
                schema.Seek(0, SeekOrigin.Begin);
            }
            return XmlReader.Create(schema);
        }

        private static Stream GetXElementStream(XElement element)
        {
            var stream = new MemoryStream();
            element.Save(stream);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
    }
}