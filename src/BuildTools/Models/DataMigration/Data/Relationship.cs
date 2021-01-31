namespace Aylos.Xrm.Sdk.BuildTools.Models.DataMigration.Data
{
    using System;
    using System.Xml.Serialization;

    [Serializable()]
    [XmlType(AnonymousType = true)]
    public partial class Relationship
    {
        [XmlArray("targetids", IsNullable = false)]
        [XmlArrayItem("targetid", IsNullable = false)]
        public string[] TargetIds { get; set; }

        [XmlAttribute(AttributeName = "sourceid")]
        public string SourceId { get; set; }

        [XmlAttribute(AttributeName = "targetentityname")]
        public string TargetEntityName { get; set; }

        [XmlAttribute(AttributeName = "targetentitynameidfield")]
        public string TargetEntityNameIdField { get; set; }

        [XmlAttribute(AttributeName = "m2mrelationshipname")]
        public string RelationshipName { get; set; }
    }
}
