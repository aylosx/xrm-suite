namespace Aylos.Xrm.Sdk.Common
{
    using Microsoft.Xrm.Sdk;
    using System.Collections.Generic;

    public sealed class EntityReferenceEqualityComparer : IEqualityComparer<EntityReference>
    {
        public bool Equals(EntityReference x, EntityReference y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            else if (x == null || y == null)
            {
                return false;
            }
            else
            {
                return x.GetHashCode() == y.GetHashCode();
            }
        }

        public int GetHashCode(EntityReference obj)
        {
            return obj == null ? 0 : obj.GetHashCode();
        }
    }
}