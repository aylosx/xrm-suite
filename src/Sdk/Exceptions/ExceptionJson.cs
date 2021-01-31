namespace Aylos.Xrm.Sdk.Exceptions
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ExceptionJson
    {
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string Details { get; set; }
        [DataMember]
        public string Source { get; set; }
    }
}