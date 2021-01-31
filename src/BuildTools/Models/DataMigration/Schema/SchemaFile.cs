namespace Aylos.Xrm.Sdk.BuildTools.Models.DataMigration.Schema
{
    using System;
    using System.Xml.Serialization;

    [Serializable()]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", ElementName = "entities", IsNullable = false)]
    public partial class SchemaFile
    {
        [XmlAttribute(AttributeName = "dateMode")]
        public DateMode DateMode { get; set; }

        [XmlIgnore]
        public bool DateModeSpecified { get; set; }

        [XmlElement("entity")]
        public Entity[] Entities { get; set; }

        [XmlArrayItem("entityName", IsNullable = false)]
        public string[] EntityImportOrder { get; set; }
    }
}