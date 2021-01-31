namespace Aylos.Xrm.Sdk.BuildTools.Models.DataMigration.Schema
{
    using System;
    using System.Xml.Serialization;

    [Serializable()]
    [XmlType(AnonymousType = true)]
    public partial class Filter
    {
        [XmlAttribute(AttributeName = "type")]
        public FilterType FilterType { get; set; }

        [XmlElement("condition", typeof(Condition))]
        public Condition[] Conditions { get; set; }

        [XmlElement("filter", typeof(Filter))]
        public Filter[] Filters { get; set; }
    }

    [Serializable()]
    public enum FilterType
    {
        [XmlEnum(Name = "and")] And = 0,
        [XmlEnum(Name = "or")] Or = 1
    }
}