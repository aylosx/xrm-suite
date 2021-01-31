namespace Aylos.Xrm.Sdk.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;
    using Microsoft.Xrm.Sdk;

    public static class EntityHelper
    {
        public const string SystemAttributes =
            "createdby,createdbyname,createdbyyominame,createdon,createdonbehalfby,createdonbehalfbyname,createdonbehalfbyyominame," +
            "importsequencenumber,modifiedby,modifiedbyname,modifiedbyyominame,modifiedon,modifiedonbehalfby,modifiedonbehalfbyname," +
            "modifiedonbehalfbyyominame,overriddencreatedon,ownerid,owneridname,owneridtype,owneridyominame,owningbusinessunit," +
            "owningteam,owninguser,statecode,statecodename,statuscode,statuscodename,timezoneruleversionnumber," +
            "utcconversiontimezonecode,versionnumber";

        public static Entity CopyEntity(Entity entity, string excludedAttributes)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (string.IsNullOrWhiteSpace(excludedAttributes)) throw new ArgumentNullException(nameof(excludedAttributes));

            string[] customAttributesExclusionArray = excludedAttributes
                .ToLowerInvariant()
                .Replace(" ", string.Empty)
                .Split(",".ToCharArray());
            List<string> customAttributesExclusionList = new List<string>(customAttributesExclusionArray);

            return CopyEntity(entity, customAttributesExclusionList);
        }

        public static Entity CopyEntity(Entity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            return CopyEntity(entity, new List<string>());
        }

        public static Entity CopyEntity(Entity entity, IList<string> excludedAttributes)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (excludedAttributes == null) throw new ArgumentNullException(nameof(excludedAttributes));

            var systemAttributes = new List<string> (SystemAttributes.Split(",".ToCharArray()));
            Entity output = new Entity(entity.LogicalName);
            foreach (KeyValuePair<string, object> attribute in entity.Attributes)
            {
                if (attribute.Value == null) continue; // NULLs are not required
                if (attribute.Value is Guid?) continue; // Primary keys are not required
                if (systemAttributes.Contains(attribute.Key)) continue; // System attributes are not required
                if (excludedAttributes.Contains(attribute.Key)) continue; // Excluded attributes are not required
                output.Attributes.Add(attribute);
            }

            return output;
        }

#pragma warning disable IDE0060 // Remove unused parameter
        public static string GetAttributeLogicalName<TEntity>(this TEntity entity, string propertyName)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            var epi = typeof(TEntity).GetProperty(propertyName);
            if (epi == null)
            {
                throw new ArgumentNullException(propertyName, "TargetEntity has no such property.");
            }

            var attr = (AttributeLogicalNameAttribute)epi.GetCustomAttribute(typeof(AttributeLogicalNameAttribute));
            if (attr == null)
            {
                throw new ArgumentNullException(propertyName, "AttributeLogicalNameAttribute not found on property.");
            }

            return attr.LogicalName;
        }

#pragma warning disable IDE0060 // Remove unused parameter
        public static string GetAttributeLogicalName<TEntity, TProperty>(this TEntity entity, Expression<Func<TEntity, TProperty>> expression)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            MemberExpression member = expression.Body as MemberExpression;
            if (member != null && member.Member is PropertyInfo)
            {
                var pi = member.Member as PropertyInfo;
                var pn = pi.Name;
                var epi = typeof(TEntity).GetProperty(pn);
                if (epi == null)
                {
                    throw new ArgumentNullException(pn, "TargetEntity has no such property.");
                }

                var attr = (AttributeLogicalNameAttribute)epi.GetCustomAttribute(typeof(AttributeLogicalNameAttribute));
                if (attr == null)
                {
                    throw new ArgumentNullException(pn, "AttributeLogicalNameAttribute not found on property.");
                }

                return attr.LogicalName;
            }

            throw new ArgumentException("Expression is not a Property.");
        }
		
        public static bool AreEqual(EntityReference x, EntityReference y)
        {
            var comparer = new EntityReferenceEqualityComparer();
            return comparer.Equals(x, y);
        }
    }
}
