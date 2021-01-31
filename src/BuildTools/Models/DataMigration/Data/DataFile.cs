namespace Aylos.Xrm.Sdk.BuildTools.Models.DataMigration.Data
{
    using System;
    using System.ComponentModel;
    using System.Xml.Serialization;

    [Serializable()]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", ElementName = "entities", IsNullable = false)]
    public partial class DataFile
    {
        [XmlElement("entity")]
        public Entity[] Entities { get; set; }
    }
}