namespace Aylos.Xrm.Sdk.CrmSvcUtilExtensions
{
    using Microsoft.Xrm.Sdk.Metadata;
    using System.Collections.Generic;

    internal class EntityInfo
    {
        internal string EntityLogicalName { get; set; }
        internal EntityMetadata EntityMetadata { get; set; }
        internal IList<string> AttributeNames { get; set; }

        internal EntityInfo()
        {
            AttributeNames = new List<string>();
        }
    }
}
