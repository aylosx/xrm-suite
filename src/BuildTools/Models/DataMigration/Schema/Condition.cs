namespace Aylos.Xrm.Sdk.BuildTools.Models.DataMigration.Schema
{
    using System;
    using System.Xml.Serialization;

    [Serializable()]
    [XmlType(AnonymousType = true)]
    public partial class Condition
    {
        [XmlAttribute(AttributeName = "attribute")]
        public string Attribute { get; set; }

        [XmlAttribute(AttributeName = "operator")]
        public ConditionOperator Operator { get; set; }

        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }

        [XmlElement("value", IsNullable = false)]
        public string[] Values { get; set; }
    }

    [Serializable()]
    public enum ConditionOperator
    {
        [XmlEnum("eq")] Equal = 0,
        [XmlEnum("neq")] NotEqual = 1,
        [XmlEnum("ne")] ne,
        [XmlEnum("gt")] GreaterThan = 2,
        [XmlEnum("ge")] GreaterEqual = 4,
        [XmlEnum("le")] LessEqual = 5,
        [XmlEnum("lt")] LessThan = 3,
        [XmlEnum("like")] Like = 6,
        [XmlEnum("not-like")] NotLike = 7,
        [XmlEnum("in")] In = 8,
        [XmlEnum("not-in")] NotIn = 9,
        [XmlEnum("between")] Between = 10,
        [XmlEnum("not-between")] NotBetween = 11,
        [XmlEnum("null")] Null = 12,
        [XmlEnum("not-null")] NotNull = 13,
        [XmlEnum("yesterday")] Yesterday = 14,
        [XmlEnum("today")] Today = 15,
        [XmlEnum("tomorrow")] Tomorrow = 16,
        [XmlEnum("last-seven-days")] Last7Days = 17,
        [XmlEnum("next-seven-days")] Next7Days = 18,
        [XmlEnum("last-week")] LastWeek = 19,
        [XmlEnum("this-week")] ThisWeek = 20,
        [XmlEnum("next-week")] NextWeek = 21,
        [XmlEnum("last-month")] LastMonth = 22,
        [XmlEnum("this-month")] ThisMonth = 23,
        [XmlEnum("next-month")] NextMonth = 24,
        [XmlEnum("on")] On = 25,
        [XmlEnum("on-or-before")] OnOrBefore = 26,
        [XmlEnum("on-or-after")] OnOrAfter = 27,
        [XmlEnum("last-year")] LastYear = 28,
        [XmlEnum("this-year")] ThisYear = 29,
        [XmlEnum("next-year")] NextYear = 30,
        [XmlEnum("last-x-hours")] LastXHours = 31,
        [XmlEnum("next-x-hours")] NextXHours = 32,
        [XmlEnum("last-x-days")] LastXDays = 33,
        [XmlEnum("next-x-days")] NextXDays = 34,
        [XmlEnum("last-x-weeks")] LastXWeeks = 35,
        [XmlEnum("next-x-weeks")] NextXWeeks = 36,
        [XmlEnum("last-x-months")] LastXMonths = 37,
        [XmlEnum("next-x-months")] NextXMonths = 38,
        [XmlEnum("olderthan-x-months")] OlderThanXMonths = 53,
        [XmlEnum("olderthan-x-years")] OlderThanXYears = 82,
        [XmlEnum("olderthan-x-weeks")] OlderThanXWeeks = 83,
        [XmlEnum("olderthan-x-days")] OlderThanXDays = 84,
        [XmlEnum("olderthan-x-hours")] OlderThanXHours = 85,
        [XmlEnum("olderthan-x-minutes")] OlderThanXMinutes = 86,
        [XmlEnum("last-x-years")] LastXYears = 39,
        [XmlEnum("next-x-years")] NextXYears = 40,
        [XmlEnum("eq-userid")] EqualUserId = 41,
        [XmlEnum("ne-userid")] NotEqualUserId = 42,
        [XmlEnum("eq-userteams")] EqualUserTeams = 73,
        [XmlEnum("eq-useroruserteams")] EqualUserOrUserTeams = 74,
        [XmlEnum("eq-useroruserhierarchy")] EqualUserOrUserHierarchy = 80,
        [XmlEnum("eq-useroruserhierarchyandteams")] EqualUserOrUserHierarchyAndTeams = 81,
        [XmlEnum("eq-businessid")] EqualBusinessId = 43,
        [XmlEnum("ne-businessid")] NotEqualBusinessId = 44,
        [XmlEnum("eq-userlanguage")] EqualUserLanguage = 51,
        [XmlEnum("this-fiscal-year")] ThisFiscalYear = 58,
        [XmlEnum("this-fiscal-period")] ThisFiscalPeriod = 59,
        [XmlEnum("next-fiscal-year")] NextFiscalYear = 60,
        [XmlEnum("next-fiscal-period")] NextFiscalPeriod = 61,
        [XmlEnum("last-fiscal-year")] LastFiscalYear = 62,
        [XmlEnum("last-fiscal-period")] LastFiscalPeriod = 63,
        [XmlEnum("last-x-fiscal-years")] LastXFiscalYears = 64,
        [XmlEnum("last-x-fiscal-periods")] LastXFiscalPeriods = 65,
        [XmlEnum("next-x-fiscal-years")] NextXFiscalYears = 66,
        [XmlEnum("next-x-fiscal-periods")] NextXFiscalPeriods = 67,
        [XmlEnum("in-fiscal-year")] InFiscalYear = 68,
        [XmlEnum("in-fiscal-period")] InFiscalPeriod = 69,
        [XmlEnum("in-fiscal-period-and-year")] InFiscalPeriodAndYear = 70,
        [XmlEnum("in-or-before-fiscal-period-and-year")] InOrBeforeFiscalPeriodAndYear = 71,
        [XmlEnum("in-or-after-fiscal-period-and-year")] InOrAfterFiscalPeriodAndYear = 72,
        [XmlEnum("begins-with")] BeginsWith = 54,
        [XmlEnum("not-begin-with")] DoesNotBeginWith = 55,
        [XmlEnum("ends-with")] EndsWith = 56,
        [XmlEnum("not-end-with")] DoesNotEndWith = 57,
        [XmlEnum("under")] Under = 75,
        [XmlEnum("eq-or-under")] UnderOrEqual = 77,
        [XmlEnum("not-under")] NotUnder = 76,
        [XmlEnum("above")] Above = 78,
        [XmlEnum("eq-or-above")] AboveOrEqual = 79,
        [XmlEnum("contain-values")] ContainValues = 87,
        [XmlEnum("not-contain-values")] DoesNotContainValues = 88
    }
}