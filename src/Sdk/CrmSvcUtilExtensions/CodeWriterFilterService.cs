namespace Aylos.Xrm.Sdk.CrmSvcUtilExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Xml.Linq;
    using Microsoft.Crm.Services.Utility;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Metadata;

    internal class CodeWriterFilterService : ICodeWriterFilterService
    {
        private IList<string> ActivityList { get; set; }

        private AttributeMetadata AttributeMetadata { get; set; }

        private ICodeWriterFilterService DefaultService { get; set; }

        private IList<string> EntityBlackList { get; set; }

        private IList<string> EntityWhiteList { get; set; }

        private IList<string> FieldBlackList { get; set; }

        private IList<string> OptionList { get; set; }

        private IList<string> OptionSetList { get; set; }

        private IList<string> RelationshipList { get; set; }

        public CodeWriterFilterService(ICodeWriterFilterService defaultService)
        {
            Console.WriteLine("Initializing CodeWriterFilterService.");

            ActivityList = ServiceHelper.ActivityLogicalNames();
            DefaultService = defaultService;
            EntityBlackList = new List<string>();
            EntityWhiteList = new List<string>();
            FieldBlackList = new List<string>();
            OptionList = new List<string>();
            OptionSetList = new List<string>();
            RelationshipList = new List<string>();

            if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.EntityBlackList))
            {
                string[] entities = Properties.Settings.Default.EntityBlackList.Split(",".ToCharArray());
                if (entities.Length > 0) EntityBlackList = entities.ToList();
            }

            if (!Properties.Settings.Default.GenerateAllEntities)
            {
                XElement xmlDocument = XElement.Load(Properties.Settings.Default.EntityWhiteListFilename);
                XElement entitiesElement = xmlDocument.Element("entities");

                if (entitiesElement == null) throw new InvalidProgramException("Entities XML file is missing the 'entities' element.");

                if (entitiesElement.Elements("entity").ToList().Count == 0) throw new InvalidProgramException("Entities XML file does not contain any entities.");

                bool entityWhiteListContainsActivity = false;

                foreach (XElement entityElement in entitiesElement.Elements("entity"))
                {
                    string entityName = Regex.Replace(entityElement.Value, @"[^a-zA-Z0-9_]", string.Empty).ToLowerInvariant();
                    if (!string.IsNullOrWhiteSpace(entityName)) EntityWhiteList.Add(entityName);
                    if (ActivityList.Contains(entityName)) entityWhiteListContainsActivity = true;
                }

                if (entityWhiteListContainsActivity)
                {
                    if (!EntityWhiteList.Contains(ServiceHelper.ActivityPartyLogicalName)) EntityWhiteList.Add(ServiceHelper.ActivityPartyLogicalName);
                    if (!EntityWhiteList.Contains(ServiceHelper.ActivityPointerLogicalName)) EntityWhiteList.Add(ServiceHelper.ActivityPointerLogicalName);
                }
            }

            if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.GlobalFieldBlackList))
            {
                string[] fields = Properties.Settings.Default.GlobalFieldBlackList.Split(",".ToCharArray());
                if (fields.Length > 0) FieldBlackList = fields.ToList();
            }

            // Debugging: Uncomment the below lines of code - run the generate batch file, set the break points and 
            // attach the debugger on CrmSvcUtil.exe process. Then press enter to start your debugging.
            //Console.WriteLine("CodeWriterFilterService attach debugger and press enter.");
            //Console.ReadLine();

            Console.WriteLine("CodeWriterFilterService initialized successfully.");
        }

        public bool GenerateAttribute(AttributeMetadata attributeMetadata, IServiceProvider services)
        {
            if (attributeMetadata == null) throw new ArgumentNullException(nameof(attributeMetadata));

            Console.WriteLine(string.Empty);
            Console.Write("Processing attribute '{0}'", attributeMetadata.LogicalName);

            AttributeMetadata = attributeMetadata;

            if (FieldBlackList.Contains(attributeMetadata.LogicalName))
            {
                Console.WriteLine(" ... ignored.");
                return false;
            }

            if (!string.IsNullOrWhiteSpace(attributeMetadata.AttributeOf))
            {
                Console.WriteLine(" ... ignored.");
                return false;
            }

            if (!attributeMetadata.IsValidForRead.GetValueOrDefault())
            {
                Console.WriteLine(" ... ignored.");
                return false;
            }

            EntityInfo entityInfo = ServiceHelper.EntityList.Single(x => x.EntityLogicalName == attributeMetadata.EntityLogicalName);

            string metadataName = ServiceHelper.GetAttributeName(attributeMetadata, entityInfo);

            if (string.IsNullOrWhiteSpace(metadataName))
            {
                Console.WriteLine(" ... ignored.");
                return false;
            }
            else
            {
                metadataName = entityInfo.AttributeNames.Where(x => x.Equals(metadataName, StringComparison.OrdinalIgnoreCase)).Any() ? attributeMetadata.SchemaName : metadataName;

                attributeMetadata.SchemaName = metadataName;

                entityInfo.AttributeNames.Add(metadataName);

                Console.WriteLine("... added as '{0}'.", metadataName);
                return true;
            }
        }

        public bool GenerateEntity(EntityMetadata entityMetadata, IServiceProvider services)
        {
            if (entityMetadata == null) throw new ArgumentNullException(nameof(entityMetadata));

            Console.WriteLine(string.Empty);
            Console.Write("Processing entity '{0}'", entityMetadata.LogicalName);

            if (EntityBlackList.Contains(entityMetadata.LogicalName))
            {
                Console.WriteLine(" ... ignored.");
                return false;
            }

            if (!Properties.Settings.Default.GenerateAllEntities && !EntityWhiteList.Contains(entityMetadata.LogicalName))
            {
                Console.WriteLine(" ... ignored.");
                return false;
            }

            string metadataName = null;

            if (entityMetadata.IsIntersect.GetValueOrDefault(true))
            {
                metadataName = ServiceHelper.TransformMetadataName(entityMetadata?.LogicalName);
            }
            else
            {
                metadataName = ServiceHelper.TransformMetadataName(entityMetadata?.DisplayName?.UserLocalizedLabel?.Label);
            }

            if (string.IsNullOrWhiteSpace(metadataName))
            {
                Console.WriteLine(" ... ignored.");
                return false;
            }
            else
            {
                entityMetadata.SchemaName = metadataName;

                EntityInfo entityInfo = ServiceHelper.EntityList.SingleOrDefault(x => x.EntityLogicalName == entityMetadata.LogicalName);
                if (entityInfo == null)
                {
                    entityInfo = new EntityInfo { EntityLogicalName = entityMetadata.LogicalName, EntityMetadata = entityMetadata };

                    ServiceHelper.EntityList.Add(entityInfo);

                    Console.WriteLine(" ... added as '{0}'.", metadataName);
                }
                else
                {
                    entityInfo.EntityMetadata = entityMetadata;

                    Console.WriteLine(" ... exist as '{0}'.", metadataName);
                }
                return true;
            }
        }

        public bool GenerateOption(OptionMetadata optionMetadata, IServiceProvider services)
        {
            if (optionMetadata == null) throw new ArgumentNullException(nameof(optionMetadata));

            Console.Write("Processing option '{0}'", optionMetadata?.Label?.UserLocalizedLabel?.Label);

            string metadataName = ServiceHelper.TransformMetadataName(optionMetadata?.Label?.UserLocalizedLabel?.Label);

            if (string.IsNullOrWhiteSpace(metadataName))
            {
                Console.WriteLine(" ... ignored.");
                return false;
            }
            else if (OptionList.Contains(metadataName))
            {
                Console.WriteLine(" ... ignored because '{0}' already exists in the option list.");
                return false;
            }
            else
            {
                Console.WriteLine(" ... added as '{0}'.", metadataName);

                OptionList.Add(metadataName);

                optionMetadata.Label.UserLocalizedLabel.Label = metadataName;

                foreach (LocalizedLabel ll in optionMetadata.Label.LocalizedLabels) ll.Label = metadataName;

                return true;
            }
        }

        public bool GenerateOptionSet(OptionSetMetadataBase optionSetMetadata, IServiceProvider services)
        {
            if (optionSetMetadata == null) throw new ArgumentNullException(nameof(optionSetMetadata));

            Console.WriteLine(string.Empty);
            Console.Write("Processing optionset '{0}'", optionSetMetadata.Name);

            OptionList.Clear();

            EntityInfo entityInfo = ServiceHelper.EntityList.SingleOrDefault(x => x.EntityLogicalName == AttributeMetadata?.EntityLogicalName);

            string metadataName = ServiceHelper.GetOptionSetName(entityInfo?.EntityMetadata, optionSetMetadata);

            if (string.IsNullOrWhiteSpace(metadataName))
            {
                Console.WriteLine(" ... ignored.");
                return false;
            }
            else if (OptionSetList.Contains(optionSetMetadata.Name))
            {
                Console.WriteLine(" ... ignored because '{0}' already exists in the optionset list.", optionSetMetadata.Name);
                return false;
            }
            else
            {
                OptionSetList.Add(optionSetMetadata.Name);

                optionSetMetadata.Name = metadataName;

                Console.WriteLine("... added as '{0}'.", metadataName);
                return true;
            }
        }

        public bool GenerateRelationship(RelationshipMetadataBase relationshipMetadata, EntityMetadata otherEntityMetadata, IServiceProvider services)
        {
            if (relationshipMetadata == null) throw new ArgumentNullException(nameof(relationshipMetadata));

            //Console.WriteLine("Added relationship '{0}'.", relationshipMetadata.SchemaName);
            //return _defaultService.GenerateRelationship(relationshipMetadata, otherEntityMetadata, services);
            return false; 
        }

        public bool GenerateServiceContext(IServiceProvider services)
        {
            Console.WriteLine("Added service context.");

            return DefaultService.GenerateServiceContext(services);
        }
    }
}
