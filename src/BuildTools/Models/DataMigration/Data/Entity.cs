namespace Aylos.Xrm.Sdk.BuildTools.Models.DataMigration.Data
{
    using System;
    using System.Xml.Serialization;

    [Serializable()]
    [XmlType(AnonymousType = true)]
    public partial class Entity
    {
        [XmlArray("records", IsNullable = false)]
        [XmlArrayItem("record", IsNullable = false)]
        public Record[] Records { get; set; }

        [XmlArray("m2mrelationships", IsNullable = false)]
        [XmlArrayItem("m2mrelationship", IsNullable = false)]
        public Relationship[] Relationships { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "displayname")]
        public string DisplayName { get; set; }
    }
}