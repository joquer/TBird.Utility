namespace TBird.Utility.Enumerations
{
    using System;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    [Serializable]
    public class EnumerationSerializer<TEnumeration> : IXmlSerializable where TEnumeration : Enumeration<TEnumeration>
    {
        public EnumerationSerializer()
        {
            this.Enum = Enumeration<TEnumeration>.DefaultValue;
        }

        public TEnumeration Enum { get; set; }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            this.Enum = Enumeration<TEnumeration>.FromString(reader.GetAttribute("Name"));
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("Name", this.Enum.Name);
        }
    }
}
