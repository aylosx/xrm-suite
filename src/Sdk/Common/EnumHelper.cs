namespace Aylos.Xrm.Sdk.Common
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Reflection;

    public static class EnumHelper
    {
        public static string GetEnumDescription(Enum value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes != null && attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            return value.ToString();
        }

        public static T ParseEnum<T>(string value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static IEnumerable<T> EnumToList<T>()
        {
            Type enumType = typeof(T);

            if (enumType.BaseType != typeof(Enum)) throw new ArgumentException("T must be of type System.Enum");

            Array enumValArray = Enum.GetValues(enumType);
            IList<T> enumValList = new List<T>(enumValArray.Length);

            foreach (int val in enumValArray)
            {
                enumValList.Add((T)Enum.Parse(enumType, val.ToString(CultureInfo.InvariantCulture)));
            }

            return enumValList;
        }
    }
}