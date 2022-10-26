namespace Aylos.Xrm.Sdk.Plugins
{
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;
    using Microsoft.Xrm.Sdk.Workflow;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;

    public static class TraceHelper
    {
        internal static string Trace(StringBuilder sb)
        {
            return sb.ToString().Length > TraceSectionMaxLength ? 
                sb.ToString().Substring(0, TraceSectionMaxLength - 5) + MoreText + Environment.NewLine : 
                sb.ToString();
        }

        internal static string Trace(ConditionExpression conditionExpression)
        {
            if (conditionExpression == null) return Environment.NewLine;

            var sb = new StringBuilder();
            sb.AppendLine(QeConditionEntityNameText + conditionExpression.EntityName);
            sb.AppendLine(QeConditionAttributeNameText + conditionExpression.AttributeName);
            sb.AppendLine(QeConditionOperatorText + conditionExpression.Operator);
            foreach (object obj in conditionExpression.Values)
            {
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, QeConditionsValueText, obj.GetType().FullName, obj.ToString()));
            }
            return Trace(sb);
        }

        internal static string Trace(ColumnSet columnSet)
        {
            if (columnSet == null) return Environment.NewLine;

            var sb = new StringBuilder();
            foreach (string columnName in columnSet.Columns)
            {
                sb.Append(string.Format(CultureInfo.InvariantCulture, "{0}, ", columnName));
            }
            return Trace(sb);
        }

        internal static string Trace(DataCollection<LinkEntity> linkEntities)
        {
            if (linkEntities == null) return Environment.NewLine;

            var sb = new StringBuilder();
            foreach (LinkEntity linkEntity in linkEntities)
            {
                sb.Append(Trace(linkEntity));
            }
            return Trace(sb);
        }

        internal static string Trace(DataCollection<string> dataCollection)
        {
            if (dataCollection == null) return Environment.NewLine;

            var sb = new StringBuilder();
            foreach (string dataItem in dataCollection)
            {
                sb.Append(string.Format(CultureInfo.InvariantCulture, "{0}, ", dataItem));
            }
            sb.AppendLine(string.Empty);
            return Trace(sb);
        }

        internal static string Trace(Entity entity)
        {
            if (entity == null) return Environment.NewLine;

            var sb = new StringBuilder();
            sb.AppendLine(EntityLogicalNameText + entity.LogicalName);
            sb.AppendLine(EntityEntityIdText + entity.Id);
            sb.AppendLine(EntityAttributesText);
            foreach (KeyValuePair<string, object> parameter in entity.Attributes)
            {
                string value = NullText;

                if (parameter.Value != null)
                {
                    switch (parameter.Value.GetType().ToString().ToLower(CultureInfo.CurrentCulture))
                    {
                        case SdkEntityReferenceText:
                            var er = (EntityReference)parameter.Value;
                            value = (er == null ? NullText : er.Id.ToString());
                            break;

                        case SdkMoneyText:
                            var mn = (Money)parameter.Value;
                            value = (mn == null ? NullText : mn.Value.ToString(CultureInfo.InvariantCulture));
                            break;

                        case SdkOptionSetValueText:
                            var osv = (OptionSetValue)parameter.Value;
                            value = (osv == null ? NullText : osv.Value.ToString(CultureInfo.InvariantCulture));
                            break;

                        case SdkOptionSetValueCollectionText:
                            var osvc = (OptionSetValueCollection)parameter.Value;
                            if (osvc.Count > 0)
                            {
                                value = string.Empty;
                                foreach (var msv in osvc)
                                {
                                    value = string.IsNullOrWhiteSpace(value) ? 
                                        msv.Value.ToString(CultureInfo.InvariantCulture) : 
                                        value + "," + msv.Value.ToString(CultureInfo.InvariantCulture);
                                }
                            }
                            break;

                        default:
                            value = parameter.Value == null ? 
                                NullText : 
                                parameter.Value.ToString().Length > TraceParameterMaxLength ? 
                                    parameter.Value.ToString().Substring(0, TraceParameterMaxLength - 4) + MoreText : 
                                    parameter.Value.ToString();
                            break;
                    }
                }

                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, KeyAndValue1Text, parameter.Key, value));
            }
            return Trace(sb);
        }

        internal static string Trace(EntityCollection entityCollection)
        {
            if (entityCollection == null) return Environment.NewLine;

            var sb = new StringBuilder();
            foreach (Entity entity in entityCollection.Entities)
            {
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, ParameterText, Trace(entity)));
            }
            return Trace(sb);
        }

        internal static string Trace(EntityImageCollection entityImageCollection)
        {
            if (entityImageCollection == null) return Environment.NewLine;

            var sb = new StringBuilder();
            foreach (KeyValuePair<string, Entity> entityImage in entityImageCollection)
            {
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, KeyText, entityImage.Key));
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, ParameterText, Trace(entityImage.Value)));
            }
            return Trace(sb);
        }

        internal static string Trace(EntityReference entityReference)
        {
            if (entityReference == null) return Environment.NewLine;

            var sb = new StringBuilder();
            sb.AppendLine(EntityLogicalNameText + entityReference.LogicalName);
            sb.AppendLine(EntityEntityIdText + entityReference.Id);
            sb.AppendLine(EntityNameText + entityReference.Name);
            return Trace(sb);
        }

        internal static string Trace(EntityReferenceCollection entityReferenceCollection)
        {
            if (entityReferenceCollection == null) return Environment.NewLine;

            var sb = new StringBuilder();
            foreach (EntityReference er in entityReferenceCollection)
            {
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, ParameterText, Trace(er)));
            }
            return Trace(sb);
        }

        internal static string Trace(FilterExpression filterExpression)
        {
            if (filterExpression == null) return Environment.NewLine;

            var sb = new StringBuilder();
            foreach (ConditionExpression conditionExpression in filterExpression.Conditions)
            {
                sb.AppendLine(QeConditionText + Environment.NewLine + Trace(conditionExpression));
            }
            foreach (FilterExpression childFilterExpression in filterExpression.Filters)
            {
                sb.AppendLine(QeCriteriaText + Environment.NewLine + Trace(childFilterExpression));
            }
            return Trace(sb);
        }

        internal static string Trace(FetchExpression fetchExpression)
        {
            if (fetchExpression == null) return Environment.NewLine;

            var sb = new StringBuilder();
            sb.AppendLine(fetchExpression.Query);
            sb.AppendLine(string.Empty);
            return Trace(sb);
        }

        internal static string Trace(LinkEntity linkEntity)
        {
            if (linkEntity == null) return Environment.NewLine;

            var sb = new StringBuilder();
            sb.AppendLine(QeColumnsText + Trace(linkEntity.Columns));
            sb.AppendLine(QeLinkEntityOperatorText + linkEntity.JoinOperator.ToString());
            sb.AppendLine(QeLinkEntityFromAttributeNameText + linkEntity.LinkFromAttributeName);
            sb.AppendLine(QeLinkEntityFromEntityNameText + linkEntity.LinkFromEntityName);
            sb.AppendLine(QeLinkEntityToAttributeNameText + linkEntity.LinkToAttributeName);
            sb.AppendLine(QeLinkEntityToEntityNameText + linkEntity.LinkToEntityName);
            sb.AppendLine(QeCriteriaText + Environment.NewLine + Trace(linkEntity.LinkCriteria));
            sb.AppendLine(QeLinkEntitiesText + Trace(linkEntity.LinkEntities));
            return Trace(sb);
        }

        internal static string Trace(QueryByAttribute queryByAttribute)
        {
            if (queryByAttribute == null) return Environment.NewLine;

            var sb = new StringBuilder();
            sb.AppendLine(QeEntityNameText + queryByAttribute.EntityName);
            sb.AppendLine(QeColumnsText + Trace(queryByAttribute.ColumnSet));
            sb.AppendLine(QeAttributesText + Trace(queryByAttribute.Attributes));
            sb.AppendLine(string.Empty);
            return Trace(sb);
        }

        internal static string Trace(QueryExpression queryExpression)
        {
            if (queryExpression == null) return Environment.NewLine;

            var sb = new StringBuilder();
            sb.AppendLine(QeEntityNameText + queryExpression.EntityName);
            sb.AppendLine(QeColumnsText + Trace(queryExpression.ColumnSet));
            sb.AppendLine(QeCriteriaText + Environment.NewLine + Trace(queryExpression.Criteria));
            sb.AppendLine(QeLinkEntitiesText + Trace(queryExpression.LinkEntities));
            return Trace(sb);
        }

        internal static string Trace(QueryBase queryBase)
        {
            if (queryBase == null) return Environment.NewLine;

            if (queryBase is QueryExpression)
            {
                return Trace((QueryExpression)queryBase);
            }
            else if (queryBase is FetchExpression)
            {
                return Trace((FetchExpression)queryBase);
            }
            else if (queryBase is QueryByAttribute)
            {
                return Trace((QueryByAttribute)queryBase);
            }
            else
            {
                return queryBase.GetType().Name;
            }
        }

        internal static string Trace(ParameterCollection parameterCollection)
        {
            if (parameterCollection == null) return Environment.NewLine;

            var sb = new StringBuilder();
            foreach (KeyValuePair<string, object> parameter in parameterCollection)
            {
                if (parameter.Value != null)
                {
                    if (parameter.Value.GetType() == typeof(Entity))
                    {
                        var entity = (Entity)parameter.Value;
                        sb.AppendLine(string.Format(CultureInfo.InvariantCulture, KeyText, parameter.Key));
                        sb.AppendLine(string.Format(CultureInfo.InvariantCulture, ParameterText, Trace(entity)));
                    }
                    else if (parameter.Value.GetType() == typeof(EntityCollection))
                    {
                        var ec = (EntityCollection)parameter.Value;
                        sb.AppendLine(string.Format(CultureInfo.InvariantCulture, KeyText, parameter.Key));
                        sb.AppendLine(string.Format(CultureInfo.InvariantCulture, ParameterText, Trace(ec)));
                    }
                    else if (parameter.Value.GetType() == typeof(EntityReference))
                    {
                        var er = (EntityReference)parameter.Value;
                        sb.AppendLine(string.Format(CultureInfo.InvariantCulture, KeyText, parameter.Key));
                        sb.AppendLine(string.Format(CultureInfo.InvariantCulture, ParameterText, Trace(er)));
                    }
                    else if (parameter.Value.GetType() == typeof(EntityReferenceCollection))
                    {
                        var erc = (EntityReferenceCollection)parameter.Value;
                        sb.AppendLine(string.Format(CultureInfo.InvariantCulture, KeyText, parameter.Key));
                        sb.AppendLine(string.Format(CultureInfo.InvariantCulture, ParameterText, Trace(erc)));
                    }
                    else if (parameter.Value.GetType() == typeof(RelationshipQueryCollection))
                    {
                        var rqc = (RelationshipQueryCollection)parameter.Value;
                        sb.AppendLine(string.Format(CultureInfo.InvariantCulture, KeyText, parameter.Key));
                        sb.AppendLine(string.Format(CultureInfo.InvariantCulture, ParameterText, Trace(rqc)));
                    }
                    else if (parameter.Value.GetType() == typeof(QueryExpression))
                    {
                        var qe = (QueryExpression)parameter.Value;
                        sb.AppendLine(string.Format(CultureInfo.InvariantCulture, KeyText, parameter.Key));
                        sb.AppendLine(string.Format(CultureInfo.InvariantCulture, ParameterText, Trace(qe)));
                    }
                    else if (parameter.Value.GetType() == typeof(QueryByAttribute))
                    {
                        var qba = (QueryByAttribute)parameter.Value;
                        sb.AppendLine(string.Format(CultureInfo.InvariantCulture, KeyText, parameter.Key));
                        sb.AppendLine(string.Format(CultureInfo.InvariantCulture, ParameterText, Trace(qba)));
                    }
                    else if (parameter.Value.GetType() == typeof(FetchExpression))
                    {
                        var fe = (FetchExpression)parameter.Value;
                        sb.AppendLine(string.Format(CultureInfo.InvariantCulture, KeyText, parameter.Key));
                        sb.AppendLine(string.Format(CultureInfo.InvariantCulture, ParameterText, Trace(fe)));
                    }
                    else if (parameter.Value.GetType() == typeof(ColumnSet))
                    {
                        var cs = (ColumnSet)parameter.Value;
                        sb.AppendLine(string.Format(CultureInfo.InvariantCulture, KeyText, parameter.Key));
                        sb.AppendLine(string.Format(CultureInfo.InvariantCulture, ParameterText, Trace(cs)));
                    }
                    else if (parameter.Value.GetType() == typeof(OptionSetValue))
                    {
                        var osv = (OptionSetValue)parameter.Value;
                        sb.AppendLine(string.Format(CultureInfo.InvariantCulture, KeyAndValue2Text, parameter.Key, osv.Value));
                    }
                    else
                    {
                        string value =
                            parameter.Value == null ? NullText :
                            parameter.Key.ToUpperInvariant().Contains("USERNAME") || parameter.Key.ToUpperInvariant().Contains("PASSWORD") ? 
                            new string('*', parameter.Value.ToString().Length) : 
                            parameter.Value.ToString(); 
                        sb.AppendLine(string.Format(CultureInfo.CurrentCulture, KeyAndValue2Text, parameter.Key, value));
                    }
                }
            }
            return Trace(sb);
        }

        internal static string Trace(Relationship relationship)
        {
            if (relationship == null) return Environment.NewLine;

            var sb = new StringBuilder();
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "PrimaryEntityRole: {0}", relationship.PrimaryEntityRole));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "SchemaName: {0}", relationship.SchemaName));
            sb.AppendLine(string.Empty);
            return Trace(sb);
        }

        internal static string Trace(RelationshipQueryCollection relationshipQueryCollection)
        {
            if (relationshipQueryCollection == null) return Environment.NewLine;

            var sb = new StringBuilder();
            foreach (KeyValuePair<Relationship, QueryBase> keyValuePair in relationshipQueryCollection)
            {
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, ParameterText, Trace(keyValuePair.Key)));
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, ParameterText, Trace(keyValuePair.Value)));
            }
            return Trace(sb);
        }

        public static string Trace(IPluginExecutionContext executionContext)
        {
            if (executionContext == null) return Environment.NewLine;

            var sb = new StringBuilder();
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, BusinessUnitIdText, executionContext.BusinessUnitId));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, CorrelationIdText, executionContext.CorrelationId));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, DepthText, executionContext.Depth));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, InitiatingUserIdText, executionContext.InitiatingUserId));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, IsExecutingOfflineText, executionContext.IsExecutingOffline));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, IsInTransactionText, executionContext.IsInTransaction));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, IsOfflinePlaybackText, executionContext.IsOfflinePlayback));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, IsolationModeText, executionContext.IsolationMode));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, MessageNameText, executionContext.MessageName));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, ModeText, executionContext.Mode));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, OperationCreatedOnText, executionContext.OperationCreatedOn.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, OperationIdText, executionContext.OperationId));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, OrganizationIdText, executionContext.OrganizationId));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, OrganizationNameText, executionContext.OrganizationName));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, OwningExtensionLogicalNameText, executionContext.OwningExtension.LogicalName));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, OwningExtensionNameText, executionContext.OwningExtension.Name));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, OwningExtensionIdText, executionContext.OwningExtension.Id));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, PrimaryEntityIdText, executionContext.PrimaryEntityId));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, PrimaryEntityNameText, executionContext.PrimaryEntityName));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, RequestIdText, (executionContext.RequestId.HasValue ? executionContext.RequestId.Value.ToString() : NullText)));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, SecondaryEntityNameText, executionContext.SecondaryEntityName));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, StageText, executionContext.Stage));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, UserIdText, executionContext.UserId));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, SharedVariablesText, Environment.NewLine + Trace(executionContext.SharedVariables)));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, InputParametersText, Environment.NewLine + Trace(executionContext.InputParameters)));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, OutputParametersText, Environment.NewLine + Trace(executionContext.OutputParameters)));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, PreEntityImagesText, Environment.NewLine + Trace(executionContext.PreEntityImages)));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, PostEntityImagesText, Environment.NewLine + Trace(executionContext.PostEntityImages)));
            if (executionContext.ParentContext != null)
            {
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, ParentContextText, Environment.NewLine + Trace(executionContext.ParentContext)));
            }
            return sb.ToString().Length > TraceMessageMaxLength ? sb.ToString().Substring(0, TraceMessageMaxLength - 4) + MoreText : sb.ToString();
        }

        public static string Trace(RemoteExecutionContext executionContext)
        {
            if (executionContext == null) return Environment.NewLine;

            var sb = new StringBuilder();
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, BusinessUnitIdText, executionContext.BusinessUnitId));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, CorrelationIdText, executionContext.CorrelationId));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, DepthText, executionContext.Depth));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, InitiatingUserIdText, executionContext.InitiatingUserId));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, IsExecutingOfflineText, executionContext.IsExecutingOffline));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, IsInTransactionText, executionContext.IsInTransaction));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, IsOfflinePlaybackText, executionContext.IsOfflinePlayback));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, IsolationModeText, executionContext.IsolationMode));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, MessageNameText, executionContext.MessageName));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, ModeText, executionContext.Mode));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, OperationCreatedOnText, executionContext.OperationCreatedOn.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, OperationIdText, executionContext.OperationId));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, OrganizationIdText, executionContext.OrganizationId));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, OrganizationNameText, executionContext.OrganizationName));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, OwningExtensionLogicalNameText, executionContext.OwningExtension.LogicalName));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, OwningExtensionNameText, executionContext.OwningExtension.Name));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, OwningExtensionIdText, executionContext.OwningExtension.Id));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, PrimaryEntityIdText, executionContext.PrimaryEntityId));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, PrimaryEntityNameText, executionContext.PrimaryEntityName));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, RequestIdText, (executionContext.RequestId.HasValue ? executionContext.RequestId.Value.ToString() : NullText)));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, SecondaryEntityNameText, executionContext.SecondaryEntityName));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, StageText, executionContext.Stage));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, UserIdText, executionContext.UserId));

            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, PostEntityImagesText, Environment.NewLine + executionContext.InitiatingUserAzureActiveDirectoryObjectId));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, PostEntityImagesText, Environment.NewLine + executionContext.UserAzureActiveDirectoryObjectId));

            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, SharedVariablesText, Environment.NewLine + Trace(executionContext.SharedVariables)));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, InputParametersText, Environment.NewLine + Trace(executionContext.InputParameters)));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, OutputParametersText, Environment.NewLine + Trace(executionContext.OutputParameters)));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, PreEntityImagesText, Environment.NewLine + Trace(executionContext.PreEntityImages)));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, PostEntityImagesText, Environment.NewLine + Trace(executionContext.PostEntityImages)));
            if (executionContext.ParentContext != null)
            {
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, ParentContextText, Environment.NewLine + Trace(executionContext.ParentContext)));
            }
            return sb.ToString().Length > TraceMessageMaxLength ? sb.ToString().Substring(0, TraceMessageMaxLength - 4) + MoreText : sb.ToString();
        }

        private const int TraceParameterMaxLength = 200;
        private const int TraceSectionMaxLength = 1500;
        private const int TraceMessageMaxLength = 9000;
        private const string NullText = "NULL";
        private const string QeEntityNameText = "-- Entity: ";
        private const string QeAttributesText = "-- Attributes ";
        private const string QeColumnsText = "-- Columns: ";
        private const string QeCriteriaText = "-- Criteria ";
        private const string QeConditionText = "---- Condition ";
        private const string QeConditionEntityNameText = "------ Entity: ";
        private const string QeConditionAttributeNameText = "------ Attribute: ";
        private const string QeConditionOperatorText = "------ Operator: ";
        private const string QeConditionsValueText = "------ {0}: {1}";
        private const string QeLinkEntitiesText = "-- Link Entities ";
        private const string QeLinkEntityFromAttributeNameText = "------ From Attribute: ";
        private const string QeLinkEntityFromEntityNameText = "------ From Entity: ";
        private const string QeLinkEntityToAttributeNameText = "------ To Attribute: ";
        private const string QeLinkEntityToEntityNameText = "------ To Entity: ";
        private const string QeLinkEntityOperatorText = "------ Operator: ";
        private const string EntityLogicalNameText = "-- LogicalName: ";
        private const string EntityEntityIdText = "-- EntityId: ";
        private const string EntityNameText = "-- Name: ";
        private const string EntityAttributesText = "-- Attributes ";
        private const string KeyAndValue1Text = "---- {0}: {1}";
        private const string KeyAndValue2Text = "-- {0}: {1}";
        private const string KeyText = "-- {0}";
        private const string ParameterText = "{0}";
        private const string SdkEntityReferenceText = "microsoft.xrm.sdk.entityreference";
        private const string SdkMoneyText = "microsoft.xrm.sdk.money";
        private const string SdkOptionSetValueText = "microsoft.xrm.sdk.optionsetvalue";
        private const string SdkOptionSetValueCollectionText = "microsoft.xrm.sdk.optionsetvaluecollection";
        private const string UserIdText = "UserId: {0}";
        private const string OrganizationIdText = "OrganizationId: {0}";
        private const string OrganizationNameText = "OrganizationName: {0}";
        private const string MessageNameText = "MessageName: {0}";
        private const string StageText = "Stage: {0}";
        private const string ModeText = "Mode: {0}";
        private const string PrimaryEntityNameText = "PrimaryEntityName: {0}";
        private const string SecondaryEntityNameText = "SecondaryEntityName: {0}";
        private const string BusinessUnitIdText = "BusinessUnitId: {0}";
        private const string CorrelationIdText = "CorrelationId: {0}";
        private const string DepthText = "Depth: {0}";
        private const string InitiatingUserIdText = "InitiatingUserId: {0}";
        private const string IsExecutingOfflineText = "IsExecutingOffline: {0}";
        private const string IsInTransactionText = "IsInTransaction: {0}";
        private const string IsOfflinePlaybackText = "IsOfflinePlayback: {0}";
        private const string IsolationModeText = "IsolationMode: {0}";
        private const string OperationCreatedOnText = "OperationCreatedOn: {0}";
        private const string OperationIdText = "OperationId: {0}";
        private const string PrimaryEntityIdText = "PrimaryEntityId: {0}";
        private const string OwningExtensionLogicalNameText = "OwningExtension LogicalName: {0}";
        private const string OwningExtensionNameText = "OwningExtension Name: {0}";
        private const string OwningExtensionIdText = "OwningExtension Id: {0}";
        private const string RequestIdText = "Request Id: {0}";
        private const string SharedVariablesText = "SharedVariables {0}";
        private const string InputParametersText = "InputParameters {0}";
        private const string OutputParametersText = "OutputParameters {0}";
        private const string PreEntityImagesText = "PreEntityImages {0}";
        private const string PostEntityImagesText = "PostEntityImages {0}";
        private const string ParentContextText = "Parent Context\n============== {0}";
        private const string MoreText = " ...";
    }
}
