namespace Aylos.Xrm.Sdk.Common
{
    using System.Runtime.Serialization;

    [DataContract]
    public class AzureFile
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public int Size { get; set; }
        [DataMember]
        public string Url { get; set; }
    }
}
