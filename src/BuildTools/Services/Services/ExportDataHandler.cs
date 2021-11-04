using Microsoft.PowerPlatform.Tooling.BatchedTelemetry;
using Microsoft.PowerPlatform.Tooling.Crm.Telemetry;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using Microsoft.Xrm.Tooling.Dmt.DataMigCommon.DataInteraction;
using Microsoft.Xrm.Tooling.Dmt.DataMigCommon.DataModel.Data;
using Microsoft.Xrm.Tooling.Dmt.DataMigCommon.DataModel.Schema;
using Microsoft.Xrm.Tooling.Dmt.DataMigCommon.Utility;
using Microsoft.Xrm.Tooling.Dmt.ExportProcessor.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Web;

namespace Microsoft.Xrm.Tooling.Dmt.ExportProcessor.DataInteraction
{
    /// <summary>
    /// Reverse engineered code from the data migration utility namespace. 
    /// I had to do that because the class didn't provide public members. 
    /// Additionally, added the CRM service client to support CRM connectivity.
    /// Working in my own tool that would help with import/export functionality.
    /// </summary>
    public class ExportDataHandler
    {
        private CrmServiceClient CrmServiceClient { get; set; }

        private List<DataMigCommon.DataModel.Data.entitiesEntity> _exportEntitiesDataObject;

        private DataMigCommon.DataModel.Schema.entities _schemaMap;

        private static bool _addProductIdFilters;

#pragma warning disable IDE0044 // Add readonly modifier
        private static List<string> _productTaxonomyEntities;
#pragma warning restore IDE0044 // Add readonly modifier

#pragma warning disable IDE0044 // Add readonly modifier
        private TraceLogger _logger = Helper.CreateTraceLogger("DataMigrationUtility.Export");
#pragma warning restore IDE0044 // Add readonly modifier

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
        static ExportDataHandler()
        {
            _addProductIdFilters = false;
            _productTaxonomyEntities = new List<string>()
            {
                "product",
                "dynamicproperty",
                "dynamicpropertyassociation",
                "dynamicpropertyoptionsetitem"
            };
        }

        public ExportDataHandler(CrmServiceClient client)
        {
#pragma warning disable IDE0016 // Use 'throw' expression
            if (client == null) throw new ArgumentNullException(nameof(client));
#pragma warning restore IDE0016 // Use 'throw' expression
            CrmServiceClient = client;
        }

        private ProgressItemEventArgs AddProgressItem(string text, ProgressItemStatus status = 0)
        {
            ProgressItemEventArgs progressItemEventArg = new ProgressItemEventArgs()
            {
                progressItem = new StatusEventUpdate()
                {
                    ItemStatus = status,
                    ItemText = text
                }
            };
            if (AddNewProgressItem != null)
            {
#pragma warning disable IDE1005 // Delegate invocation can be simplified.
                AddNewProgressItem(this, progressItemEventArg);
#pragma warning restore IDE1005 // Delegate invocation can be simplified.
            }
            return progressItemEventArg;
        }

#pragma warning disable IDE0060 // Remove unused parameter
#pragma warning disable IDE0051 // Remove unused private members
        private string ConstructDataStructure(DataMigCommon.DataModel.Schema.entitiesEntityRelationship relationship, Dictionary<string, Dictionary<string, object>> results)
#pragma warning restore IDE0051 // Remove unused private members
#pragma warning restore IDE0060 // Remove unused parameter
        {
            return null;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Microsoft.Xrm.Tooling.Dmt.DataMigCommon.Utility.TraceLogger.Log(System.String,System.Diagnostics.TraceEventType)")]
        private DataMigCommon.DataModel.Data.entitiesEntity ConstructExportDataStructure(DataMigCommon.DataModel.Schema.entitiesEntity entity, Dictionary<string, Dictionary<string, object>> results)
        {
            entitiesEntityRecordFieldActivitypointerrecords[] array;
            DataMigCommon.DataModel.Data.entitiesEntity _entitiesEntity = new DataMigCommon.DataModel.Data.entitiesEntity()
            {
                displayname = entity.displayname,
                name = entity.name
            };
            _logger.Log("ConstructDataStructure", TraceEventType.Start);
            if (results == null || results.Values == null)
            {
                _logger.Log(string.Format(CultureInfo.InvariantCulture, "No records to parse for {0}", entity.displayname));
                return null;
            }
            _logger.Log(string.Format(CultureInfo.InvariantCulture, "Processing {0} rows for {1} to export stream", entity.displayname, results.Count));
            List<entitiesEntityRecord> entitiesEntityRecords = new List<entitiesEntityRecord>();
            List<entitiesEntityM2mrelationship> entitiesEntityM2mrelationships = new List<entitiesEntityM2mrelationship>();
            foreach (Dictionary<string, object> value in results.Values)
            {
                Guid empty = Guid.Empty;
                List<entitiesEntityRecordField> entitiesEntityRecordFields = new List<entitiesEntityRecordField>();
#pragma warning disable IDE0038 // Use pattern matching
                if (value.ContainsKey(entity.primaryidfield) && value[entity.primaryidfield] != null && value[entity.primaryidfield] is Guid)
#pragma warning restore IDE0038 // Use pattern matching
                {
                    empty = (Guid)value[entity.primaryidfield];
                }
                if (entity.fields != null && entity.fields.Length != 0)
                {
                    entitiesEntityField[] entitiesEntityFieldArray = entity.fields;
                    for (int i = 0; i < (int)entitiesEntityFieldArray.Length; i++)
                    {
                        entitiesEntityField _entitiesEntityField = entitiesEntityFieldArray[i];
                        string str = string.Empty;
                        string empty1 = string.Empty;
                        object dataItemFromResult = GetDataItemFromResult(value, _entitiesEntityField.name, _entitiesEntityField.type, out str, out empty1);
#pragma warning disable IDE0038 // Use pattern matching
                        if (dataItemFromResult != null && (!(dataItemFromResult is Guid) || !((Guid)dataItemFromResult == Guid.Empty)))
#pragma warning restore IDE0038 // Use pattern matching
                        {
                            List<entitiesEntityRecordFieldActivitypointerrecords> _entitiesEntityRecordFieldActivitypointerrecords = null;
#pragma warning disable IDE0038 // Use pattern matching
                            if (dataItemFromResult is List<entitiesEntityRecordFieldActivitypointerrecords>)
#pragma warning restore IDE0038 // Use pattern matching
                            {
                                _entitiesEntityRecordFieldActivitypointerrecords = (List<entitiesEntityRecordFieldActivitypointerrecords>)dataItemFromResult;
                                dataItemFromResult = string.Empty;
                            }
#pragma warning disable IDE0038 // Use pattern matching
                            else if (!(dataItemFromResult is DateTime))
#pragma warning restore IDE0038 // Use pattern matching
                            {
                                string lowerInvariant = _entitiesEntityField.type.ToLowerInvariant();
                                dataItemFromResult = (lowerInvariant == "number" || lowerInvariant == "decimal" || lowerInvariant == "float" ? Convert.ToString(dataItemFromResult, CultureInfo.InvariantCulture) : HttpUtility.HtmlEncode(dataItemFromResult));
                            }
                            else
                            {
                                dataItemFromResult = ((DateTime)dataItemFromResult).ToString("o");
                            }
                            List<entitiesEntityRecordField> entitiesEntityRecordFields1 = entitiesEntityRecordFields;
                            entitiesEntityRecordField _entitiesEntityRecordField = new entitiesEntityRecordField()
                            {
                                name = _entitiesEntityField.name,
                                @value = (string)dataItemFromResult,
                                lookupentity = str,
                                lookupentityname = empty1
                            };
                            if (_entitiesEntityRecordFieldActivitypointerrecords != null)
                            {
                                array = _entitiesEntityRecordFieldActivitypointerrecords.ToArray();
                            }
                            else
                            {
                                array = null;
                            }
                            _entitiesEntityRecordField.activitypointerrecords = array;
                            entitiesEntityRecordFields1.Add(_entitiesEntityRecordField);
                        }
                    }
                }
                entitiesEntityRecords.Add(new entitiesEntityRecord()
                {
                    field = entitiesEntityRecordFields.ToArray(),
                    id = empty.ToString()
                });
                if (entity.relationships == null || entity.relationships.Length == 0)
                {
                    continue;
                }
                foreach (DataMigCommon.DataModel.Schema.entitiesEntityRelationship _entitiesEntityRelationship in
                    from w in (IEnumerable<DataMigCommon.DataModel.Schema.entitiesEntityRelationship>)entity.relationships
                    where w.manyToMany
                    select w)
                {
                    entitiesEntityM2mrelationship manyRelationshipData = FetchManyToManyRelationshipData(empty, entity, _entitiesEntityRelationship);
                    if (manyRelationshipData == null)
                    {
                        continue;
                    }
                    entitiesEntityM2mrelationships.Add(manyRelationshipData);
                }
            }
            _entitiesEntity.records = entitiesEntityRecords.ToArray();
            _entitiesEntity.m2mrelationships = entitiesEntityM2mrelationships.ToArray();
            _logger.Log("ConstructDataStructure", TraceEventType.Stop);
            return _entitiesEntity;
        }

        internal bool ExportData(string schemaFileName, string outputFileName, IAppTelemetryClient telemetry)
        {
            bool flag;
            using (StartStopEventsTracker nullable = telemetry.StartTrackingWorkload("export", new Guid?(CrmServiceClient.ConnectedOrgId), null))
            {
                bool flag1 = this.ExportData(schemaFileName, outputFileName);
                nullable.Success = new bool?(flag1);
                flag = flag1;
            }
            return flag;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Microsoft.Xrm.Tooling.Dmt.DataMigCommon.Utility.TraceLogger.Log(System.String)")]
        internal bool ExportData(string schemaFileName, string outputFileName)
        {
            TimeSpan timeSpan;
            _logger.Log("ExportData", TraceEventType.Start);
            _logger.Log("*********************************************************");
            _logger.Log("** BEGIN EXPORT PROCESS **");
            _logger.Log(string.Format(CultureInfo.InvariantCulture, "** Log Start: {0}", DateTime.Now));
            DateTime utcNow = DateTime.UtcNow;
            if (string.IsNullOrEmpty(schemaFileName) || string.IsNullOrEmpty(outputFileName))
            {
                ArgumentException argumentException = new ArgumentException(Resources.SCHEMA_FILE_OUTPUT_FILE_REQUIRED);
                _logger.Log(argumentException);
                _logger.Log("*********************************************************");
                _logger.Log("** EXIT EXPORT PROCESS FAIL **");
                _logger.Log("*********************************************************");
                _logger.Log("ExportData", TraceEventType.Stop);
                throw argumentException;
            }
            if (!ReadDataFromCrm(schemaFileName))
            {
                _logger.Log("No Data Found to export", TraceEventType.Information);
                _logger.Log("*********************************************************");
                _logger.Log("** EXIT EXPORT PROCESS FAIL **");
                _logger.Log("*********************************************************");
                _logger.Log("ExportData", TraceEventType.Stop);
                CultureInfo currentUICulture = CultureInfo.CurrentUICulture;
                string fAILEDSCHEMAVALIDATIONMISSINGENTITIES = Resources.FAILED_SCHEMA_VALIDATION_MISSING_ENTITIES;
                timeSpan = utcNow.Subtract(DateTime.UtcNow);
                timeSpan = timeSpan.Duration();
                AddProgressItem(string.Format(currentUICulture, fAILEDSCHEMAVALIDATIONMISSINGENTITIES, timeSpan.ToString()), ProgressItemStatus.Failed);
                return false;
            }
            bool disk = false;
            if (_exportEntitiesDataObject != null)
            {
                disk = SaveOutputToDisk(schemaFileName, outputFileName);
            }
            if (!disk)
            {
                CultureInfo cultureInfo = CultureInfo.CurrentUICulture;
                string eXPORTCRMFAILEDCHECKLOGEXPORTDURATION = Resources.EXPORT_CRM_FAILED_CHECK_LOG_EXPORT_DURATION;
                timeSpan = utcNow.Subtract(DateTime.UtcNow);
                timeSpan = timeSpan.Duration();
                AddProgressItem(string.Format(cultureInfo, eXPORTCRMFAILEDCHECKLOGEXPORTDURATION, timeSpan.ToString()), ProgressItemStatus.Failed);
                _logger.Log("*********************************************************");
                _logger.Log("Error : No Valid Data Found to export", TraceEventType.Information);
                _logger.Log("*********************************************************");
                _logger.Log("** EXIT EXPORT PROCESS FAIL **");
                _logger.Log("*********************************************************");
                _logger.Log("ExportData", TraceEventType.Stop);
            }
            else
            {
                CultureInfo currentUICulture1 = CultureInfo.CurrentUICulture;
                string eXPORTCRMCOMPLETEEXPORTEDENTITIES = Resources.EXPORT_CRM_COMPLETE_EXPORTED_ENTITIES;
                object obj = _exportEntitiesDataObject.Count<DataMigCommon.DataModel.Data.entitiesEntity>();
                timeSpan = utcNow.Subtract(DateTime.UtcNow);
                timeSpan = timeSpan.Duration();
                AddProgressItem(string.Format(currentUICulture1, eXPORTCRMCOMPLETEEXPORTEDENTITIES, obj, timeSpan.ToString()), ProgressItemStatus.Complete);
                _logger.Log("*********************************************************");
                _logger.Log("Data Exported Successfully", TraceEventType.Information);
                _logger.Log("*********************************************************");
                _logger.Log("ExportData", TraceEventType.Stop);
            }
            return disk;
        }

        private void FetchDataFromCrm(DataMigCommon.DataModel.Schema.entitiesEntity entity, ProgressItemEventArgs progressArgs)
        {
            List<string> strs = new List<string>();
            if (entity.fields != null && entity.fields.Length != 0)
            {
                strs = (
                    from s in (IEnumerable<entitiesEntityField>)entity.fields
                    select s.name).ToList<string>();
            }
            Dictionary<string, Dictionary<string, object>> strs1 = new Dictionary<string, Dictionary<string, object>>();
            string empty = string.Empty;
            bool flag = true;
            int num = 500;
            int num1 = 1;
            string itemText = progressArgs.progressItem.ItemText;
            UpdateProgress(progressArgs, string.Format(CultureInfo.CurrentUICulture, Resources.READING_RECORDS, itemText), ProgressItemStatus.Working);
            while (flag)
            {
                Dictionary<string, Dictionary<string, object>> strs2 = PageEnabledFetchFromCRM(entity.name, strs, entity.filter, ref empty, ref flag, num, num1, CrmServiceClient);
                if (strs2 != null && strs2.Count > 0)
                {
                    foreach (KeyValuePair<string, Dictionary<string, object>> keyValuePair in strs2)
                    {
                        strs1.Add(keyValuePair.Key, keyValuePair.Value);
                    }
                }
                if (!flag)
                {
                    UpdateProgress(progressArgs, string.Format(CultureInfo.CurrentUICulture, Resources.READING_RECORDS_READ_COMPLETE, itemText, strs1.Count<KeyValuePair<string, Dictionary<string, object>>>()), ProgressItemStatus.Working);
                }
                else
                {
                    num1++;
                    UpdateProgress(progressArgs, string.Format(CultureInfo.CurrentUICulture, Resources.READING_RECORDS_FOUND_RECORDS_REQUESTING_NEXT, itemText, strs1.Count<KeyValuePair<string, Dictionary<string, object>>>(), num), ProgressItemStatus.Working);
                }
            }
            if (strs1 == null || strs1.Count == 0)
            {
                _logger.Log(string.Format(CultureInfo.InvariantCulture, "No Results found from entity {0}", entity.name));
                if (string.IsNullOrWhiteSpace(CrmServiceClient.LastCrmError))
                {
                    UpdateProgress(progressArgs, string.Format(CultureInfo.CurrentUICulture, Resources.NO_DATA_FOUND_ENTITY, entity.displayname), ProgressItemStatus.Complete);
                    return;
                }
                UpdateProgress(progressArgs, string.Format(CultureInfo.CurrentUICulture, Resources.ERROR_REQUESTING_DATA, entity.displayname, CrmServiceClient.LastCrmError), ProgressItemStatus.Failed);
                _logger.Log(string.Format(CultureInfo.InvariantCulture, "CRM Retrieve Error: {0}", CrmServiceClient.LastCrmError), TraceEventType.Error, CrmServiceClient.LastCrmException);
                return;
            }
            _logger.Log(string.Format(CultureInfo.InvariantCulture, "Found {0} rows for {1} entity", strs1.Count, entity.name));
            if (_exportEntitiesDataObject == null)
            {
                _exportEntitiesDataObject = new List<DataMigCommon.DataModel.Data.entitiesEntity>();
            }
            UpdateProgress(progressArgs, string.Format(CultureInfo.CurrentUICulture, Resources.ADDING_RECORDS_EXPORT_SET, itemText, strs1.Count<KeyValuePair<string, Dictionary<string, object>>>()), ProgressItemStatus.Working);
            DataMigCommon.DataModel.Data.entitiesEntity _entitiesEntity = ConstructExportDataStructure(entity, strs1);
            if (_entitiesEntity == null)
            {
                UpdateProgress(progressArgs, string.Format(CultureInfo.CurrentUICulture, Resources.FAILED_TO_ADD_RECORDS_TO_EXPORT_SET, entity.displayname, strs1.Count<KeyValuePair<string, Dictionary<string, object>>>()), ProgressItemStatus.Failed);
                return;
            }
            _exportEntitiesDataObject.Add(_entitiesEntity);
            UpdateProgress(progressArgs, string.Format(CultureInfo.CurrentUICulture, Resources.RECORDS_ADDED_TO_EXPORT_SET, entity.displayname, strs1.Count<KeyValuePair<string, Dictionary<string, object>>>()), ProgressItemStatus.Complete);
        }

        private entitiesEntityM2mrelationship FetchManyToManyRelationshipData(Guid guRootId, DataMigCommon.DataModel.Schema.entitiesEntity entity, DataMigCommon.DataModel.Schema.entitiesEntityRelationship m2mRel)
        {
            entitiesEntityM2mrelationship _entitiesEntityM2mrelationship = new entitiesEntityM2mrelationship();
            CrmServiceClient.CrmFilterConditionItem crmFilterConditionItem = new CrmServiceClient.CrmFilterConditionItem()
            {
                FieldName = entity.primaryidfield,
                FieldOperator = ConditionOperator.Equal,
                FieldValue = guRootId
            };
            List<CrmServiceClient.CrmSearchFilter> crmSearchFilters = new List<CrmServiceClient.CrmSearchFilter>()
            {
                new CrmServiceClient.CrmSearchFilter()
                {
                    FilterOperator = LogicalOperator.And,
                    SearchConditions = new List<CrmServiceClient.CrmFilterConditionItem>()
                    {
                        crmFilterConditionItem
                    }
                }
            };
            CrmServiceClient crmServiceClient = CrmServiceClient;
            string str = m2mRel.m2mTargetEntity;
            List<CrmServiceClient.CrmSearchFilter> crmSearchFilters1 = new List<CrmServiceClient.CrmSearchFilter>();
            string str1 = entity.name;
            string str2 = entity.primaryidfield;
            string str3 = m2mRel.name;
            string str4 = m2mRel.m2mTargetEntityPrimaryKey;
            List<string> strs = new List<string>()
            {
                m2mRel.m2mTargetEntityPrimaryKey
            };
            Guid dataByKeyFromResultsSet = new Guid();
            Dictionary<string, Dictionary<string, object>> entityDataByLinkedSearch = crmServiceClient.GetEntityDataByLinkedSearch(str, crmSearchFilters1, str1, crmSearchFilters, str2, str3, str4, CrmServiceClient.LogicalSearchOperator.None, strs, dataByKeyFromResultsSet, m2mRel.isreflexive);
            if (entityDataByLinkedSearch != null && entityDataByLinkedSearch.Count > 0)
            {
                _entitiesEntityM2mrelationship.m2mrelationshipname = m2mRel.name;
                _entitiesEntityM2mrelationship.sourceid = guRootId.ToString();
                _entitiesEntityM2mrelationship.targetentityname = m2mRel.m2mTargetEntity;
                _entitiesEntityM2mrelationship.targetentitynameidfield = m2mRel.m2mTargetEntityPrimaryKey;
                List<string> strs1 = new List<string>();
                foreach (Dictionary<string, object> value in entityDataByLinkedSearch.Values)
                {
                    dataByKeyFromResultsSet = CrmServiceClient.GetDataByKeyFromResultsSet<Guid>(value, m2mRel.m2mTargetEntityPrimaryKey);
                    strs1.Add(dataByKeyFromResultsSet.ToString());
                }
                _entitiesEntityM2mrelationship.targetids = strs1.ToArray();
            }
            if (_entitiesEntityM2mrelationship.targetids != null && _entitiesEntityM2mrelationship.targetids.Count<string>() > 0)
            {
                return _entitiesEntityM2mrelationship;
            }
            return null;
        }

#pragma warning disable IDE0060 // Remove unused parameter
#pragma warning disable IDE0051 // Remove unused private members
        private string FetchRelationshipData(List<DataMigCommon.DataModel.Schema.entitiesEntityRelationship> relationships)
#pragma warning restore IDE0051 // Remove unused private members
#pragma warning restore IDE0060 // Remove unused parameter
        {
            return null;
        }

        private static List<CrmServiceClient.CrmSearchFilter> GenerateSpecialFiltersList(string logicalName, CrmServiceClient client)
        {
            if (!logicalName.Equals("product"))
            {
                if (logicalName.Equals("dynamicproperty"))
                {
                    return new List<CrmServiceClient.CrmSearchFilter>()
                    {
                        new CrmServiceClient.CrmSearchFilter()
                        {
                            FilterOperator = LogicalOperator.Or,
                            SearchConditions = new List<CrmServiceClient.CrmFilterConditionItem>()
                            {
                                new CrmServiceClient.CrmFilterConditionItem()
                                {
                                    FieldName = "statecode",
                                    FieldOperator = ConditionOperator.NotEqual,
                                    FieldValue = 1
                                }
                            }
                        }
                    };
                }
                if (!logicalName.Equals("duplicaterule"))
                {
                    return null;
                }
                return new List<CrmServiceClient.CrmSearchFilter>()
                {
                    new CrmServiceClient.CrmSearchFilter()
                    {
                        FilterOperator = LogicalOperator.And,
                        SearchConditions = new List<CrmServiceClient.CrmFilterConditionItem>()
                        {
                            new CrmServiceClient.CrmFilterConditionItem()
                            {
                                FieldName = "statuscode",
                                FieldOperator = ConditionOperator.Equal,
                                FieldValue = 0
                            },
                            new CrmServiceClient.CrmFilterConditionItem()
                            {
                                FieldName = "statecode",
                                FieldOperator = ConditionOperator.Equal,
                                FieldValue = 0
                            }
                        }
                    }
                };
            }
            List<CrmServiceClient.CrmSearchFilter> crmSearchFilters = new List<CrmServiceClient.CrmSearchFilter>();
            CrmServiceClient.CrmSearchFilter crmSearchFilter = new CrmServiceClient.CrmSearchFilter()
            {
                FilterOperator = LogicalOperator.And
            };
            List<CrmServiceClient.CrmFilterConditionItem> crmFilterConditionItems = new List<CrmServiceClient.CrmFilterConditionItem>()
            {
                new CrmServiceClient.CrmFilterConditionItem()
                {
                    FieldName = "statecode",
                    FieldOperator = ConditionOperator.NotEqual,
                    FieldValue = 2
                },
                new CrmServiceClient.CrmFilterConditionItem()
                {
                    FieldName = "statecode",
                    FieldOperator = ConditionOperator.NotEqual,
                    FieldValue = 3
                }
            };
            if (_addProductIdFilters)
            {
                HashSet<Guid> guids = new HashSet<Guid>();
                foreach (KeyValuePair<string, Dictionary<string, object>> dynamicPropertyAssociation in GetDynamicPropertyAssociations(client))
                {
                    object item = dynamicPropertyAssociation.Value["regardingobjectid"];
                    if (item is string)
                    {
                        item = ((KeyValuePair<string, object>)dynamicPropertyAssociation.Value["regardingobjectid_Property"]).Value;
                    }
                    EntityReference entityReference = (EntityReference)item;
                    if (guids.Contains(entityReference.Id))
                    {
                        continue;
                    }
                    crmFilterConditionItems.Add(new CrmServiceClient.CrmFilterConditionItem()
                    {
                        FieldName = "productid",
                        FieldOperator = ConditionOperator.NotEqual,
                        FieldValue = entityReference.Id
                    });
                    guids.Add(entityReference.Id);
                }
            }
            _addProductIdFilters = false;
            crmSearchFilter.SearchConditions = crmFilterConditionItems;
            crmSearchFilters.Add(crmSearchFilter);
            return crmSearchFilters;
        }

        private object GetDataItemFromResult(Dictionary<string, object> result, string fieldName, string fieldType, out string lookupEntity, out string lookupEntityName)
        {
            KeyValuePair<string, object> dataByKeyFromResultsSet;
            int? nullable;
            int value;
            bool attributeValue;
            bool? value1;
            int? nullable1;
            lookupEntity = string.Empty;
            lookupEntityName = string.Empty;
            string str = string.Concat(fieldName, "_Property");
            string lowerInvariant = fieldType.ToLowerInvariant();
            if (lowerInvariant == "string" || lowerInvariant == "number" || lowerInvariant == "datetime" || lowerInvariant == "decimal" || lowerInvariant == "float")
            {
                dataByKeyFromResultsSet = CrmServiceClient.GetDataByKeyFromResultsSet<KeyValuePair<string, object>>(result, str);
                return dataByKeyFromResultsSet.Value;
            }
            if (lowerInvariant == "money")
            {
                dataByKeyFromResultsSet = CrmServiceClient.GetDataByKeyFromResultsSet<KeyValuePair<string, object>>(result, str);
#pragma warning disable IDE0019 // Use pattern matching
                Money money = dataByKeyFromResultsSet.Value as Money;
#pragma warning restore IDE0019 // Use pattern matching
                if (money == null)
                {
                    return null;
                }
                return money.Value;
            }
            if (lowerInvariant == "optionsetvalue" || lowerInvariant == "status" || lowerInvariant == "state")
            {
                if (!result.ContainsKey(str))
                {
                    nullable = null;
                    nullable1 = nullable;
                }
                else if (CrmServiceClient.GetDataByKeyFromResultsSet<KeyValuePair<string, object>>(result, str).Value is OptionSetValue)
                {
                    dataByKeyFromResultsSet = CrmServiceClient.GetDataByKeyFromResultsSet<KeyValuePair<string, object>>(result, str);
                    nullable1 = new int?((dataByKeyFromResultsSet.Value as OptionSetValue).Value);
                }
                else
                {
                    nullable = null;
                    nullable1 = nullable;
                }
                return nullable1;
            }
            if (lowerInvariant == "owner" || lowerInvariant == "customer" || lowerInvariant == "lookup" || lowerInvariant == "entityreference")
            {
                EntityReference entityReference = CrmServiceClient.GetDataByKeyFromResultsSet<EntityReference>(result, fieldName);
                if (entityReference == null)
                {
                    return Guid.Empty;
                }
                if (!fieldName.Equals("objectid") || !entityReference.LogicalName.Equals("activitymimeattachment") || !result.ContainsKey("objecttypecode"))
                {
                    lookupEntity = entityReference.LogicalName;
                    lookupEntityName = entityReference.Name;
                    return entityReference.Id;
                }
                lookupEntity = CrmServiceClient.GetDataByKeyFromResultsSet<string>(result, "objecttypecode");
                lookupEntityName = entityReference.Name;
                return entityReference.Id;
            }
            if (lowerInvariant != "partylist")
            {
                if (lowerInvariant == "guid")
                {
                    return CrmServiceClient.GetDataByKeyFromResultsSet<Guid>(result, fieldName);
                }
                if (lowerInvariant == "bool")
                {
                    if (result.ContainsKey(str))
                    {
                        dataByKeyFromResultsSet = CrmServiceClient.GetDataByKeyFromResultsSet<KeyValuePair<string, object>>(result, str);
                        value1 = (bool?)dataByKeyFromResultsSet.Value;
                    }
                    else
                    {
                        value1 = null;
                    }
                    return value1;
                }
                if (lowerInvariant == "imagedata")
                {
                    byte[] numArray = CrmServiceClient.GetDataByKeyFromResultsSet<byte[]>(result, fieldName);
                    if (numArray == null)
                    {
                        return null;
                    }
                    return Convert.ToBase64String(numArray);
                }
                if (lowerInvariant != "optionsetvaluecollection")
                {
                    return null;
                }
                dataByKeyFromResultsSet = CrmServiceClient.GetDataByKeyFromResultsSet<KeyValuePair<string, object>>(result, str);
#pragma warning disable IDE0019 // Use pattern matching
                OptionSetValueCollection optionSetValueCollection = dataByKeyFromResultsSet.Value as OptionSetValueCollection;
#pragma warning restore IDE0019 // Use pattern matching
                if (optionSetValueCollection == null)
                {
                    return null;
                }
                return string.Concat("[-1,", string.Join<int>(",",
                    from v in optionSetValueCollection
                    select v.Value into v
                    orderby v
                    select v), ",-1]");
            }
            EntityCollection entityCollection = CrmServiceClient.GetDataByKeyFromResultsSet<EntityCollection>(result, fieldName);
            if (entityCollection == null)
            {
                return null;
            }
            List<entitiesEntityRecordFieldActivitypointerrecords> _entitiesEntityRecordFieldActivitypointerrecords = new List<entitiesEntityRecordFieldActivitypointerrecords>();
            foreach (Entity entity in entityCollection.Entities)
            {
                entitiesEntityRecordFieldActivitypointerrecords entitiesEntityRecordFieldActivitypointerrecord = new entitiesEntityRecordFieldActivitypointerrecords()
                {
                    id = entity.Id.ToString()
                };
                List<entitiesEntityRecordFieldActivitypointerrecordsField> entitiesEntityRecordFieldActivitypointerrecordsFields = new List<entitiesEntityRecordFieldActivitypointerrecordsField>();
                if (entity.Attributes.ContainsKey("partyid"))
                {
                    EntityReference attributeValue1 = entity.GetAttributeValue<EntityReference>("partyid");
                    if (attributeValue1 != null)
                    {
                        entitiesEntityRecordFieldActivitypointerrecordsFields.Add(new entitiesEntityRecordFieldActivitypointerrecordsField()
                        {
                            name = "partyid",
                            @value = attributeValue1.Id.ToString(),
                            lookupentity = attributeValue1.LogicalName,
                            lookupentityname = attributeValue1.Name
                        });
                    }
                }
                if (entity.Attributes.ContainsKey("activityid"))
                {
                    EntityReference entityReference1 = entity.GetAttributeValue<EntityReference>("activityid");
                    if (entityReference1 != null)
                    {
                        entitiesEntityRecordFieldActivitypointerrecordsFields.Add(new entitiesEntityRecordFieldActivitypointerrecordsField()
                        {
                            name = "activityid",
                            @value = entityReference1.Id.ToString(),
                            lookupentity = entityReference1.LogicalName,
                            lookupentityname = entityReference1.Name
                        });
                    }
                }
                if (entity.Attributes.ContainsKey("ownerid"))
                {
                    EntityReference attributeValue2 = entity.GetAttributeValue<EntityReference>("ownerid");
                    if (attributeValue2 != null)
                    {
                        entitiesEntityRecordFieldActivitypointerrecordsFields.Add(new entitiesEntityRecordFieldActivitypointerrecordsField()
                        {
                            name = "ownerid",
                            @value = attributeValue2.Id.ToString(),
                            lookupentity = attributeValue2.LogicalName,
                            lookupentityname = attributeValue2.Name
                        });
                    }
                }
                if (entity.Attributes.ContainsKey("participationtypemask"))
                {
                    OptionSetValue optionSetValue = entity.GetAttributeValue<OptionSetValue>("participationtypemask");
                    if (optionSetValue != null)
                    {
                        entitiesEntityRecordFieldActivitypointerrecordsField _entitiesEntityRecordFieldActivitypointerrecordsField = new entitiesEntityRecordFieldActivitypointerrecordsField()
                        {
                            name = "participationtypemask"
                        };
                        value = optionSetValue.Value;
                        _entitiesEntityRecordFieldActivitypointerrecordsField.@value = value.ToString(CultureInfo.InvariantCulture);
                        entitiesEntityRecordFieldActivitypointerrecordsFields.Add(_entitiesEntityRecordFieldActivitypointerrecordsField);
                    }
                }
                if (entity.Attributes.ContainsKey("instancetypecode"))
                {
                    OptionSetValue optionSetValue1 = entity.GetAttributeValue<OptionSetValue>("instancetypecode");
                    if (optionSetValue1 != null)
                    {
                        entitiesEntityRecordFieldActivitypointerrecordsField _entitiesEntityRecordFieldActivitypointerrecordsField1 = new entitiesEntityRecordFieldActivitypointerrecordsField()
                        {
                            name = "instancetypecode"
                        };
                        value = optionSetValue1.Value;
                        _entitiesEntityRecordFieldActivitypointerrecordsField1.@value = value.ToString(CultureInfo.InvariantCulture);
                        entitiesEntityRecordFieldActivitypointerrecordsFields.Add(_entitiesEntityRecordFieldActivitypointerrecordsField1);
                    }
                }
                if (entity.Attributes.ContainsKey("donotemail"))
                {
                    entitiesEntityRecordFieldActivitypointerrecordsField str1 = new entitiesEntityRecordFieldActivitypointerrecordsField()
                    {
                        name = "donotemail"
                    };
                    attributeValue = entity.GetAttributeValue<bool>("donotemail");
                    str1.@value = attributeValue.ToString();
                    entitiesEntityRecordFieldActivitypointerrecordsFields.Add(str1);
                }
                if (entity.Attributes.ContainsKey("donotfax"))
                {
                    entitiesEntityRecordFieldActivitypointerrecordsField _entitiesEntityRecordFieldActivitypointerrecordsField2 = new entitiesEntityRecordFieldActivitypointerrecordsField()
                    {
                        name = "donotfax"
                    };
                    attributeValue = entity.GetAttributeValue<bool>("donotfax");
                    _entitiesEntityRecordFieldActivitypointerrecordsField2.@value = attributeValue.ToString();
                    entitiesEntityRecordFieldActivitypointerrecordsFields.Add(_entitiesEntityRecordFieldActivitypointerrecordsField2);
                }
                if (entity.Attributes.ContainsKey("donotpostalmail"))
                {
                    entitiesEntityRecordFieldActivitypointerrecordsField str2 = new entitiesEntityRecordFieldActivitypointerrecordsField()
                    {
                        name = "donotpostalmail"
                    };
                    attributeValue = entity.GetAttributeValue<bool>("donotpostalmail");
                    str2.@value = attributeValue.ToString();
                    entitiesEntityRecordFieldActivitypointerrecordsFields.Add(str2);
                }
                if (entity.Attributes.ContainsKey("donotphone"))
                {
                    entitiesEntityRecordFieldActivitypointerrecordsField _entitiesEntityRecordFieldActivitypointerrecordsField3 = new entitiesEntityRecordFieldActivitypointerrecordsField()
                    {
                        name = "donotphone"
                    };
                    attributeValue = entity.GetAttributeValue<bool>("donotphone");
                    _entitiesEntityRecordFieldActivitypointerrecordsField3.@value = attributeValue.ToString();
                    entitiesEntityRecordFieldActivitypointerrecordsFields.Add(_entitiesEntityRecordFieldActivitypointerrecordsField3);
                }
                if (entity.Attributes.ContainsKey("ispartydeleted"))
                {
                    entitiesEntityRecordFieldActivitypointerrecordsField str3 = new entitiesEntityRecordFieldActivitypointerrecordsField()
                    {
                        name = "ispartydeleted"
                    };
                    attributeValue = entity.GetAttributeValue<bool>("ispartydeleted");
                    str3.@value = attributeValue.ToString();
                    entitiesEntityRecordFieldActivitypointerrecordsFields.Add(str3);
                }
                if (entity.Attributes.ContainsKey("addressused"))
                {
                    entitiesEntityRecordFieldActivitypointerrecordsFields.Add(new entitiesEntityRecordFieldActivitypointerrecordsField()
                    {
                        name = "addressused",
                        @value = entity.GetAttributeValue<string>("addressused")
                    });
                }
                if (entity.Attributes.ContainsKey("activitypartyid"))
                {
                    entitiesEntityRecordFieldActivitypointerrecordsFields.Add(new entitiesEntityRecordFieldActivitypointerrecordsField()
                    {
                        name = "activitypartyid",
                        @value = entity.GetAttributeValue<Guid>("activitypartyid").ToString()
                    });
                }
                entitiesEntityRecordFieldActivitypointerrecord.field = entitiesEntityRecordFieldActivitypointerrecordsFields.ToArray();
                _entitiesEntityRecordFieldActivitypointerrecords.Add(entitiesEntityRecordFieldActivitypointerrecord);
            }
            return _entitiesEntityRecordFieldActivitypointerrecords;
        }

        private static Dictionary<string, Dictionary<string, object>> GetDynamicPropertyAssociations(CrmServiceClient client)
        {
            List<string> strs = new List<string>()
            {
                "dynamicpropertyassociationid",
                "regardingobjectid"
            };
            Dictionary<string, Dictionary<string, object>> strs1 = new Dictionary<string, Dictionary<string, object>>();
            string empty = string.Empty;
            bool flag = true;
            int num = 500;
            int num1 = 1;
            while (flag)
            {
                Dictionary<string, Dictionary<string, object>> strs2 = PageEnabledFetchFromCRM("dynamicpropertyassociation", strs, null, ref empty, ref flag, num, num1, client);
                if (strs2 != null && strs2.Count > 0)
                {
                    foreach (KeyValuePair<string, Dictionary<string, object>> keyValuePair in strs2)
                    {
                        strs1.Add(keyValuePair.Key, keyValuePair.Value);
                    }
                }
                if (!flag)
                {
                    continue;
                }
                num1++;
            }
            return strs1;
        }

        private bool HandleProductTaxonomyEntities(DataMigCommon.DataModel.Schema.entitiesEntity entity)
        {
            if (!_productTaxonomyEntities.Contains(entity.name))
            {
                return false;
            }
            ((IEnumerable<DataMigCommon.DataModel.Schema.entitiesEntity>)_schemaMap.entity).Count<DataMigCommon.DataModel.Schema.entitiesEntity>((DataMigCommon.DataModel.Schema.entitiesEntity x) => x.name == "product");
            bool flag = ((IEnumerable<DataMigCommon.DataModel.Schema.entitiesEntity>)_schemaMap.entity).Count<DataMigCommon.DataModel.Schema.entitiesEntity>((DataMigCommon.DataModel.Schema.entitiesEntity x) => _productTaxonomyEntities.Contains(x.name)) == 4;
            _addProductIdFilters = false;
            if (entity.name != "product" && !flag)
            {
                return true;
            }
            if (entity.name == "product" && !flag)
            {
                _addProductIdFilters = true;
            }
            return false;
        }

        private static string MergeFetchXmlWithFields(string fetchXml, List<string> fieldList, string logicalName)
        {
            XDocument xDocument = XDocument.Parse(fetchXml);
            XElement xElement = xDocument.XPathSelectElements("//fetch/entity").FirstOrDefault<XElement>();
            if (xElement == null)
            {
                xDocument = XDocument.Parse(string.Concat(new string[] { "<fetch mapping='logical' version='1.0' distinct='false' output-format='xml-platform'> <entity name='", logicalName, "'>", xDocument.Root.ToString(), "</entity></fetch>" }));
                xElement = xDocument.XPathSelectElements("//fetch/entity").FirstOrDefault<XElement>();
                foreach (string str in fieldList)
                {
                    XElement xElement1 = new XElement("attribute", new XAttribute("name", str));
                    xElement.Add(xElement1);
                }
            }
            else if (xElement != null)
            {
                foreach (string str1 in fieldList)
                {
                    XElement xElement2 = new XElement("attribute", new XAttribute("name", str1));
                    xElement.Add(xElement2);
                }
            }
            StringBuilder stringBuilder = new StringBuilder();
            xDocument.Save(new StringWriter(stringBuilder));
            return stringBuilder.ToString();
        }

        private static Dictionary<string, Dictionary<string, object>> PageEnabledFetchFromCRM(string logicalName, List<string> fieldList, string fetchXml, ref string pageCookie, ref bool isMoreRecords, int iPageCount, int iPageNumber, CrmServiceClient client)
        {
            Dictionary<string, Dictionary<string, object>> entityDataBySearchParams;
            Guid guid;
            List<CrmServiceClient.CrmSearchFilter> crmSearchFilters = GenerateSpecialFiltersList(logicalName, client);
            CrmServiceClient crmServiceClient = client;
            string empty = string.Empty;
            if (crmSearchFilters == null && !string.IsNullOrEmpty(fetchXml))
            {
                empty = MergeFetchXmlWithFields(fetchXml, fieldList, logicalName);
            }
            if (crmSearchFilters != null || string.IsNullOrEmpty(empty))
            {
                Dictionary<string, CrmServiceClient.LogicalSortOrder> strs = new Dictionary<string, CrmServiceClient.LogicalSortOrder>();
                guid = new Guid();
                entityDataBySearchParams = crmServiceClient.GetEntityDataBySearchParams(logicalName, crmSearchFilters, CrmServiceClient.LogicalSearchOperator.None, fieldList, strs, iPageCount, iPageNumber, pageCookie, out pageCookie, out isMoreRecords, guid);
            }
            else
            {
                guid = new Guid();
                entityDataBySearchParams = crmServiceClient.GetEntityDataByFetchSearch(empty, iPageCount, iPageNumber, pageCookie, out pageCookie, out isMoreRecords, guid);
            }
            if (crmSearchFilters != null)
            {
                crmSearchFilters.Clear();
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                crmSearchFilters = null;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
            }
            if (entityDataBySearchParams != null)
            {
                return entityDataBySearchParams;
            }
            return new Dictionary<string, Dictionary<string, object>>();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Microsoft.Xrm.Tooling.Dmt.DataMigCommon.Utility.TraceLogger.Log(System.String)")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private bool ReadDataFromCrm(string schemaFileName)
        {
            bool flag;
            int i;
            _logger.Log("ReadDataFromCrm", TraceEventType.Start);
            if (_schemaMap == null)
            {
                _schemaMap = new DataMigCommon.DataModel.Schema.entities();
            }
            try
            {
                _logger.Log("Deserialize Schema information");
                _schemaMap = Helper.Deserialize<DataMigCommon.DataModel.Schema.entities>(schemaFileName);
                if (_schemaMap == null || _schemaMap.entity == null || _schemaMap.entity.Length == 0)
                {
                    _logger.Log("Failed to deserialize the schema information", TraceEventType.Error);
                    _logger.Log("ReadDataFromCrm", TraceEventType.Stop);
                    flag = false;
                }
                else
                {
                    goto Label0;
                }
            }
            catch
            {
                _logger.Log("ReadDataFromCrm", TraceEventType.Stop);
                _logger.Log("Failed to deserialize the schema information", TraceEventType.Error);
                flag = false;
            }
            return flag;
            Label0:
            ProgressItemEventArgs progressItemEventArg = AddProgressItem(string.Format(CultureInfo.CurrentUICulture, Resources.BEGINING_EXPORT, (int)_schemaMap.entity.Length), ProgressItemStatus.Working);
            ProgressItemEventArgs progressItemEventArg1 = AddProgressItem(Resources.RUNNING_SCHEMA_VALIDATION, ProgressItemStatus.Working);
            SchemaValidator schemaValidator = new SchemaValidator();
            IEnumerable<string> strs =
                from s in (IEnumerable<DataMigCommon.DataModel.Schema.entitiesEntity>)_schemaMap.entity
                select s.name;
            if (!schemaValidator.LoadSchemaInfoForEntities(strs.ToList<string>(), CrmServiceClient, null))
            {
                _logger.Log(string.Format(CultureInfo.InvariantCulture, "Failed Schema Vaildation, Missing Entities: {0} ", schemaValidator.GetFieldStringList(schemaValidator.MissingEntityList)), TraceEventType.Error);
                UpdateProgress(progressItemEventArg1, string.Format(CultureInfo.CurrentUICulture, Resources.FAILED_SCHEMA_VALIDATION_MISSING_ENTITIES, schemaValidator.MissingEntityList.Count), ProgressItemStatus.Failed);
                UpdateProgress(progressItemEventArg, Resources.IMPORT_PROCESS_COMPLETED, ProgressItemStatus.Failed);
                return false;
            }
            List<string> strs1 = new List<string>();
            Dictionary<string, List<string>> strs2 = new Dictionary<string, List<string>>();
            DataMigCommon.DataModel.Schema.entitiesEntity[] entitiesEntityArray = _schemaMap.entity;
            for (i = 0; i < (int)entitiesEntityArray.Length; i++)
            {
                DataMigCommon.DataModel.Schema.entitiesEntity _entitiesEntity = entitiesEntityArray[i];
                if (schemaValidator.ValidateSchemaElement(_entitiesEntity, out strs1))
                {
                    strs2.Add(_entitiesEntity.name, new List<string>(strs1.ToArray()));
                    _logger.Log(string.Format(CultureInfo.InvariantCulture, "Failed Schema Vaildation, Missing Fields on {1}: {0} ", schemaValidator.GetFieldStringList(schemaValidator.MissingEntityList), _entitiesEntity.name), TraceEventType.Error);
                }
            }
            if (strs2.Count > 0)
            {
                UpdateProgress(progressItemEventArg1, Resources.SCHEMA_VALIDATION_FAILED_MISSING_FIELDS_ENTITIES, ProgressItemStatus.Failed);
                UpdateProgress(progressItemEventArg, Resources.IMPORT_PROCESS_COMPLETED, ProgressItemStatus.Failed);
                return false;
            }
            UpdateProgress(progressItemEventArg1, Resources.SCHEMA_VALIDATION_COMPLETE, ProgressItemStatus.Complete);
            _logger.Log("Starting Entity Export Process");
            entitiesEntityArray = _schemaMap.entity;
            for (i = 0; i < (int)entitiesEntityArray.Length; i++)
            {
                DataMigCommon.DataModel.Schema.entitiesEntity _entitiesEntity1 = entitiesEntityArray[i];
                if (!HandleProductTaxonomyEntities(_entitiesEntity1))
                {
                    _logger.Log("*********************************************************");
                    _logger.Log(string.Format(CultureInfo.InvariantCulture, "** Exporting Entity {0} **", _entitiesEntity1.name));
                    ProgressItemEventArgs progressItemEventArg2 = AddProgressItem(string.Format(CultureInfo.CurrentUICulture, Resources.EXPORTING_RECORDS, _entitiesEntity1.displayname), ProgressItemStatus.Working);
                    _logger.Log(string.Format(CultureInfo.InvariantCulture, "Processing {0}", _entitiesEntity1.name));
                    FetchDataFromCrm(_entitiesEntity1, progressItemEventArg2);
                }
            }
            UpdateProgress(progressItemEventArg, string.Format(CultureInfo.CurrentUICulture, Resources.EXPORT_FROM_CRM_ENTITIES_PROCESS_COMPLETE, (int)_schemaMap.entity.Length), ProgressItemStatus.Complete);
            _logger.Log("Entity Export Process Complete");
            _logger.Log("ReadDataFromCrm", TraceEventType.Stop);
            return true;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Microsoft.Xrm.Tooling.Dmt.DataMigCommon.Utility.TraceLogger.Log(System.String,System.Diagnostics.TraceEventType)")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private bool SaveOutputToDisk(string schemaFileName, string outputFileName)
        {
            bool flag;
            _logger.Log("SaveOutputToDisk", TraceEventType.Start);
            try
            {
                string str = Path.Combine((new FileInfo(outputFileName)).DirectoryName, InteractionCommonVars.ExportDataFileName);
                if (File.Exists(str))
                {
                    File.Delete(str);
                }
                _logger.Log(string.Format(CultureInfo.InvariantCulture, "Writting output file : {0}", str));
                string str1 = Helper.Serialize<DataMigCommon.DataModel.Data.entities>(new DataMigCommon.DataModel.Data.entities()
                {
                    entity = _exportEntitiesDataObject.ToArray(),
                    timestamp = DateTime.UtcNow.ToString("o")
                }).Replace("lookupentity=\"\" lookupentityname=\"\" ", string.Empty).Replace("<?xml version=\"1.0\"?>", string.Empty);
                if (!string.IsNullOrWhiteSpace(str1))
                {
                    using (FileStream fileStream = new FileStream(str, FileMode.Create))
                    {
                        byte[] bytes = (new UTF8Encoding()).GetBytes(str1);
                        fileStream.Write(bytes, 0, (int)bytes.Length);
                        fileStream.Flush();
                    }
                    string str2 = Path.Combine((new FileInfo(schemaFileName)).DirectoryName, InteractionCommonVars.ExportScheamDataFileName);
                    if (File.Exists(str2))
                    {
                        File.Delete(str2);
                    }
                    File.Copy(schemaFileName, str2, true);
                    if (ZipUtils.ZipFiles(new List<string>()
                    {
                        str2,
                        str
                    }, outputFileName, true))
                    {
                        if (File.Exists(str2))
                        {
                            File.Delete(str2);
                        }
                        if (File.Exists(str))
                        {
                            File.Delete(str);
                        }
                        _logger.Log("SaveOutputToDisk", TraceEventType.Stop);
                        return true;
                    }
                    else
                    {
                        _logger.Log("Failed to create compressed output file", TraceEventType.Error);
                        _logger.Log("SaveOutputToDisk", TraceEventType.Stop);
                        flag = false;
                    }
                }
                else
                {
                    _logger.Log("Failed to create output file", TraceEventType.Error);
                    _logger.Log("SaveOutputToDisk", TraceEventType.Stop);
                    flag = false;
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                _logger.Log("Exception raised in SaveOutputToDisk", TraceEventType.Error);
                _logger.Log(exception);
                flag = false;
            }
            return flag;
        }

        private void UpdateProgress(ProgressItemEventArgs progressArgs, string text, ProgressItemStatus status = 0)
        {
            progressArgs.progressItem.ItemText = text;
            progressArgs.progressItem.ItemStatus = status;
            if (UpdateProgressItem != null)
            {
#pragma warning disable IDE1005 // Delegate invocation can be simplified.
                UpdateProgressItem(this, progressArgs);
#pragma warning restore IDE1005 // Delegate invocation can be simplified.
            }
        }

        public event EventHandler<ProgressItemEventArgs> AddNewProgressItem;

        public event EventHandler<ProgressItemEventArgs> UpdateProgressItem;
    }
}