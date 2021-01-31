namespace Aylos.Xrm.Sdk.CrmSvcUtilExtensions
{
    using Humanizer;
    using Microsoft.Xrm.Sdk.Metadata;
    using System;
    using System.CodeDom;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    internal static class ServiceHelper
    {
        #region Relationships Handling

        /* TO-DO

        private const string EntityName = "ENTITY_NAME";

        private const string RelationshipSchemaName = "RELATIONSHIP_SCHEMA_NAME";

        private const string PropertyName = "PROPERTY_NAME";

        private const string ManyToManyPropertyNameFormatText = "{0}{1}Association";

        private const string ManyToOnePropertyNameFormatText = "{0}ReferredBy{1}Lookup";

        private const string OneToManyPropertyNameFormatText = "{0}On{1}LookupReferring{2}";

        */

        #endregion

        internal static string ActivityPointerLogicalName = "activitypointer";

        internal static string ActivityPartyLogicalName = "activityparty";

        internal static string StateCodeLogicalName = "statecode";

        internal static IList<EntityInfo> EntityList = new List<EntityInfo>();

        internal static IList<string> ActivityLogicalNames()
        {
            if (string.IsNullOrWhiteSpace(Properties.Settings.Default.ActivityLogicalNames))
            {
                throw new InvalidProgramException("ActivityLogicalNames setting is required.");
            }
            else
            {
                string[] entities = Properties.Settings.Default.ActivityLogicalNames.Split(",".ToCharArray());
                if (entities.Length > 0) return entities.ToList();
                throw new InvalidProgramException("ActivityLogicalNames setting is required.");
            }
        }

        internal static CodeGeneratorOptions CodeGeneratorOptions = new CodeGeneratorOptions()
        {
            BlankLinesBetweenMembers = true,
            BracingStyle = "C",
            ElseOnClosing = false,
            IndentString = "\t",
            VerbatimOrder = true
        };

        internal static IList<string> EnumTypeImportedNamespaces = new List<string> {
            "System.CodeDom.Compiler",
            "System.Runtime.Serialization"
        };

        internal static IList<string> ImportedNamespaces = new List<string> {
            "Microsoft.Xrm.Sdk",
            "Microsoft.Xrm.Sdk.Client",
            "System",
            "System.CodeDom.Compiler",
            "System.Collections.Generic",
            "System.ComponentModel",
            "System.Linq",
            "System.Runtime.Serialization"
        };

        internal static IList<KeyValuePair<string, string>> ReplaceStrings = new List<KeyValuePair<string, string>> {
            new KeyValuePair<string, string> ("Microsoft.Xrm.Sdk.Client.", ""),
            new KeyValuePair<string, string> ("Microsoft.Xrm.Sdk.", ""),
            new KeyValuePair<string, string> ("System.CodeDom.Compiler.", ""),
            new KeyValuePair<string, string> ("System.Collections.Generic.", ""),
            new KeyValuePair<string, string> ("System.ComponentModel.", ""),
            new KeyValuePair<string, string> ("System.Linq.", ""),
            new KeyValuePair<string, string> ("System.Runtime.Serialization.", ""),
            new KeyValuePair<string, string> ("System.Nullable", "Nullable"),
            new KeyValuePair<string, string> ("System.Enum", "Enum"),
            new KeyValuePair<string, string> ("System.Guid", "Guid"),
            new KeyValuePair<string, string> ("System.DateTime", "DateTime"),
            new KeyValuePair<string, string> ("using Client;", "using Microsoft.Xrm.Sdk.Client;"),
            new KeyValuePair<string, string> ("[assembly: ProxyTypesAssemblyAttribute()]", "[assembly: Microsoft.Xrm.Sdk.Client.ProxyTypesAssemblyAttribute()]"),
            new KeyValuePair<string, string> ("((value == null))", "(value == null)"),
            new KeyValuePair<string, string> ("((optionSet != null))", "(optionSet != null)"),
            new KeyValuePair<string, string> ("new OptionSetValue(((int)(value)))", "new OptionSetValue((int)value)"),
        };

        #region Relationships Handling

        /* TO-DO

        internal static IList<EntityMetadata> IntersectionEntityList = new List<EntityMetadata>();

        internal static IList<string> ManyToManyRelationshipsReplaceStrings = new List<string> {
            string.Format(CultureInfo.InvariantCulture, "/// N:N {0}", PropertyName),
            string.Format(CultureInfo.InvariantCulture, "public IEnumerable<{0}> {1}", EntityName, PropertyName),
            string.Format(CultureInfo.InvariantCulture, "this.OnPropertyChanging(\"{0}\");", PropertyName),
            string.Format(CultureInfo.InvariantCulture, "this.OnPropertyChanged(\"{0}\");", PropertyName),
        };

        internal static IList<string> ManyToOneRelationshipsReplaceStrings = new List<string> {
            string.Format(CultureInfo.InvariantCulture, "/// N:1 {0}", PropertyName),
            string.Format(CultureInfo.InvariantCulture, "public {0} {1}", EntityName, PropertyName),
            string.Format(CultureInfo.InvariantCulture, "this.OnPropertyChanging(\"{0}\");", PropertyName),
            string.Format(CultureInfo.InvariantCulture, "this.OnPropertyChanged(\"{0}\");", PropertyName),
        };

        internal static IList<string> OneToManyRelationshipsReplaceStrings = new List<string> {
            string.Format(CultureInfo.InvariantCulture, "/// 1:N {0}", PropertyName),
            string.Format(CultureInfo.InvariantCulture, "public IEnumerable<{0}> {1}", EntityName, PropertyName),
            string.Format(CultureInfo.InvariantCulture, "this.OnPropertyChanging(\"{0}\");", PropertyName),
            string.Format(CultureInfo.InvariantCulture, "this.OnPropertyChanged(\"{0}\");", PropertyName),
        }; */

        #endregion

        internal static void GenerateCodeFile(string filePath, CodeDomProvider codeDomProvider, CodeCompileUnit codeCompileUnit, CodeNamespace codeNamespace)
        {
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentNullException(nameof(filePath));
            if (codeCompileUnit == null) throw new ArgumentNullException(nameof(codeCompileUnit));
            if (codeDomProvider == null) throw new ArgumentNullException(nameof(codeDomProvider));
            if (codeNamespace == null) throw new ArgumentNullException(nameof(codeNamespace));

            Console.WriteLine("Generating '{0}'", filePath);

            WriteFile(filePath, codeCompileUnit, codeDomProvider);

            string stream = ReadFile(filePath);

            stream = stream.Replace(codeNamespace.Name + ".", "");

            foreach (KeyValuePair<string, string> pair in ReplaceStrings)
            {
                stream = stream.Replace(pair.Key, pair.Value);
            }

            #region Relationships handling

            /*
            foreach (EntityInfo entityInfo in EntityList)
            {
                EntityMetadata entityMetadata = entityInfo.EntityMetadata;

                foreach (OneToManyRelationshipMetadata rm in entityMetadata.OneToManyRelationships.Where(x => x is OneToManyRelationshipMetadata).ToList())
                {
                    if (EntityList.SingleOrDefault(x => x.EntityLogicalName == rm.ReferencingEntity) != null)
                    {
                        string lookupPropertyName = TransformMetadataName(EntityList.SingleOrDefault(x => x.EntityLogicalName == rm.ReferencingEntity)?.EntityMetadata?.Attributes.SingleOrDefault(x => x.LogicalName == rm.ReferencingAttribute)?.DisplayName?.UserLocalizedLabel?.Label);
                        string propertyCollectionName = TransformMetadataName(EntityList.SingleOrDefault(x => x.EntityLogicalName == rm.ReferencingEntity)?.EntityMetadata?.DisplayCollectionName?.UserLocalizedLabel?.Label);
                        string propertyName = string.Format(OneToManyPropertyNameFormatText, propertyCollectionName, lookupPropertyName, entityMetadata.SchemaName);
                        string entityName = TransformMetadataName(EntityList.SingleOrDefault(x => x.EntityLogicalName == rm.ReferencingEntity)?.EntityMetadata?.DisplayName?.UserLocalizedLabel?.Label);

                        foreach (string line in OneToManyRelationshipsReplaceStrings)
                        {
                            string find = line.Replace(EntityName, entityName).Replace(PropertyName, rm.SchemaName);
                            string replace = line.Replace(EntityName, entityName).Replace(PropertyName, propertyName);
                            //stream = stream.Replace(find, replace);

                            find = line.Replace(EntityName, entityName).Replace(PropertyName, "Referenced" + rm.SchemaName);
                            //stream = stream.Replace(find, replace);
                        }
                    }
                    else
                    {
                    }
                }

                foreach (OneToManyRelationshipMetadata rm in entityMetadata.ManyToOneRelationships.Where(x => x is OneToManyRelationshipMetadata).ToList())
                {
                    if (EntityList.SingleOrDefault(x => x.EntityLogicalName == rm.ReferencedEntity) != null)
                    {
                        string lookupPropertyName = TransformMetadataName(EntityList.SingleOrDefault(x => x.EntityLogicalName == rm.ReferencingEntity)?.EntityMetadata?.Attributes.SingleOrDefault(x => x.LogicalName == rm.ReferencingAttribute)?.DisplayName?.UserLocalizedLabel?.Label);
                        string entityName = TransformMetadataName(EntityList.SingleOrDefault(x => x.EntityLogicalName == rm.ReferencedEntity)?.EntityMetadata?.DisplayName?.UserLocalizedLabel?.Label);
                        string propertyName = string.Format(ManyToOnePropertyNameFormatText, entityName, lookupPropertyName);

                        foreach (string line in ManyToOneRelationshipsReplaceStrings)
                        {
                            string find = line.Replace(EntityName, entityName).Replace(PropertyName, rm.SchemaName);
                            string replace = line.Replace(EntityName, entityName).Replace(PropertyName, propertyName);
                            //stream = stream.Replace(find, replace);

                            find = line.Replace(EntityName, entityName).Replace(PropertyName, "Referencing" + rm.SchemaName);
                            //stream = stream.Replace(find, replace);
                        }
                    }
                    else
                    {
                    }
                }

                foreach (ManyToManyRelationshipMetadata rm in entityMetadata.ManyToManyRelationships.Where(x => x is ManyToManyRelationshipMetadata).ToList())
                {
                    EntityMetadata intersectionEntity = IntersectionEntityList.SingleOrDefault(x => x.LogicalName == rm.IntersectEntityName);
                    if (intersectionEntity != null)
                    {
                        if (EntityList.SingleOrDefault(x => x.EntityLogicalName == rm.Entity1LogicalName) != null &&
                            EntityList.SingleOrDefault(x => x.EntityLogicalName == rm.Entity2LogicalName) != null)
                        {
                            string entityName = TransformMetadataName(entityMetadata?.DisplayName?.UserLocalizedLabel?.Label);
                            string entityCollectionName = string.Empty;

                            if (entityMetadata.LogicalName == rm.Entity1LogicalName)
                            {
                                if (string.IsNullOrWhiteSpace(rm.Entity1AssociatedMenuConfiguration?.Label?.UserLocalizedLabel?.Label))
                                {
                                    entityCollectionName = TransformMetadataName(EntityList.SingleOrDefault(x => x.EntityLogicalName == rm.Entity2LogicalName)?.EntityMetadata?.DisplayCollectionName?.UserLocalizedLabel?.Label);
                                }
                                else
                                {
                                    entityCollectionName = TransformMetadataName(rm.Entity2AssociatedMenuConfiguration?.Label?.UserLocalizedLabel?.Label);
                                }
                            }
                            else if (entityMetadata.LogicalName == rm.Entity2LogicalName)
                            {
                                if (string.IsNullOrWhiteSpace(rm.Entity2AssociatedMenuConfiguration?.Label?.UserLocalizedLabel?.Label))
                                {
                                    entityCollectionName = TransformMetadataName(EntityList.SingleOrDefault(x => x.EntityLogicalName == rm.Entity1LogicalName)?.EntityMetadata?.DisplayCollectionName?.UserLocalizedLabel?.Label);
                                }
                                else
                                {
                                    entityCollectionName = TransformMetadataName(rm.Entity1AssociatedMenuConfiguration?.Label?.UserLocalizedLabel?.Label);
                                }
                            }
                            else
                            {
                            }


                            string propertyName = string.Format(ManyToManyPropertyNameFormatText, entityName, entityCollectionName);
                            Console.WriteLine(propertyName);

                            foreach (string line in ManyToManyRelationshipsReplaceStrings)
                            {
                                string find = line.Replace(EntityName, entityName).Replace(PropertyName, rm.SchemaName);
                                string replace = line.Replace(EntityName, entityName).Replace(PropertyName, propertyName);
                                //stream = stream.Replace(find, replace);
                            }
                        }
                        else
                        {
                        }
                    }
                }
            } */

            #endregion

            WriteFile(filePath, stream);
        }

        internal static string GetAttributeName(AttributeMetadata attributeMetadata, EntityInfo entityInfo)
        {
            string metadataName;
            if (attributeMetadata.IsPrimaryId.GetValueOrDefault())
            {
                if (attributeMetadata.IsRenameable.Value && attributeMetadata.IsRetrievable.GetValueOrDefault())
                {
                    metadataName = entityInfo.EntityMetadata.SchemaName + Properties.Settings.Default.PrimaryKeySuffix;
                }
                else
                {
                    metadataName = TransformMetadataName(attributeMetadata.SchemaName);
                }
            }
            else
            {
                switch (attributeMetadata.AttributeType)
                {
                    case AttributeTypeCode.State:
                        metadataName = Properties.Settings.Default.StateCodeSuffix;
                        break;

                    case AttributeTypeCode.Status:
                        metadataName = Properties.Settings.Default.StatusReasonSuffix;
                        break;

                    case AttributeTypeCode.Virtual:
                        if (attributeMetadata.AttributeTypeName.Equals("MultiSelectPicklistType"))
                        {
                            metadataName = string.IsNullOrWhiteSpace(attributeMetadata?.DisplayName?.UserLocalizedLabel?.Label)
                                ? TransformMetadataName(attributeMetadata.SchemaName)
                                : TransformMetadataName(attributeMetadata?.DisplayName?.UserLocalizedLabel?.Label);
                        }
                        else
                        {
                            metadataName = null;
                        }
                        break;

                    default:
                        metadataName = string.IsNullOrWhiteSpace(attributeMetadata?.DisplayName?.UserLocalizedLabel?.Label)
                            ? TransformMetadataName(attributeMetadata.SchemaName)
                            : TransformMetadataName(attributeMetadata?.DisplayName?.UserLocalizedLabel?.Label);
                        break;
                }
            }

            return metadataName;
        }

        internal static string GetOptionSetName(EntityMetadata entityMetadata, OptionSetMetadataBase optionSetMetadata)
        {
            string metadataName = TransformMetadataName(optionSetMetadata?.DisplayName?.UserLocalizedLabel?.Label);

            if (optionSetMetadata.OptionSetType == OptionSetType.Boolean)
            {
                return null;
            }
            else if (optionSetMetadata.IsGlobal.GetValueOrDefault())
            {
                return string.IsNullOrWhiteSpace(metadataName) ? null : Properties.Settings.Default.GlobalOptionSetPrefix + metadataName;
            }
            else if (entityMetadata == null)
            {
                return string.IsNullOrWhiteSpace(metadataName) ? null : metadataName;
            }
            else if (optionSetMetadata.Name.EndsWith(StateCodeLogicalName, StringComparison.Ordinal))
            {
                return entityMetadata.SchemaName + Properties.Settings.Default.StateCodeSuffix;
            }
            else if (optionSetMetadata.Name.EndsWith(Properties.Settings.Default.StateCodeSuffix, StringComparison.Ordinal))
            {
                return optionSetMetadata.Name;
            }
            else if (optionSetMetadata.Name.EndsWith(Properties.Settings.Default.StatusReasonSuffix, StringComparison.Ordinal))
            {
                return optionSetMetadata.Name;
            }
            else
            {
                return string.IsNullOrWhiteSpace(metadataName) ? null : entityMetadata.SchemaName + metadataName;
            }
        }

        internal static string GetPrivateAttributeName(string attributeName)
        {
            if (string.IsNullOrWhiteSpace(attributeName)) throw new ArgumentNullException(nameof(attributeName));

            return "_" + char.ToLowerInvariant(attributeName[0]) + attributeName.Substring(1);
        }

        internal static string ReadFile(string filePath)
        {
            string stream = null;

            using (var streamReader = new StreamReader(filePath))
            {
                stream = streamReader.ReadToEnd();
                streamReader.Close();
            }

            return stream;
        }

        internal static string ReplaceSpecialCharacters(string inputText)
        {
            if (string.IsNullOrWhiteSpace(inputText)) throw new ArgumentNullException(nameof(inputText));

            return Regex.Replace(inputText, Properties.Settings.Default.RegexAllowedCharacters, " ");
        }

        internal static string ReplaceDigits(string inputText)
        {
            if (string.IsNullOrWhiteSpace(inputText)) return inputText;

            string numberText = string.Empty;
            bool digitFound = false;

            foreach (char c in inputText.ToCharArray())
            {
                if (char.IsDigit(c))
                {
                    digitFound = true;
                    numberText += c;
                    continue;
                }
                if (digitFound == true) break; else continue;
            }

            if (string.IsNullOrWhiteSpace(numberText))
            {
                return inputText;
            }
            else
            {
                var regex = new Regex(Regex.Escape(numberText));
                string word = Convert.ToInt32(numberText, CultureInfo.InvariantCulture).ToWords().Humanize().Pascalize();
                return ReplaceDigits(regex.Replace(inputText, word, 1));
            }
        }

        internal static string TransformMetadataName(string inputText)
        {
            if (string.IsNullOrWhiteSpace(inputText)) return inputText;

            string result = string.IsNullOrWhiteSpace(Properties.Settings.Default.TextToRemove)
                ? inputText
                : inputText.Replace(Properties.Settings.Default.TextToRemove, string.Empty);
            result = ReplaceSpecialCharacters(result).Trim();
            result = Properties.Settings.Default.ReplaceDigits ? ReplaceDigits(result) : result;
            result = !string.IsNullOrWhiteSpace(result) ? result.Humanize().Pascalize() : result;
            result = result.Length > Properties.Settings.Default.MetadataMaxCharacterCount ? result.Substring(0, Properties.Settings.Default.MetadataMaxCharacterCount) : result;

            if (!string.IsNullOrWhiteSpace(result))
            {
                char firstChar = result.ToCharArray()[0];
                result = Char.IsDigit(firstChar) ? "Number" + result : result;
            }

            return result;
        }

        internal static void WriteFile(string filePath, string stream)
        {
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentNullException(nameof(filePath));
            if (string.IsNullOrWhiteSpace(stream)) throw new ArgumentNullException(nameof(stream));

            using (var streamWriter = new StreamWriter(filePath))
            {
                streamWriter.Write(stream);
                streamWriter.Close();
            }
        }

        internal static void WriteFile(string filePath, CodeCompileUnit codeCompileUnit, CodeDomProvider codeDomProvider)
        {
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentNullException(nameof(filePath));
            if (codeCompileUnit == null) throw new ArgumentNullException(nameof(codeCompileUnit));
            if (codeDomProvider == null) throw new ArgumentNullException(nameof(codeDomProvider));

            using (var streamWriter = new StreamWriter(filePath))
            {
                codeDomProvider.GenerateCodeFromCompileUnit(codeCompileUnit, streamWriter, CodeGeneratorOptions);
                streamWriter.Close();
            }
        }
    }
}
