namespace EnergyTrading.Contracts.Atom
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    /// <summary>
    /// Simple representation of an Atom link, sufficient to be included in an XML message.
    /// <para>
    /// Atom is not fully representable in XML schema, but this form gives us enough to
    /// use the link element.
    /// </para>
    /// </summary>
    [DataContract(Namespace = "http://www.w3.org/2005/Atom", Name = "link")]
    [XmlType(Namespace = "http://www.w3.org/2005/Atom", TypeName = "link")]
    public class Link
    {
        /// <summary>
        /// Gets or sets the target href.
        /// <para>
        /// atom:link elements MUST have an href attribute, whose value MUST be a IRI reference
        /// </para>
        /// </summary>
        [DataMember(Name = "href")]
        [XmlAttribute("href")]
        public string Uri { get; set; }

        /// <summary>
        /// Gets or sets the link relationship type; optional.
        /// <para>
        /// Elements MAY have a "rel" attribute that indicates the link
        /// relation type.  If the "rel" attribute is not present, the link
        /// element MUST be interpreted as if the link relation type is
        /// "alternate".
        /// </para>
        /// </summary>
        /// <seealso cref="http://www.iana.org/assignments/link-relations/link-relations.xml"/>
        [DataMember(Name = "rel")]
        [XmlAttribute("rel")]
        public string Rel { get; set; }

        /// <summary>
        /// Gets or sets the link type.
        /// <para>
        /// the "type" attribute's value is an advisory
        /// media type: it is a hint about the type of the representation that is
        /// expected to be returned when the value of the href attribute is
        /// dereferenced.  Note that the type attribute does not override the
        /// actual media type returned with the representation.  Link elements
        /// MAY have a type attribute, whose value MUST conform to the syntax of
        /// a MIME media type [MIMEREG].
        /// </para>
        /// </summary>
        [DataMember(Name = "type")]
        [XmlAttribute("type")]
        public string Type { get; set; }
    }
}