﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Aylos.Xrm.Sdk.CrmSvcUtilExtensions.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.8.1.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"appointment,bulkoperation,campaignactivity,campaignresponse,email,fax,incidentresolution,letter,opportunityclose,orderclose,phonecall,quoteclose,recurringappointmentmaster,serviceappointment,socialactivity,task,untrackedemail
appointment,bulkoperation,campaignactivity,campaignresponse,email,fax,incidentresolution,letter,opportunityclose,orderclose,phonecall,quoteclose,recurringappointmentmaster,serviceappointment,socialactivity,task,untrackedemail
")]
        public string ActivityLogicalNames {
            get {
                return ((string)(this["ActivityLogicalNames"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("knowledgearticleincident,knowledgearticleviews,languagelocale,officedocument,pers" +
            "onaldocumenttemplate,rollupjob,socialinsightsconfiguration")]
        public string EntityBlackList {
            get {
                return ((string)(this["EntityBlackList"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Entities.xml")]
        public string EntityWhiteListFilename {
            get {
                return ((string)(this["EntityWhiteListFilename"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Fields")]
        public string FieldStructName {
            get {
                return ((string)(this["FieldStructName"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("CrmServiceContext.cs")]
        public string Filename {
            get {
                return ((string)(this["Filename"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool GenerateAllEntities {
            get {
                return ((bool)(this["GenerateAllEntities"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool GenerateFieldStructures {
            get {
                return ((bool)(this["GenerateFieldStructures"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool GenerateOnlyReferencedGlobalOptionSets {
            get {
                return ((bool)(this["GenerateOnlyReferencedGlobalOptionSets"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool GenerateSeparateFilesPerType {
            get {
                return ((bool)(this["GenerateSeparateFilesPerType"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("createdby,modifiedby,createdon,modifiedon,")]
        public string GenerateSettersFor {
            get {
                return ((string)(this["GenerateSettersFor"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("createdonbehalfby,importsequencenumber,modifiedonbehalfby,overriddencreatedon,tim" +
            "ezoneruleversionnumber,utcconversiontimezonecode,versionnumber,entitylogicalname" +
            ",entitytypecode")]
        public string GlobalFieldBlackList {
            get {
                return ((string)(this["GlobalFieldBlackList"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Global")]
        public string GlobalOptionSetPrefix {
            get {
                return ((string)(this["GlobalOptionSetPrefix"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("100")]
        public int MetadataMaxCharacterCount {
            get {
                return ((int)(this["MetadataMaxCharacterCount"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("..\\..\\..\\Models\\Shared.Models\\Domain")]
        public string OutputPath {
            get {
                return ((string)(this["OutputPath"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Id")]
        public string PrimaryKeySuffix {
            get {
                return ((string)(this["PrimaryKeySuffix"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("[^a-zA-Z0-9\\s]")]
        public string RegexAllowedCharacters {
            get {
                return ((string)(this["RegexAllowedCharacters"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool ReplaceDigits {
            get {
                return ((bool)(this["ReplaceDigits"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("State")]
        public string StateCodeSuffix {
            get {
                return ((string)(this["StateCodeSuffix"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("StatusReason")]
        public string StatusReasonSuffix {
            get {
                return ((string)(this["StatusReasonSuffix"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("aylos_")]
        public string TextToRemove {
            get {
                return ((string)(this["TextToRemove"]));
            }
        }
    }
}
