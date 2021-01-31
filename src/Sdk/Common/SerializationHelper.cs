namespace Aylos.Xrm.Sdk.Common
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;
    using System.Text;

    public static class SerializationHelper
    {
        public const string DateTimeFormatText = "yyyy-MM-ddTHH:mm:ssZ";

        public static T DeserializeXml<T>(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) throw new ArgumentNullException(nameof(input));

            T output;
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                ms.Position = 0;
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                output = (T)serializer.ReadObject(ms);
            }
            return output;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public static string SerializeXml<T>(T input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            string output;
            using (var ms = new MemoryStream())
            using (var sr = new StreamReader(ms))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(ms, input);
                ms.Position = 0;
                output = sr.ReadToEnd();
                sr.Close();
                ms.Close();
            }
            return output;
        }

        public static T DeserializeJson<T>(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) throw new ArgumentNullException(nameof(input));
            DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings
            {
                DateTimeFormat = new DateTimeFormat(DateTimeFormatText, CultureInfo.InvariantCulture)
            };
            return DeserializeJson<T>(input, settings);
        }

        public static string SerializeJson<T>(T input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings
            {
                DateTimeFormat = new DateTimeFormat(DateTimeFormatText, CultureInfo.InvariantCulture)
            };
            return SerializeJson<T>(input, settings);
        }

        public static T DeserializeJson<T>(string input, DataContractJsonSerializerSettings settings)
        {
            if (string.IsNullOrWhiteSpace(input)) throw new ArgumentNullException(nameof(input));
            if (settings == null) throw new ArgumentNullException(nameof(settings));

            T output;
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                ms.Position = 0;
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T), settings);
                output = (T)serializer.ReadObject(ms);
            }
            return output;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public static string SerializeJson<T>(T input, DataContractJsonSerializerSettings settings)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (settings == null) throw new ArgumentNullException(nameof(settings));

            string output;
            using (var ms = new MemoryStream())
            using (var sr = new StreamReader(ms))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T), settings);
                serializer.WriteObject(ms, input);
                ms.Position = 0;
                output = sr.ReadToEnd();
                sr.Close();
                ms.Close();
            }
            return output;
        }
    }
}
