namespace Aylos.Xrm.Sdk.BuildTools.Models.DataMigration.Schema
{
    using System;
    using System.Xml.Serialization;

    /// <remarks/>
    [Serializable()]
    [XmlType(AnonymousType = true)]
    public partial class Field
    {

        [XmlAttribute(AttributeName = "customfield")]
        public bool CustomField { get; set; }

        [XmlIgnore()]
        public bool CustomFieldSpecified { get; set; }

        [XmlAttribute(AttributeName = "dateMode")]
        public DateMode DateMode { get; set; }

        [XmlIgnore]
        public bool DateModeSpecified { get; set; }

        [XmlAttribute(AttributeName = "displayname")]
        public string DisplayName { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }

        [XmlAttribute(AttributeName = "lookupType")]
        public string LookupType { get; set; }

        [XmlAttribute(AttributeName = "primaryKey")]
        public bool PrimaryKey { get; set; }

        [XmlIgnore()]
        public bool PrimaryKeySpecified { get; set; }

        [XmlAttribute(AttributeName = "updateCompare")]
        public bool UpdateCompare { get; set; }

        [XmlIgnore]
        public bool UpdateCompareSpecified { get; set; }
    }
}