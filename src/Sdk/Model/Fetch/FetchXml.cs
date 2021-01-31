namespace Aylos.Xrm.Sdk.Models.Fetch
{
    using System;
    using System.ComponentModel;
    using System.Xml.Serialization;

    [Serializable()]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", ElementName = "condition", IsNullable = false)]
    public partial class Condition
    {
        [XmlElement("value")]
        public ConditionValue[] Items { get; set; }

        [XmlAttribute(AttributeName = "column")]
        public string Column { get; set; }

        [XmlAttribute(AttributeName = "attribute")]
        public string Attribute { get; set; }

        [XmlAttribute(AttributeName = "entityname")]
        public string EntityName { get; set; }

        [XmlAttribute(AttributeName = "operator")]
        public ComparisonOperator Operator { get; set; }

        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }
        
        [XmlAttribute(AttributeName = "aggregate")]
        public AggregateType Aggregate { get; set; }

        [XmlIgnore()]
        public bool AggregateSpecified { get; set; }

        [XmlAttribute(AttributeName = "rowaggregate")]
        public RowAggregateType RowAggregate { get; set; }

        [XmlIgnore()]
        public bool RowAggregateSpecified { get; set; }

        [XmlAttribute(AttributeName = "alias")]
        public string Alias { get; set; }

        [XmlAttribute(AttributeName = "uinam")]
        public string UINam { get; set; }

        [XmlAttribute(AttributeName = "uitype")]
        public string UIType { get; set; }

        [XmlAttribute(AttributeName = "uihidden")]
        public TrueFalse01Type UIHidden { get; set; }

        [XmlIgnore()]
        public bool UIHiddenSpecified { get; set; }
    }

    [Serializable()]
    [XmlType(AnonymousType = true)]
    public partial class ConditionValue
    {
        [XmlAttribute(AttributeName = "uiname")]
        public string UIName { get; set; }

        [XmlAttribute(AttributeName = "uitype")]
        public string UIType { get; set; }

        [XmlText()]
        public string Value { get; set; }
    }

    [Serializable()]
    public partial class FieldXmlFieldUIType
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }

        [XmlAttribute(AttributeName = "description")]
        public string Description { get; set; }

        [XmlAttribute(AttributeName = "languagecode", DataType = "positiveInteger")]
        public string LanguageCode { get; set; }
    }

    [Serializable()]
    public partial class SerializedTrueFalse01Type
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlText()]
        public TrueFalse01Type Value { get; set; }
    }

    [Serializable()]
    public enum TrueFalse01Type
    {
        [XmlEnum("0")]
        Item0,
        [XmlEnum("1")]
        Item1,
    }

    [Serializable()]
    public partial class SerializedInteger
    {
        [XmlAttribute(AttributeName = "formattedvalue")]
        public string FormattedValue { get; set; }

        [XmlText(DataType = "nonNegativeInteger")]
        public string Value { get; set; }
    }

    [Serializable()]
    public partial class FetchLinkEntityType
    {
        [XmlElement("all-attributes", typeof(AllAttributes))]
        [XmlElement("attribute", typeof(FetchAttributeType))]
        [XmlElement("filter", typeof(Filter))]
        [XmlElement("link-entity", typeof(FetchLinkEntityType))]
        [XmlElement("order", typeof(FetchOrderType))]
        public object[] Items { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "to")]
        public string To { get; set; }

        [XmlAttribute(AttributeName = "from")]
        public string From { get; set; }

        [XmlAttribute(AttributeName = "alias")]
        public string Alias { get; set; }

        [XmlAttribute(AttributeName = "link-type")]
        public string LinkType { get; set; }

        [XmlAttribute(AttributeName = "visible")]
        public bool Visible { get; set; }

        [XmlIgnore()]
        public bool VisibleSpecified { get; set; }

        [XmlAttribute(AttributeName = "intersect")]
        public bool Intersect { get; set; }

        [XmlIgnore()]
        public bool IntersectSpecified { get; set; }

        [XmlAttribute(AttributeName = "enableprefiltering")]
        public bool EnablePreFiltering { get; set; }

        [XmlIgnore()]
        public bool EnablePreFilteringSpecified { get; set; }

        [XmlAttribute(AttributeName = "prefilterparametername")]
        public string PreFilterParameterName { get; set; }
    }

    [Serializable()]
    [XmlType(AnonymousType = true)]
    [XmlRoot("all-attributes", Namespace = "", IsNullable = false)]
    public partial class AllAttributes
    {
    }

    [Serializable()]
    public partial class FetchAttributeType
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "build")]
        public Build Build { get; set; }

        [XmlIgnore()]
        public bool BuildSpecified { get; set; }

        [XmlAttribute(AttributeName = "addedby")]
        public string AddedBy { get; set; }

        [XmlAttribute(AttributeName = "alias")]
        public string Alias { get; set; }

        [XmlAttribute(AttributeName = "aggregate")]
        public AggregateType Aggregate { get; set; }

        [XmlIgnore()]
        public bool AggregateSpecified { get; set; }

        [XmlAttribute(AttributeName = "groupby")]
        public FetchBoolType GroupBy { get; set; }

        [XmlIgnore()]
        public bool GroupBySpecified { get; set; }

        [XmlAttribute(AttributeName = "dategrouping")]
        public DateGroupingType DateGrouping { get; set; }

        [XmlIgnore()]
        public bool DateGroupingSpecified { get; set; }

        [XmlAttribute(AttributeName = "usertimezone")]
        public FetchBoolType UserTimeZone { get; set; }

        [XmlIgnore()]
        public bool UserTimeZoneSpecified { get; set; }

        [XmlAttribute(AttributeName = "distinct")]
        public FetchBoolType Distinct { get; set; }

        [XmlIgnore()]
        public bool DistinctSpecified { get; set; }
    }

    [Serializable()]
    [XmlType(TypeName = "build")]
    public enum Build
    {
        [XmlEnum("1.504021")]
        Item1504021,
        [XmlEnum("1.003017")]
        Item1003017,
    }

    [Serializable()]
    public enum AggregateType
    {
        [XmlEnum("count")]
        Count,
        [XmlEnum("countcolumn")]
        CountColumn,
        [XmlEnum("sum")]
        Sum,
        [XmlEnum("avg")]
        Avg,
        [XmlEnum("min")]
        Min,
        [XmlEnum("max")]
        Max,
    }

    [Serializable()]
    public enum FetchBoolType
    {
        [XmlEnum("true")]
        True,
        [XmlEnum("false")]
        False,
        [XmlEnum("1")]
        Item1,
        [XmlEnum("0")]
        Item0,
    }

    [Serializable()]
    public enum DateGroupingType
    {
        [XmlEnum("day")]
        Day,
        [XmlEnum("week")]
        Week,
        [XmlEnum("month")]
        Month,
        [XmlEnum("quarter")]
        Quarter,
        [XmlEnum("year")]
        Year,
        [XmlEnum("fiscal-period")]
        FiscalPeriod,
        [XmlEnum("fiscal-year")]
        FiscalYear,
    }

    [Serializable()]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", ElementName = "filter", IsNullable = false)]
    public partial class Filter
    {
        public Filter()
        {
            FilterType = FilterType.And;
        }

        [XmlElement("condition", typeof(Condition))]
        [XmlElement("filter", typeof(Filter))]
        public object[] Items { get; set; }

        [XmlAttribute(AttributeName = "type")]
        [DefaultValue(FilterType.And)]
        public FilterType FilterType { get; set; }

        [XmlAttribute(AttributeName = "isquickfindfields")]
        public bool IsQuickFindFields { get; set; }

        [XmlIgnore()]
        public bool IsQuickFindFieldsSpecified { get; set; }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, TypeName = "type")]
    public enum FilterType
    {
        [XmlEnum("and")]
        And,
        [XmlEnum("or")]
        Or,
    }

    [Serializable()]
    public partial class FetchOrderType
    {
        public FetchOrderType()
        {
            Descending = false;
        }

        public object[] Items { get; set; }

        [XmlAttribute(AttributeName = "attribute")]
        public string Attribute { get; set; }

        [XmlAttribute(AttributeName = "alias")]
        public string Alias { get; set; }

        [XmlAttribute(AttributeName = "descending")]
        [DefaultValue(false)]
        public bool Descending { get; set; }
    }

    [Serializable()]
    public partial class FetchEntityType
    {
        [XmlElement("all-attributes", typeof(AllAttributes))]
        [XmlElement("attribute", typeof(FetchAttributeType))]
        [XmlElement("filter", typeof(Filter))]
        [XmlElement("link-entity", typeof(FetchLinkEntityType))]
        [XmlElement("order", typeof(FetchOrderType))]
        public object[] Items { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "enableprefiltering")]
        public bool EnablePreFiltering { get; set; }

        [XmlIgnore()]
        public bool EnablePreFilteringSpecified { get; set; }

        [XmlAttribute(AttributeName = "prefilterparametername")]
        public string PreFilterParameterName { get; set; }
    }

    [Serializable()]
    [XmlType(TypeName = "operator")]
    public enum ComparisonOperator
    {
        [XmlEnum("eq")]
        Equals,
        [XmlEnum("neq")]
        DoesNotEqual,
        [XmlEnum("ne")]
        DoesNotEqualTo,
        [XmlEnum("gt")]
        GreaterThan,
        [XmlEnum("ge")]
        GreaterEqual,
        [XmlEnum("le")]
        LessEqual,
        [XmlEnum("lt")]
        LessThan,
        [XmlEnum("like")]
        Like,
        [XmlEnum("not-like")]
        NotLike,
        [XmlEnum("in")]
        In,
        [XmlEnum("not-in")]
        NotIn,
        [XmlEnum("between")]
        Between,
        [XmlEnum("not-between")]
        NotBetween,
        [XmlEnum("null")]
        Null,
        [XmlEnum("not-null")]
        NotNull,
        [XmlEnum("yesterday")]
        Yesterday,
        [XmlEnum("today")]
        Today,
        [XmlEnum("tomorrow")]
        Tomorrow,
        [XmlEnum("last-seven-days")]
        LastSevenDays,
        [XmlEnum("next-seven-days")]
        NextSevenDays,
        [XmlEnum("last-week")]
        LastWeek,
        [XmlEnum("this-week")]
        ThisWeek,
        [XmlEnum("next-week")]
        NextWeek,
        [XmlEnum("last-month")]
        LastMonth,
        [XmlEnum("this-month")]
        ThisMonth,
        [XmlEnum("next-month")]
        NextMonth,
        [XmlEnum("on")]
        On,
        [XmlEnum("on-or-before")]
        OnOrBefore,
        [XmlEnum("on-or-after")]
        OnOrAfter,
        [XmlEnum("last-year")]
        LastYear,
        [XmlEnum("this-year")]
        ThisYear,
        [XmlEnum("next-year")]
        NextYear,
        [XmlEnum("last-x-hours")]
        LastXHours,
        [XmlEnum("next-x-hours")]
        NextXHours,
        [XmlEnum("last-x-days")]
        LastXDays,
        [XmlEnum("next-x-days")]
        NextXDays,
        [XmlEnum("last-x-weeks")]
        LastXWeeks,
        [XmlEnum("next-x-weeks")]
        NextXWeeks,
        [XmlEnum("last-x-months")]
        LastXMonths,
        [XmlEnum("next-x-months")]
        NextXMonths,
        [XmlEnum("OlderThan-x-months")]
        OlderThanXMonths,
        [XmlEnum("OlderThan-x-years")]
        OlderThanXYears,
        [XmlEnum("OlderThan-x-weeks")]
        OlderThanXWeeks,
        [XmlEnum("OlderThan-x-days")]
        OlderThanXDays,
        [XmlEnum("OlderThan-x-hours")]
        OlderThanXHours,
        [XmlEnum("OlderThan-x-minutes")]
        OlderThanXMinutes,
        [XmlEnum("last-x-years")]
        LastXYears,
        [XmlEnum("next-x-years")]
        NextXYears,
        [XmlEnum("eq-userid")]
        EqualsCurrentUser,
        [XmlEnum("ne-userid")]
        DoesNotEqualCurrentUser,
        [XmlEnum("eq-userteams")]
        EqualsCurrentUserTeams,
        [XmlEnum("eq-useroruserteams")]
        EqualsCurrentUserOrUserTeams,
        [XmlEnum("eq-useroruserhierarchy")]
        EqualsCurrentUserOrUserHierarchy,
        [XmlEnum("eq-useroruserhierarchyandteams")]
        EqualsCurrentUserOrUserHierarchyAndTeams,
        [XmlEnum("eq-businessid")]
        EqualsCurrentBusinessUnit,
        [XmlEnum("ne-businessid")]
        DoesNotEqualCurrentBusinessUnit,
        [XmlEnum("eq-userlanguage")]
        EqualsUserLanguage,
        [XmlEnum("this-fiscal-year")]
        ThisFiscalYear,
        [XmlEnum("this-fiscal-period")]
        ThisFiscalPeriod,
        [XmlEnum("next-fiscal-year")]
        NextFiscalYear,
        [XmlEnum("next-fiscal-period")]
        NextFiscalPeriod,
        [XmlEnum("last-fiscal-year")]
        LastFiscalYear,
        [XmlEnum("last-fiscal-period")]
        LastFiscalPeriod,
        [XmlEnum("last-x-fiscal-years")]
        LastXFiscalYears,
        [XmlEnum("last-x-fiscal-periods")]
        LastXFiscalPeriods,
        [XmlEnum("next-x-fiscal-years")]
        NextXFiscalYears,
        [XmlEnum("next-x-fiscal-periods")]
        NextXFiscalPeriods,
        [XmlEnum("in-fiscal-year")]
        InFiscalYear,
        [XmlEnum("in-fiscal-period")]
        InFiscalPeriod,
        [XmlEnum("in-fiscal-period-and-year")]
        InFiscalPeriodAndYear,
        [XmlEnum("in-or-before-fiscal-period-and-year")]
        InOrBeforeFiscalPeriodAndYear,
        [XmlEnum("in-or-after-fiscal-period-and-year")]
        InOrAfterFiscalPeriodAndYear,
        [XmlEnum("begins-with")]
        BeginsWith,
        [XmlEnum("not-begin-with")]
        NotBeginWith,
        [XmlEnum("ends-with")]
        EndsWith,
        [XmlEnum("not-end-with")]
        NotEndWith,
        [XmlEnum("under")]
        Under,
        [XmlEnum("eq-or-under")]
        EqualOrUnder,
        [XmlEnum("not-under")]
        NotUnder,
        [XmlEnum("above")]
        Above,
        [XmlEnum("eq-or-above")]
        EqualOrAbove,
        [XmlEnum("contain-values")]
        ContainValues,
        [XmlEnum("not-contain-values")]
        DoesNotContainValues,
    }

    [Serializable()]
    public enum RowAggregateType
    {
        [XmlEnum("countchildren")]
        CountChildren,
    }

    [Serializable()]
    [XmlRoot(Namespace = "", ElementName = "fetch", IsNullable = false)]
    public partial class FetchXml
    {
        public FetchXml()
        {
            MinActiveRowVersion = false;
            ReturnTotalRecordCount = false;
            Nolock = false;
        }

        [XmlElement("entity", typeof(FetchEntityType))]
        [XmlElement("order", typeof(FetchOrderType))]
        public object[] Items { get; set; }

        [XmlAttribute(AttributeName = "version")]
        public string Version { get; set; }

        [XmlAttribute(AttributeName = "count", DataType = "integer")]
        public string Count { get; set; }

        [XmlAttribute(AttributeName = "page", DataType = "integer")]
        public string Page { get; set; }

        [XmlAttribute(AttributeName = "paging-cookie")]
        public string PagingCookie { get; set; }

        [XmlAttribute(AttributeName = "utc-offset")]
        public string UtcOffset { get; set; }

        [XmlAttribute(AttributeName = "aggregate")]
        public bool Aggregate { get; set; }

        [XmlIgnore()]
        public bool AggregateSpecified { get; set; }

        [XmlAttribute(AttributeName = "distinct")]
        public bool Distinct { get; set; }

        [XmlIgnore()]
        public bool DistinctSpecified { get; set; }

        [XmlAttribute(AttributeName = "top", DataType = "integer")]
        public string Top { get; set; }

        [XmlAttribute(AttributeName = "mapping")]
        public FetchTypeMapping Mapping { get; set; }

        [XmlIgnore()]
        public bool MappingSpecified { get; set; }

        [XmlAttribute(AttributeName = "min-active-row-version")]
        [DefaultValue(false)]
        public bool MinActiveRowVersion { get; set; }

        [XmlAttribute(AttributeName = "output-format")]
        public FetchTypeOutputformat OutputFormat { get; set; }

        [XmlIgnore()]
        public bool OutputFormatSpecified { get; set; }

        [XmlAttribute(AttributeName = "returntotalrecordcount")]
        [DefaultValue(false)]
        public bool ReturnTotalRecordCount { get; set; }

        [XmlAttribute(AttributeName = "no-lock")]
        [DefaultValue(false)]
        public bool Nolock { get; set; }
    }

    [Serializable()]
    [XmlType(AnonymousType = true)]
    public enum FetchTypeMapping
    {
        [XmlEnum("internal")]
        Internal,
        [XmlEnum("logical")]
        Logical,
    }

    [Serializable()]
    [XmlType(AnonymousType = true)]
    public enum FetchTypeOutputformat
    {
        [XmlEnum("xml-ado")]
        XmlAdo,
        [XmlEnum("xml-auto")]
        XmlAuto,
        [XmlEnum("xml-elements")]
        XmlElements,
        [XmlEnum("xml-raw")]
        XmlRaw,
        [XmlEnum("xml-platform")]
        XmlPlatform,
    }

    [Serializable()]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", ElementName = "savedquery", IsNullable = false)]
    public partial class SavedQuery
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "savedqueryid")]
        public string SavedQueryId { get; set; }

        [XmlAttribute(AttributeName = "returnedtypecode")]
        public SerializedInteger ReturnedTypeCode { get; set; }

        [XmlAttribute(AttributeName = "description")]
        public string Description { get; set; }

        [XmlAttribute(AttributeName = "querytype")]
        public SerializedInteger QueryType { get; set; }

        public SerializedTrueFalse01Type IsCustomizable { get; set; }

        public SerializedTrueFalse01Type CanBeDeleted { get; set; }

        public string IntroducedVersion { get; set; }

        [XmlAttribute(AttributeName = "isquickfindquery")]
        public SerializedTrueFalse01Type IsQuickFindQuery { get; set; }

        [XmlAttribute(AttributeName = "isuserdefined")]
        public SerializedTrueFalse01Type IsUserDefined { get; set; }

        [XmlAttribute(AttributeName = "isdefault")]
        public SerializedTrueFalse01Type IsDefault { get; set; }

        [XmlAttribute(AttributeName = "isprivate")]
        public bool IsPrivate { get; set; }

        [XmlIgnore()]
        public bool IsPrivateSpecified { get; set; }

        [XmlAttribute(AttributeName = "queryapi")]
        public string QueryApi { get; set; }

        [XmlAttribute(AttributeName = "fetchxml")]
        public SavedQueryFetchXml FetchXml { get; set; }

        [XmlAttribute(AttributeName = "columnsetxml")]
        public SavedQueryColumnSetXml ColumnSetXml { get; set; }

        [XmlAttribute(AttributeName = "layoutxml")]
        public SavedQueryLayoutXml LayoutXml { get; set; }

        [XmlAttribute(AttributeName = "donotuseinLCID")]
        public string DoNotUseInLCID { get; set; }

        [XmlAttribute(AttributeName = "useinLCID")]
        public string UseInLCID { get; set; }
    }

    [Serializable()]
    [XmlType(AnonymousType = true)]
    public partial class SavedQueryFetchXml
    {
        [XmlAttribute(AttributeName = "fetch")]
        public FetchXml FetchXml { get; set; }
    }

    [Serializable()]
    [XmlType(AnonymousType = true)]
    public partial class SavedQueryColumnSetXml
    {
        [XmlAttribute(AttributeName = "columnset")]
        public SavedQueryColumnSetXmlColumnSet ColumnSet { get; set; }
    }

    [Serializable()]
    [XmlType(AnonymousType = true)]
    public partial class SavedQueryColumnSetXmlColumnSet
    {
        [XmlElement("ascend", typeof(object))]
        [XmlElement("column", typeof(SavedQueryColumnSetXmlColumnSetColumn))]
        [XmlElement("descend", typeof(object))]
        [XmlElement("filter", typeof(SavedQueryColumnSetXmlColumnSetFilter))]
        [XmlChoiceIdentifier("ItemsElementName")]
        public object[] Items { get; set; }

        [XmlElement("ItemsElementName")]
        [XmlIgnore()]
        public ItemsChoiceType[] ItemsElementName { get; set; }

        [XmlAttribute(AttributeName = "version")]
        public string Version { get; set; }

        [XmlAttribute(AttributeName = "distinct")]
        public bool Distinct { get; set; }

        [XmlIgnore()]
        public bool DistinctSpecified { get; set; }
    }

    [Serializable()]
    [XmlType(AnonymousType = true)]
    public partial class SavedQueryColumnSetXmlColumnSetColumn
    {
        [XmlAttribute(AttributeName = "build")]
        public Build Build { get; set; }

        [XmlIgnore()]
        public bool BuildSpecified { get; set; }

        [XmlAttribute(AttributeName = "addedby")]
        public string AddedBy { get; set; }

        [XmlText()]
        public string Value { get; set; }
    }

    [Serializable()]
    [XmlType(AnonymousType = true)]
    public partial class SavedQueryColumnSetXmlColumnSetFilter
    {

        [XmlElement("condition")]
        public SavedQueryColumnSetXmlColumnSetFilterCondition[] Condition { get; set; }

        [XmlAttribute(AttributeName = "column")]
        public string Column { get; set; }

        [XmlAttribute(AttributeName = "operator")]
        public ComparisonOperator Operator { get; set; }

        [XmlIgnore()]
        public bool OperatorSpecified { get; set; }

        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }

        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
    }

    [Serializable()]
    [XmlType(AnonymousType = true)]
    public partial class SavedQueryColumnSetXmlColumnSetFilterCondition
    {
        [XmlAttribute(AttributeName = "column")]
        public string Column { get; set; }

        [XmlAttribute(AttributeName = "operator")]
        public ComparisonOperator Operator { get; set; }

        [XmlIgnore()]
        public bool OperatorSpecified { get; set; }

        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }
    }

    [Serializable()]
    [XmlType(IncludeInSchema = false)]
    public enum ItemsChoiceType
    {
        [XmlEnum("ascend")]
        Ascend,
        [XmlEnum("column")]
        Column,
        [XmlEnum("descend")]
        Descend,
        [XmlEnum("filter")]
        Filter,
    }

    [Serializable()]
    [XmlType(AnonymousType = true)]
    public partial class SavedQueryLayoutXml
    {
        [XmlAttribute(AttributeName = "grid")]
        public SavedQueryLayoutXmlGrid Grid { get; set; }
    }

    [Serializable()]
    [XmlType(AnonymousType = true)]
    public partial class SavedQueryLayoutXmlGrid
    {
        [XmlAttribute(AttributeName = "row")]
        public SavedQueryLayoutXmlGridRow Row { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "select")]
        public bool Select { get; set; }

        [XmlAttribute(AttributeName = "preview")]
        public string Preview { get; set; }

        [XmlAttribute(AttributeName = "icon")]
        public string Icon { get; set; }

        [XmlAttribute(AttributeName = "jump")]
        public string Jump { get; set; }

        [XmlAttribute(AttributeName = "@object", DataType = "integer")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        public string Object { get; set; }

        [XmlAttribute(AttributeName = "disableInlineEditing", DataType = "integer")]
        public string DisableInlineEditing { get; set; }

        [XmlAttribute(AttributeName = "iconrenderer")]
        public string IconRenderer { get; set; }

        [XmlAttribute(AttributeName = "multilinerows")]
        public string MultilineRows { get; set; }
    }

    [Serializable()]
    [XmlType(AnonymousType = true)]
    public partial class SavedQueryLayoutXmlGridRow
    {
        [XmlElement("cell")]
        public SavedQueryLayoutXmlGridRowCell[] Cell { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }

        [XmlAttribute(AttributeName = "multiobjectidfield")]
        public string MultiObjectIdField { get; set; }

        [XmlAttribute(AttributeName = "layoutstyle")]
        public string LayoutStyle { get; set; }
    }

    [Serializable()]
    [XmlType(AnonymousType = true)]
    public partial class SavedQueryLayoutXmlGridRowCell
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "width", DataType = "integer")]
        public string Width { get; set; }

        public string LabelId { get; set; }

        [XmlAttribute(AttributeName = "label")]
        public string Label { get; set; }

        [XmlAttribute(AttributeName = "desc")]
        public string Description { get; set; }

        [XmlAttribute(AttributeName = "ishidden", DataType = "integer")]
        public string IsHidden { get; set; }

        [XmlAttribute(AttributeName = "disableSorting", DataType = "integer")]
        public string DisableSorting { get; set; }

        [XmlAttribute(AttributeName = "disableMetaDataBinding", DataType = "integer")]
        public string DisableMetaDataBinding { get; set; }

        [XmlAttribute(AttributeName = "cellType")]
        public string CellType { get; set; }

        [XmlAttribute(AttributeName = "imageproviderwebresource")]
        public string ImageProviderWebResource { get; set; }

        [XmlAttribute(AttributeName = "imageproviderfunctionname")]
        public string ImageProviderFunctionName { get; set; }
    }
}

