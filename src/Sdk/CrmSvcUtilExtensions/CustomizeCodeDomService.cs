namespace Aylos.Xrm.Sdk.CrmSvcUtilExtensions
{
    using Microsoft.Crm.Services.Utility;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Metadata;
    using System;
    using System.CodeDom;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    internal class CustomizeCodeDomService : ICustomizeCodeDomService
    {
        private const string AttributeIsNotExpectedMessage = "Attribute type is not the expected.";

        private IList<string> ReferencedOptionSetNames { get; set; }

        public void CustomizeCodeDom(CodeCompileUnit codeCompileUnit, IServiceProvider services)
        {
            if (codeCompileUnit == null) throw new ArgumentNullException(nameof(codeCompileUnit));
            if (services == null) throw new ArgumentNullException(nameof(services));

            Console.WriteLine(string.Empty);
            Console.WriteLine("Generating code is in progress.");

            ReferencedOptionSetNames = new List<string>();

            ICodeWriterFilterService codeWriterFilterService = (ICodeWriterFilterService)services.GetService(typeof(ICodeWriterFilterService));
            IMetadataProviderService metadataProviderService = (IMetadataProviderService)services.GetService(typeof(IMetadataProviderService));
            IOrganizationMetadata organizationMetadata = metadataProviderService.LoadMetadata();

            // ServiceHelper.IntersectionEntityList = organizationMetadata.Entities.Where(x => x.IsIntersect == true).ToList();

            foreach (CodeNamespace codeNamespace in codeCompileUnit.Namespaces)
            {
                foreach (EntityInfo entityInfo in ServiceHelper.EntityList)
                {
                    EntityMetadata entityMetadata = entityInfo.EntityMetadata;

                    CodeTypeDeclaration codeTypeDeclaration = codeNamespace
                        .Types
                        .OfType<CodeTypeDeclaration>()
                        .FirstOrDefault(codeType => codeType.Name.ToUpperInvariant() == entityMetadata.SchemaName.ToUpperInvariant());

                    GenerateMultiSelectOptionSets(codeNamespace, codeTypeDeclaration, entityMetadata);

                    GenerateOptionSets(codeNamespace, codeTypeDeclaration, entityMetadata);

                    GenerateSetterForReadOnlyFields(codeTypeDeclaration, entityMetadata);
                }
            }

            ExcludeNonReferencedGlobalOptionSets(codeCompileUnit, organizationMetadata);

            GenerateFieldStructures(codeCompileUnit);

            GenerateCodeFiles(codeCompileUnit);

            CleanupNamespaces(codeCompileUnit);

            Console.WriteLine("Generating code completed successfully.");
        }

        private void CleanupNamespaces(CodeCompileUnit codeCompileUnit)
        {
            if (codeCompileUnit == null) throw new ArgumentNullException(nameof(codeCompileUnit));

            if (!Properties.Settings.Default.GenerateSeparateFilesPerType) return;

            Console.WriteLine(string.Empty);
            Console.WriteLine("Cleaning up all namespaces/types.");

            while (codeCompileUnit.Namespaces.Count > 0)
            {
                CodeNamespace cn = codeCompileUnit.Namespaces[0];

                if (cn != null) codeCompileUnit.Namespaces.Remove(cn);
            }
        }

        private void ExcludeNonReferencedGlobalOptionSets(CodeCompileUnit codeCompileUnit, IOrganizationMetadata organizationMetadata)
        {
            if (codeCompileUnit == null) throw new ArgumentNullException(nameof(codeCompileUnit));
            if (organizationMetadata == null) throw new ArgumentNullException(nameof(organizationMetadata));

            if (!Properties.Settings.Default.GenerateOnlyReferencedGlobalOptionSets) return;

            Console.WriteLine(string.Empty);
            Console.WriteLine("Excluding non referenced option sets.");

            foreach (OptionSetMetadataBase optionSetMetadata in organizationMetadata.OptionSets)
            {
                if (ReferencedOptionSetNames.Contains(optionSetMetadata.Name)) continue;

                foreach (CodeNamespace codeNamespace in codeCompileUnit.Namespaces)
                {
                    CodeTypeDeclaration codeTypeDeclaration = codeNamespace
                        .Types.OfType<CodeTypeDeclaration>()
                        .FirstOrDefault(codeType => codeType.Name.ToUpperInvariant() == optionSetMetadata.Name.ToUpperInvariant());

                    if (codeTypeDeclaration == null) continue;

                    codeNamespace.Types.Remove(codeTypeDeclaration);
                }
            }
        }

        private void GenerateFieldStructures(CodeCompileUnit codeCompileUnit)
        {
            if (codeCompileUnit == null) throw new ArgumentNullException(nameof(codeCompileUnit));

            if (!Properties.Settings.Default.GenerateFieldStructures) return;

            Console.WriteLine(string.Empty);
            Console.WriteLine("Generating field structure.");

            foreach (CodeNamespace codeNamespace in codeCompileUnit.Namespaces)
            {
                foreach (CodeTypeDeclaration ctd in codeNamespace.Types)
                {
                    if (!ctd.IsClass) continue;

                    Dictionary<string, string> fields = new Dictionary<string, string>();

                    foreach (var ctdm in ctd.Members)
                    {
                        if (ctdm.GetType() != typeof(CodeMemberProperty)) continue;

                        var cmp = (CodeMemberProperty)ctdm;

                        if (cmp.CustomAttributes.Count > 0 && cmp.CustomAttributes[0].AttributeType.BaseType == typeof(AttributeLogicalNameAttribute).FullName)
                        {
                            if (fields.ContainsKey(cmp.Name)) continue;

                            Console.WriteLine("Adding field: '{0}'.", cmp.Name);

                            fields.Add(cmp.Name, ((CodePrimitiveExpression)cmp.CustomAttributes[0].Arguments[0].Value).Value.ToString());
                        }
                    }

                    if (fields.Count > 0)
                    {
                        var codeTypeDeclaration = new CodeTypeDeclaration
                        {
                            Attributes = MemberAttributes.Public,
                            IsStruct = true,
                            Name = Properties.Settings.Default.FieldStructName,
                            TypeAttributes = TypeAttributes.NestedPublic
                        };

                        foreach (KeyValuePair<string, string> field in fields)
                        {
                            codeTypeDeclaration.Members.Add(new CodeMemberField
                            {
                                Attributes = MemberAttributes.Public | MemberAttributes.Static,
                                Name = field.Key,
                                Type = new CodeTypeReference(typeof(string)),
                                InitExpression = new CodePrimitiveExpression(field.Value)
                            });
                        }

                        ctd.Members.Add(codeTypeDeclaration);
                    }
                }
            }
        }

        private void GenerateCodeFiles(CodeCompileUnit codeCompileUnit)
        {
            if (codeCompileUnit == null) throw new ArgumentNullException(nameof(codeCompileUnit));

            Console.WriteLine(string.Empty);
            Console.WriteLine("Generating files.");

            if (Directory.Exists(Properties.Settings.Default.OutputPath))
            {
                foreach (string file in Directory.GetFiles(Properties.Settings.Default.OutputPath))
                {
                    File.Delete(file);
                }
            }
            else
            {
                Directory.CreateDirectory(Properties.Settings.Default.OutputPath);
            }

            using (CodeDomProvider codeDomProvider = CodeDomProvider.CreateProvider("CSharp"))
            {
                if (Properties.Settings.Default.GenerateSeparateFilesPerType)
                {
                    foreach (CodeNamespace codeNamespace in codeCompileUnit.Namespaces)
                    {
                        foreach (CodeTypeDeclaration codeTypeDeclaration in codeNamespace.Types)
                        {
                            var cn = new CodeNamespace(codeNamespace.Name); 
                            
                            cn.Types.Add(codeTypeDeclaration);

                            var ccu = new CodeCompileUnit(); 
                            
                            ccu.Namespaces.Add(cn);

                            if (codeTypeDeclaration.IsEnum)
                            {
                                foreach (string importedNamespace in ServiceHelper.EnumTypeImportedNamespaces) cn.Imports.Add(new CodeNamespaceImport { Namespace = importedNamespace });
                            }
                            else
                            {
                                foreach (string importedNamespace in ServiceHelper.ImportedNamespaces) cn.Imports.Add(new CodeNamespaceImport { Namespace = importedNamespace });
                            }

                            string filePath = Path.Combine(Properties.Settings.Default.OutputPath, string.Format(CultureInfo.CurrentCulture, "{0}.cs", codeTypeDeclaration.Name));

                            ServiceHelper.GenerateCodeFile(filePath, codeDomProvider, ccu, cn);
                        }
                    }
                }
                else
                {
                    string filePath = Path.Combine(Properties.Settings.Default.OutputPath, string.Format(CultureInfo.CurrentCulture, "{0}", Properties.Settings.Default.Filename));

                    foreach (CodeNamespace codeNamespace in codeCompileUnit.Namespaces)
                    {
                        foreach (string importedNamespace in ServiceHelper.ImportedNamespaces)
                        {
                            codeNamespace.Imports.Add(new CodeNamespaceImport { Namespace = importedNamespace });
                        }

                        ServiceHelper.GenerateCodeFile(filePath, codeDomProvider, codeCompileUnit, codeNamespace);
                    }
                }
            }
        }

        private void GenerateMultiSelectOptionSets(CodeNamespace codeNamespace, CodeTypeDeclaration codeTypeDeclaration, EntityMetadata entityMetadata)
        {
            if (codeNamespace == null) throw new ArgumentNullException(nameof(codeNamespace));
            if (codeTypeDeclaration == null) throw new ArgumentNullException(nameof(codeTypeDeclaration));
            if (entityMetadata == null) throw new ArgumentNullException(nameof(entityMetadata));

            Console.WriteLine(string.Empty);
            Console.WriteLine("Generating multi-select option sets.");

            IList<AttributeMetadata> attributeMetadataList = entityMetadata.Attributes
                .Where(x => !Properties.Settings.Default.GlobalFieldBlackList.Split(",".ToCharArray()).Contains(x.LogicalName))
                .Where(x => x is MultiSelectPicklistAttributeMetadata).ToList();

            foreach (AttributeMetadata am in attributeMetadataList)
            {
                IList<CodeMemberField> privateAttributes = new List<CodeMemberField>();

                foreach (CodeTypeMember ctm in codeTypeDeclaration.Members.OfType<CodeTypeMember>().Where(x => x.Name.ToUpperInvariant() == am.SchemaName.ToUpperInvariant()))
                {
                    OptionSetMetadata osm = ((MultiSelectPicklistAttributeMetadata)am).OptionSet;
                    string osn = ServiceHelper.GetOptionSetName(entityMetadata, osm); if (string.IsNullOrWhiteSpace(osn)) continue;

                    ReferencedOptionSetNames.Add(osn);

                    CodeMemberProperty cmp = (CodeMemberProperty)ctm;
                    string typeName = string.IsNullOrWhiteSpace(codeNamespace.Name) ? osn : string.Format(CultureInfo.InvariantCulture, "{0}.{1}", codeNamespace.Name, osn);
                    cmp.Type = new CodeTypeReference(string.Format(CultureInfo.InvariantCulture, "System.Collections.Generic.HashSet<{0}>", typeName));

                    if (cmp.HasGet || cmp.HasSet)
                    {
                        privateAttributes.Add(new CodeMemberField
                        {
                            Attributes = MemberAttributes.Private,
                            Name = ServiceHelper.GetPrivateAttributeName(osn),
                            Type = cmp.Type,
                            InitExpression = new CodePrimitiveExpression(null)
                        });
                    }

                    if (cmp.HasGet)
                    {
                        Console.WriteLine("Generating getter for property: '{0}'.", cmp.Name);
                        GenerateGetterForMultiSelectOptionSetProperty(codeNamespace, am, cmp, entityMetadata);
                    }

                    if (cmp.HasSet)
                    {
                        Console.WriteLine("Generating setter for property: '{0}'.", cmp.Name);
                        GenerateSetterForMultiSelectOptionSetProperty(codeNamespace, am, cmp, entityMetadata);
                    }
                }

                foreach (CodeMemberField cmf in privateAttributes)
                {
                    codeTypeDeclaration.Members.Add(cmf);
                }
            }
        }

        private void GenerateGetterForMultiSelectOptionSetProperty(CodeNamespace codeNamespace, AttributeMetadata attributeMetadata, CodeMemberProperty codeMemberProperty, EntityMetadata entityMetadata)
        {
            if (codeNamespace == null) throw new ArgumentNullException(nameof(codeNamespace));
            if (attributeMetadata == null) throw new ArgumentNullException(nameof(attributeMetadata));
            if (codeMemberProperty == null) throw new ArgumentNullException(nameof(codeMemberProperty));
            if (entityMetadata == null) throw new ArgumentNullException(nameof(entityMetadata));
            if (attributeMetadata.GetType() != typeof(MultiSelectPicklistAttributeMetadata)) throw new InvalidCastException(AttributeIsNotExpectedMessage);

            OptionSetMetadata osm = ((MultiSelectPicklistAttributeMetadata)attributeMetadata).OptionSet;
            string osn = ServiceHelper.GetOptionSetName(entityMetadata, osm); if (string.IsNullOrWhiteSpace(osn)) return;
            string typeName = string.IsNullOrWhiteSpace(codeNamespace.Name) ? osn : string.Format(CultureInfo.InvariantCulture, "{0}.{1}", codeNamespace.Name, osn);

            codeMemberProperty.GetStatements.Clear();
            codeMemberProperty.GetStatements.Add(new CodeSnippetStatement(string.Format(CultureInfo.InvariantCulture, "\t\t\t\tif ({0} == null)", ServiceHelper.GetPrivateAttributeName(osn))));
            codeMemberProperty.GetStatements.Add(new CodeSnippetStatement("\t\t\t\t{"));
            codeMemberProperty.GetStatements.Add(new CodeSnippetStatement(string.Format(CultureInfo.InvariantCulture, "\t\t\t\t\tvar optionSetValueCollection = this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValueCollection>(\"{0}\");", attributeMetadata.LogicalName)));
            codeMemberProperty.GetStatements.Add(new CodeSnippetStatement("\t\t\t\t\tif (optionSetValueCollection == null)"));
            codeMemberProperty.GetStatements.Add(new CodeSnippetStatement("\t\t\t\t\t{"));
            codeMemberProperty.GetStatements.Add(new CodeSnippetStatement("\t\t\t\t\t\treturn null;"));
            codeMemberProperty.GetStatements.Add(new CodeSnippetStatement("\t\t\t\t\t}"));
            codeMemberProperty.GetStatements.Add(new CodeSnippetStatement("\t\t\t\t\telse"));
            codeMemberProperty.GetStatements.Add(new CodeSnippetStatement("\t\t\t\t\t{"));
            codeMemberProperty.GetStatements.Add(new CodeSnippetStatement(string.Format(CultureInfo.InvariantCulture, "\t\t\t\t\t\t{1} = new System.Collections.Generic.HashSet<{0}>();", typeName, ServiceHelper.GetPrivateAttributeName(osn))));
            codeMemberProperty.GetStatements.Add(new CodeSnippetStatement("\t\t\t\t\t\tforeach(var optionSet in optionSetValueCollection)"));
            codeMemberProperty.GetStatements.Add(new CodeSnippetStatement("\t\t\t\t\t\t{"));
            codeMemberProperty.GetStatements.Add(new CodeSnippetStatement(string.Format(CultureInfo.InvariantCulture, "\t\t\t\t\t\t\t{1}.Add(({0})(System.Enum.ToObject(typeof({0}), optionSet.Value)));", typeName, ServiceHelper.GetPrivateAttributeName(osn))));
            codeMemberProperty.GetStatements.Add(new CodeSnippetStatement("\t\t\t\t\t\t}"));
            codeMemberProperty.GetStatements.Add(new CodeSnippetStatement("\t\t\t\t\t}"));
            codeMemberProperty.GetStatements.Add(new CodeSnippetStatement("\t\t\t\t}"));
            codeMemberProperty.GetStatements.Add(new CodeSnippetStatement("\t\t\t\telse"));
            codeMemberProperty.GetStatements.Add(new CodeSnippetStatement("\t\t\t\t{"));
            codeMemberProperty.GetStatements.Add(new CodeSnippetStatement("\t\t\t\t\tvar optionSetValueCollection = new Microsoft.Xrm.Sdk.OptionSetValueCollection();"));
            codeMemberProperty.GetStatements.Add(new CodeSnippetStatement(string.Format(CultureInfo.InvariantCulture, "\t\t\t\t\tforeach(var item in {0})", ServiceHelper.GetPrivateAttributeName(osn))));
            codeMemberProperty.GetStatements.Add(new CodeSnippetStatement("\t\t\t\t\t{"));
            codeMemberProperty.GetStatements.Add(new CodeSnippetStatement("\t\t\t\t\t\toptionSetValueCollection.Add(new Microsoft.Xrm.Sdk.OptionSetValue((int)item));"));
            codeMemberProperty.GetStatements.Add(new CodeSnippetStatement("\t\t\t\t\t}"));
            codeMemberProperty.GetStatements.Add(new CodeSnippetStatement(string.Format(CultureInfo.InvariantCulture, "\t\t\t\t\tthis.SetAttributeValue(\"{0}\", optionSetValueCollection);", attributeMetadata.LogicalName)));
            codeMemberProperty.GetStatements.Add(new CodeSnippetStatement("\t\t\t\t}"));
            codeMemberProperty.GetStatements.Add(new CodeSnippetStatement(string.Format(CultureInfo.InvariantCulture, "\t\t\t\treturn {0};", ServiceHelper.GetPrivateAttributeName(osn))));
        }

        private void GenerateSetterForMultiSelectOptionSetProperty(CodeNamespace codeNamespace, AttributeMetadata attributeMetadata, CodeMemberProperty codeMemberProperty, EntityMetadata entityMetadata)
        {
            if (codeNamespace == null) throw new ArgumentNullException(nameof(codeNamespace));
            if (attributeMetadata == null) throw new ArgumentNullException(nameof(attributeMetadata));
            if (codeMemberProperty == null) throw new ArgumentNullException(nameof(codeMemberProperty));
            if (entityMetadata == null) throw new ArgumentNullException(nameof(entityMetadata));
            if (attributeMetadata.GetType() != typeof(MultiSelectPicklistAttributeMetadata)) throw new InvalidCastException(AttributeIsNotExpectedMessage);

            OptionSetMetadata osm = ((MultiSelectPicklistAttributeMetadata)attributeMetadata).OptionSet;
            string osn = ServiceHelper.GetOptionSetName(entityMetadata, osm); if (string.IsNullOrWhiteSpace(osn)) return;

            codeMemberProperty.SetStatements.Clear();
            codeMemberProperty.SetStatements.Add(new CodeSnippetStatement(string.Format(CultureInfo.InvariantCulture, "\t\t\t\tthis.OnPropertyChanging(\"{0}\");", attributeMetadata.SchemaName)));
            codeMemberProperty.SetStatements.Add(new CodeSnippetStatement("\t\t\t\tif (value == null)"));
            codeMemberProperty.SetStatements.Add(new CodeSnippetStatement("\t\t\t\t{"));
            codeMemberProperty.SetStatements.Add(new CodeSnippetStatement(string.Format(CultureInfo.InvariantCulture, "\t\t\t\t\tthis.SetAttributeValue(\"{0}\", null);", attributeMetadata.LogicalName)));
            codeMemberProperty.SetStatements.Add(new CodeSnippetStatement("\t\t\t\t}"));
            codeMemberProperty.SetStatements.Add(new CodeSnippetStatement("\t\t\t\telse"));
            codeMemberProperty.SetStatements.Add(new CodeSnippetStatement("\t\t\t\t{"));
            codeMemberProperty.SetStatements.Add(new CodeSnippetStatement("\t\t\t\t\tvar optionSetValueCollection = new Microsoft.Xrm.Sdk.OptionSetValueCollection();"));
            codeMemberProperty.SetStatements.Add(new CodeSnippetStatement("\t\t\t\t\tforeach(var item in value)"));
            codeMemberProperty.SetStatements.Add(new CodeSnippetStatement("\t\t\t\t\t{"));
            codeMemberProperty.SetStatements.Add(new CodeSnippetStatement("\t\t\t\t\t\toptionSetValueCollection.Add(new Microsoft.Xrm.Sdk.OptionSetValue((int)item));"));
            codeMemberProperty.SetStatements.Add(new CodeSnippetStatement("\t\t\t\t\t}"));
            codeMemberProperty.SetStatements.Add(new CodeSnippetStatement(string.Format(CultureInfo.InvariantCulture, "\t\t\t\t\tthis.SetAttributeValue(\"{0}\", optionSetValueCollection);", attributeMetadata.LogicalName)));
            codeMemberProperty.SetStatements.Add(new CodeSnippetStatement("\t\t\t\t}"));
            codeMemberProperty.SetStatements.Add(new CodeSnippetStatement(string.Format(CultureInfo.InvariantCulture, "\t\t\t\t{0} = value;", ServiceHelper.GetPrivateAttributeName(osn))));
            codeMemberProperty.SetStatements.Add(new CodeSnippetStatement(string.Format(CultureInfo.InvariantCulture, "\t\t\t\tthis.OnPropertyChanged(\"{0}\");", attributeMetadata.SchemaName)));
        }

        private void GenerateOptionSets(CodeNamespace codeNamespace, CodeTypeDeclaration codeTypeDeclaration, EntityMetadata entityMetadata)
        {
            if (codeNamespace == null) throw new ArgumentNullException(nameof(codeNamespace));
            if (codeTypeDeclaration == null) throw new ArgumentNullException(nameof(codeTypeDeclaration));
            if (entityMetadata == null) throw new ArgumentNullException(nameof(entityMetadata));

            Console.WriteLine(string.Empty);
            Console.WriteLine("Generating option sets.");

            IList<AttributeMetadata> attributeMetadataList = entityMetadata.Attributes
                .Where(x => !Properties.Settings.Default.GlobalFieldBlackList.Split(",".ToCharArray()).Contains(x.LogicalName))
                .Where(x => x is PicklistAttributeMetadata).ToList();

            foreach (AttributeMetadata am in attributeMetadataList)
            {
                foreach (CodeTypeMember ctm in codeTypeDeclaration.Members.OfType<CodeTypeMember>().Where(x => x.Name.ToUpperInvariant() == am.SchemaName.ToUpperInvariant()))
                {
                    OptionSetMetadata osm = ((PicklistAttributeMetadata)am).OptionSet;
                    string osn = ServiceHelper.GetOptionSetName(entityMetadata, osm);
                    if (string.IsNullOrWhiteSpace(osn)) continue;

                    ReferencedOptionSetNames.Add(osn);

                    CodeMemberProperty cmp = (CodeMemberProperty)ctm;
                    string typeName = string.IsNullOrWhiteSpace(codeNamespace.Name) ? osn : string.Format(CultureInfo.InvariantCulture, "{0}.{1}", codeNamespace.Name, osn);
                    cmp.Type = new CodeTypeReference(string.Format(CultureInfo.InvariantCulture, "System.Nullable<{0}>", typeName));

                    if (cmp.HasGet)
                    {
                        Console.WriteLine("Generating getter for property: '{0}'.", cmp.Name);
                        GenerateGetterForOptionSetProperty(codeNamespace, am, cmp, entityMetadata);
                    }

                    if (cmp.HasSet)
                    {
                        Console.WriteLine("Generating setter for property: '{0}'.", cmp.Name);
                        GenerateSetterForOptionSetProperty(am, cmp);
                    }
                }
            }
        }

        private void GenerateGetterForOptionSetProperty(CodeNamespace codeNamespace, AttributeMetadata attributeMetadata, CodeMemberProperty codeMemberProperty, EntityMetadata entityMetadata)
        {
            if (codeNamespace == null) throw new ArgumentNullException(nameof(codeNamespace));
            if (attributeMetadata == null) throw new ArgumentNullException(nameof(attributeMetadata));
            if (codeMemberProperty == null) throw new ArgumentNullException(nameof(codeMemberProperty));
            if (entityMetadata == null) throw new ArgumentNullException(nameof(entityMetadata));
            if (attributeMetadata.GetType() != typeof(PicklistAttributeMetadata)) throw new InvalidCastException(AttributeIsNotExpectedMessage);

            OptionSetMetadata osm = ((PicklistAttributeMetadata)attributeMetadata).OptionSet;
            string osn = ServiceHelper.GetOptionSetName(entityMetadata, osm); if (string.IsNullOrWhiteSpace(osn)) return;
            osn = string.IsNullOrWhiteSpace(codeNamespace.Name) ? osn : string.Format(CultureInfo.InvariantCulture, "{0}.{1}", codeNamespace.Name, osn);

            codeMemberProperty.GetStatements.Clear();
            codeMemberProperty.GetStatements.Add(new CodeSnippetStatement(string.Format(CultureInfo.InvariantCulture, "\t\t\t\tvar optionSet = this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>(\"{0}\");", attributeMetadata.LogicalName)));
            codeMemberProperty.GetStatements.Add(new CodeSnippetStatement("\t\t\t\tif (optionSet == null)"));
            codeMemberProperty.GetStatements.Add(new CodeSnippetStatement("\t\t\t\t{"));
            codeMemberProperty.GetStatements.Add(new CodeSnippetStatement("\t\t\t\t\treturn null;"));
            codeMemberProperty.GetStatements.Add(new CodeSnippetStatement("\t\t\t\t}"));
            codeMemberProperty.GetStatements.Add(new CodeSnippetStatement("\t\t\t\telse"));
            codeMemberProperty.GetStatements.Add(new CodeSnippetStatement("\t\t\t\t{"));
            codeMemberProperty.GetStatements.Add(new CodeSnippetStatement(string.Format(CultureInfo.InvariantCulture, "\t\t\t\t\treturn ({0})(System.Enum.ToObject(typeof({0}), optionSet.Value));", osn)));
            codeMemberProperty.GetStatements.Add(new CodeSnippetStatement("\t\t\t\t}"));
        }

        private void GenerateSetterForOptionSetProperty(AttributeMetadata attributeMetadata, CodeMemberProperty codeMemberProperty)
        {
            if (attributeMetadata == null) throw new ArgumentNullException(nameof(attributeMetadata));
            if (codeMemberProperty == null) throw new ArgumentNullException(nameof(codeMemberProperty));
            if (attributeMetadata.GetType() != typeof(PicklistAttributeMetadata)) throw new InvalidCastException(AttributeIsNotExpectedMessage);

            codeMemberProperty.SetStatements.Clear();
            codeMemberProperty.SetStatements.Add(new CodeSnippetStatement(string.Format(CultureInfo.InvariantCulture, "\t\t\t\tthis.OnPropertyChanging(\"{0}\");", attributeMetadata.SchemaName)));
            codeMemberProperty.SetStatements.Add(new CodeSnippetStatement("\t\t\t\tif (value == null)"));
            codeMemberProperty.SetStatements.Add(new CodeSnippetStatement("\t\t\t\t{"));
            codeMemberProperty.SetStatements.Add(new CodeSnippetStatement(string.Format(CultureInfo.InvariantCulture, "\t\t\t\t\tthis.SetAttributeValue(\"{0}\", null);", attributeMetadata.LogicalName)));
            codeMemberProperty.SetStatements.Add(new CodeSnippetStatement("\t\t\t\t}"));
            codeMemberProperty.SetStatements.Add(new CodeSnippetStatement("\t\t\t\telse"));
            codeMemberProperty.SetStatements.Add(new CodeSnippetStatement("\t\t\t\t{"));
            codeMemberProperty.SetStatements.Add(new CodeSnippetStatement(string.Format(CultureInfo.InvariantCulture, "\t\t\t\t\tthis.SetAttributeValue(\"{0}\", new Microsoft.Xrm.Sdk.OptionSetValue((int)value));", attributeMetadata.LogicalName)));
            codeMemberProperty.SetStatements.Add(new CodeSnippetStatement("\t\t\t\t}"));
            codeMemberProperty.SetStatements.Add(new CodeSnippetStatement(string.Format(CultureInfo.InvariantCulture, "\t\t\t\tthis.OnPropertyChanged(\"{0}\");", attributeMetadata.SchemaName)));
        }

        private void GenerateSetterForReadOnlyFields(CodeTypeDeclaration codeTypeDeclaration, EntityMetadata entityMetadata)
        {
            if (codeTypeDeclaration == null) throw new ArgumentNullException(nameof(codeTypeDeclaration));
            if (entityMetadata == null) throw new ArgumentNullException(nameof(entityMetadata));

            Console.WriteLine(string.Empty);
            Console.WriteLine("Generating setters for read-only properties.");

            IList<AttributeMetadata> attributeMetadataList = entityMetadata.Attributes
                .Where(x => !Properties.Settings.Default.GlobalFieldBlackList.Split(",".ToCharArray()).Contains(x.LogicalName))
                .Where(x => Properties.Settings.Default.GenerateSettersFor.Split(",".ToCharArray()).Contains(x.LogicalName))
                .Where(x => x.IsValidForCreate == false || x.IsValidForUpdate == false).ToList();

            foreach (AttributeMetadata am in attributeMetadataList)
            {
                foreach (CodeTypeMember ctm in codeTypeDeclaration.Members.OfType<CodeTypeMember>().Where(x => x.Name == am.SchemaName))
                {
                    CodeMemberProperty cmp = (CodeMemberProperty)ctm;

                    if (cmp.HasSet) continue;

                    Console.WriteLine("Generating setter for read-only property: '{0}'.", cmp.Name);

                    cmp.HasSet = true;

                    if (am.GetType() == typeof(PicklistAttributeMetadata))
                    {
                        GenerateSetterForOptionSetProperty(am, cmp);
                    }
                    else if (am.GetType() == typeof(MultiSelectPicklistAttributeMetadata))
                    {

                    }
                    else
                    {
                        cmp.SetStatements.Add(new CodeSnippetStatement(string.Format(CultureInfo.InvariantCulture, "\t\t\t\tthis.OnPropertyChanging(\"{0}\");", am.SchemaName)));
                        cmp.SetStatements.Add(new CodeSnippetStatement(string.Format(CultureInfo.InvariantCulture, "\t\t\t\tthis.SetAttributeValue(\"{0}\", value);", am.LogicalName)));
                        cmp.SetStatements.Add(new CodeSnippetStatement(string.Format(CultureInfo.InvariantCulture, "\t\t\t\tthis.OnPropertyChanged(\"{0}\");", am.SchemaName)));
                    }
                    cmp.Comments.Add(new CodeCommentStatement("<remarks>", true));
                    cmp.Comments.Add(new CodeCommentStatement("The property is read-only and the setter has been added to assist with unit testing only.", true));
                    cmp.Comments.Add(new CodeCommentStatement("</remarks>", true));
                }
            }
        }
    }
}