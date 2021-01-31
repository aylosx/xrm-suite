namespace Aylos.Xrm.Sdk.BuildTools.Models.DataMigration.Data
{
    using System;
    using System.Xml.Serialization;

    [Serializable()]
    [XmlType(AnonymousType = true)]
    public partial class Field
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }

        [XmlAttribute(AttributeName = "lookupentity")]
        public string LookupEntity { get; set; }

        [XmlAttribute(AttributeName = "lookupentityname")]
        public string LookupEntityName { get; set; }
    }
}