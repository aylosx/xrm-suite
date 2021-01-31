namespace Aylos.Xrm.Sdk.BuildTools.Models.DataMigration.Schema
{
    using System;
    using System.Xml.Serialization;

    [Serializable()]
    [XmlType(AnonymousType = true)]
    public partial class Entity
    {
        [XmlArray("fields", IsNullable = false)]
        [XmlArrayItem("field", IsNullable = false)]
        public Field[] Fields { get; set; }

        [XmlArray("relationships", IsNullable = false)]
        [XmlArrayItem("relationship", IsNullable = false)]
        public Relationship[] Relationships { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "displayname")]
        public string DisplayName { get; set; }

        [XmlAttribute(AttributeName = "etc")]
        public short TypeCode { get; set; }

        [XmlAttribute(AttributeName = "primaryidfield")]
        public string PrimaryIdField { get; set; }

        [XmlAttribute(AttributeName = "primarynamefield")]
        public string PrimaryNameField { get; set; }

        [XmlAttribute(AttributeName = "disableplugins")]
        public bool DisablePlugins { get; set; }

        [XmlElement(ElementName = "fetchxml", IsNullable = false)]
        public string FetchXml { get; set; }
    }
}