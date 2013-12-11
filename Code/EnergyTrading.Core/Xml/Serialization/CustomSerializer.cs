namespace EnergyTrading.Xml.Serialization
{
    using System;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    /// <summary>
    /// Allows us to serialize unknown subtypes of a given base type without decorating the parent with all subtypes
    /// Utilization is [XmlElement(Type = typeof(CustomerSerializer&lt;baseClass&gt;))]
    /// </summary>
    /// <remarks>Based on http://www.codeproject.com/csharp/XmlSerializerForUnknown.asp and http://mfharoon.blogspot.com/2006/12/using-ixmlserializable-to-overcome-not.html</remarks>
    public class CustomSerializer<T> : IXmlSerializable
    {
        public CustomSerializer()
        {
        }

        public CustomSerializer(T parameters)
        {
            this.Parameters = parameters;
        }

        public T Parameters { get; private set; }

        /// <summary>   
        /// Returns schema of the XML document representation of the object that is produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)"></see> method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)"></see> method.   
        /// </summary>   
        /// <returns>   
        /// An <see cref="T:System.Xml.Schema.XmlSchema"></see> that describes the XML representation of the object that is produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)"></see> method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)"></see> method.   
        /// </returns>   
        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        /// <summary>   
        /// Generates an object from its XML representation.   
        /// </summary>   
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"></see> stream from which the object is deserialized.</param>   
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            // Get type from xml attribute   
            var type = Type.GetType(reader.GetAttribute("type"));

            // Deserialize   
            reader.ReadStartElement();
            this.Parameters = (T)new XmlSerializer(type).Deserialize(reader);
            reader.ReadEndElement();
        }

        /// <summary>   
        /// Converts an object into its XML representation.   
        /// </summary>   
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"></see> stream to which the object is serialized.</param>   
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            // Write type as xml attribute   
            writer.WriteAttributeString("type", this.Parameters.GetType().ToString());
            new XmlSerializer(this.Parameters.GetType()).Serialize(writer, this.Parameters);
        }

        /// <summary>
        /// Implicit operators the specified subtype.
        /// </summary>
        /// <param name="subtype"></param>
        /// <returns></returns>
        public static implicit operator CustomSerializer<T>(T subtype)
        {
            return subtype == null ? null : new CustomSerializer<T>(subtype);
        }

        public static implicit operator T(CustomSerializer<T> subtype)
        {
            return subtype.Equals(default(T)) ? default(T) : subtype.Parameters;
        }
    }
}