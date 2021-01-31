namespace Aylos.Xrm.Sdk.BuildTools.Models.DataMigration.Schema
{
    using System;
    using System.Xml.Serialization;

    [Serializable()]
    [XmlType(AnonymousType = true)]
    public partial class Relationship
    {

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "manyToMany")]
        public bool IsManyToMany { get; set; }

        [XmlAttribute(AttributeName = "isreflexive")]
        public bool IsReflexive { get; set; }

        [XmlAttribute(AttributeName = "relatedEntityName")]
        public string RelatedEntityName { get; set; }

        [XmlAttribute(AttributeName = "m2mTargetEntity")]
        public string TargetEntity { get; set; }

        [XmlAttribute(AttributeName = "m2mTargetEntityPrimaryKey")]
        public string TargetEntityPrimaryKey { get; set; }
    }
}