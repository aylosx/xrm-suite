namespace Aylos.Xrm.Sdk.BuildTools.Models.DataMigration.Data
{
    using System;
    using System.Xml.Serialization;

    [Serializable()]
    [XmlType(AnonymousType = true)]
    public partial class Record
    {
        [XmlElement("field")]
        public Field[] Fields { get; set; }

        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }
}