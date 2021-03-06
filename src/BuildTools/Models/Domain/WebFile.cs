//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Aylos.Xrm.Sdk.BuildTools.Models.Domain
{
	using Microsoft.Xrm.Sdk;
	using Microsoft.Xrm.Sdk.Client;
	using System;
	using System.CodeDom.Compiler;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq;
	using System.Runtime.Serialization;
	
	
	/// <summary>
	/// Storage of files used in the web Portals.
	/// </summary>
	[DataContractAttribute()]
	[EntityLogicalNameAttribute("adx_webfile")]
	[GeneratedCodeAttribute("CrmSvcUtil", "9.0.0.9479")]
	public partial class WebFile : Entity, INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public WebFile() : 
				base(EntityLogicalName)
		{
		}
		
		public const string EntityLogicalName = "adx_webfile";
		
		public const int EntityTypeCode = 10179;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		private void OnPropertyChanged(string propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void OnPropertyChanging(string propertyName)
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
			}
		}
		
		/// <summary>
		/// Defines CORS header Access-Control-Allow-Origin for cross origin requests.
		/// </summary>
		[AttributeLogicalNameAttribute("adx_alloworigin")]
		public string AllowOrigin
		{
			get
			{
				return this.GetAttributeValue<string>("adx_alloworigin");
			}
			set
			{
				this.OnPropertyChanging("AllowOrigin");
				this.SetAttributeValue("adx_alloworigin", value);
				this.OnPropertyChanged("AllowOrigin");
			}
		}
		
		/// <summary>
		/// 
		/// </summary>
		[AttributeLogicalNameAttribute("adx_cloudblobaddress")]
		public string CloudBlobAddress
		{
			get
			{
				return this.GetAttributeValue<string>("adx_cloudblobaddress");
			}
			set
			{
				this.OnPropertyChanging("CloudBlobAddress");
				this.SetAttributeValue("adx_cloudblobaddress", value);
				this.OnPropertyChanged("CloudBlobAddress");
			}
		}
		
		/// <summary>
		/// Shows the value to be applied to the HTTP Response Headers Content-Disposition.
		/// </summary>
		[AttributeLogicalNameAttribute("adx_contentdisposition")]
		public Nullable<WebFileContentDisposition> ContentDisposition
		{
			get
			{
				var optionSet = this.GetAttributeValue<OptionSetValue>("adx_contentdisposition");
				if (optionSet == null)
				{
					return null;
				}
				else
				{
					return (WebFileContentDisposition)(Enum.ToObject(typeof(WebFileContentDisposition), optionSet.Value));
				}
			}
			set
			{
				this.OnPropertyChanging("ContentDisposition");
				if (value == null)
				{
					this.SetAttributeValue("adx_contentdisposition", null);
				}
				else
				{
					this.SetAttributeValue("adx_contentdisposition", new OptionSetValue((int)value));
				}
				this.OnPropertyChanged("ContentDisposition");
			}
		}
		
		/// <summary>
		/// 
		/// </summary>
		[AttributeLogicalNameAttribute("adx_createdbyipaddress")]
		public string CreatedByIPAddress
		{
			get
			{
				return this.GetAttributeValue<string>("adx_createdbyipaddress");
			}
			set
			{
				this.OnPropertyChanging("CreatedByIPAddress");
				this.SetAttributeValue("adx_createdbyipaddress", value);
				this.OnPropertyChanged("CreatedByIPAddress");
			}
		}
		
		/// <summary>
		/// 
		/// </summary>
		[AttributeLogicalNameAttribute("adx_createdbyusername")]
		public string CreatedByUsername
		{
			get
			{
				return this.GetAttributeValue<string>("adx_createdbyusername");
			}
			set
			{
				this.OnPropertyChanging("CreatedByUsername");
				this.SetAttributeValue("adx_createdbyusername", value);
				this.OnPropertyChanged("CreatedByUsername");
			}
		}
		
		/// <summary>
		/// 
		/// </summary>
		[AttributeLogicalNameAttribute("adx_displaydate")]
		public Nullable<DateTime> DisplayDate
		{
			get
			{
				return this.GetAttributeValue<Nullable<DateTime>>("adx_displaydate");
			}
			set
			{
				this.OnPropertyChanging("DisplayDate");
				this.SetAttributeValue("adx_displaydate", value);
				this.OnPropertyChanged("DisplayDate");
			}
		}
		
		/// <summary>
		/// 
		/// </summary>
		[AttributeLogicalNameAttribute("adx_displayorder")]
		public Nullable<int> DisplayOrder
		{
			get
			{
				return this.GetAttributeValue<Nullable<int>>("adx_displayorder");
			}
			set
			{
				this.OnPropertyChanging("DisplayOrder");
				this.SetAttributeValue("adx_displayorder", value);
				this.OnPropertyChanged("DisplayOrder");
			}
		}
		
		/// <summary>
		/// Select whether to enable logging of users' downloads of this web file.
		/// </summary>
		[AttributeLogicalNameAttribute("adx_enabletracking")]
		public Nullable<bool> EnableTrackingDeprecated
		{
			get
			{
				return this.GetAttributeValue<Nullable<bool>>("adx_enabletracking");
			}
			set
			{
				this.OnPropertyChanging("EnableTrackingDeprecated");
				this.SetAttributeValue("adx_enabletracking", value);
				this.OnPropertyChanged("EnableTrackingDeprecated");
			}
		}
		
		/// <summary>
		/// Shows whether the web file is excluded from the portal search.
		/// </summary>
		[AttributeLogicalNameAttribute("adx_excludefromsearch")]
		public Nullable<bool> ExcludeFromSearch
		{
			get
			{
				return this.GetAttributeValue<Nullable<bool>>("adx_excludefromsearch");
			}
			set
			{
				this.OnPropertyChanging("ExcludeFromSearch");
				this.SetAttributeValue("adx_excludefromsearch", value);
				this.OnPropertyChanged("ExcludeFromSearch");
			}
		}
		
		/// <summary>
		/// 
		/// </summary>
		[AttributeLogicalNameAttribute("adx_expirationdate")]
		public Nullable<DateTime> ExpirationDate
		{
			get
			{
				return this.GetAttributeValue<Nullable<DateTime>>("adx_expirationdate");
			}
			set
			{
				this.OnPropertyChanging("ExpirationDate");
				this.SetAttributeValue("adx_expirationdate", value);
				this.OnPropertyChanged("ExpirationDate");
			}
		}
		
		/// <summary>
		/// 
		/// </summary>
		[AttributeLogicalNameAttribute("adx_hiddenfromsitemap")]
		public Nullable<bool> HiddenFromSitemap
		{
			get
			{
				return this.GetAttributeValue<Nullable<bool>>("adx_hiddenfromsitemap");
			}
			set
			{
				this.OnPropertyChanging("HiddenFromSitemap");
				this.SetAttributeValue("adx_hiddenfromsitemap", value);
				this.OnPropertyChanged("HiddenFromSitemap");
			}
		}
		
		/// <summary>
		/// Unique identifier for Web File associated with Web File.
		/// </summary>
		[AttributeLogicalNameAttribute("adx_masterwebfileid")]
		public EntityReference MasterWebFile
		{
			get
			{
				return this.GetAttributeValue<EntityReference>("adx_masterwebfileid");
			}
			set
			{
				this.OnPropertyChanging("MasterWebFile");
				this.SetAttributeValue("adx_masterwebfileid", value);
				this.OnPropertyChanged("MasterWebFile");
			}
		}
		
		/// <summary>
		/// 
		/// </summary>
		[AttributeLogicalNameAttribute("adx_modifiedbyipaddress")]
		public string ModifiedByIPAddress
		{
			get
			{
				return this.GetAttributeValue<string>("adx_modifiedbyipaddress");
			}
			set
			{
				this.OnPropertyChanging("ModifiedByIPAddress");
				this.SetAttributeValue("adx_modifiedbyipaddress", value);
				this.OnPropertyChanged("ModifiedByIPAddress");
			}
		}
		
		/// <summary>
		/// 
		/// </summary>
		[AttributeLogicalNameAttribute("adx_modifiedbyusername")]
		public string ModifiedByUsername
		{
			get
			{
				return this.GetAttributeValue<string>("adx_modifiedbyusername");
			}
			set
			{
				this.OnPropertyChanging("ModifiedByUsername");
				this.SetAttributeValue("adx_modifiedbyusername", value);
				this.OnPropertyChanged("ModifiedByUsername");
			}
		}
		
		/// <summary>
		/// Shows the name of the custom entity.
		/// </summary>
		[AttributeLogicalNameAttribute("adx_name")]
		public string Name
		{
			get
			{
				return this.GetAttributeValue<string>("adx_name");
			}
			set
			{
				this.OnPropertyChanging("Name");
				this.SetAttributeValue("adx_name", value);
				this.OnPropertyChanged("Name");
			}
		}
		
		/// <summary>
		/// Unique identifier for Web Page associated with Web File.
		/// </summary>
		[AttributeLogicalNameAttribute("adx_parentpageid")]
		public EntityReference ParentPage
		{
			get
			{
				return this.GetAttributeValue<EntityReference>("adx_parentpageid");
			}
			set
			{
				this.OnPropertyChanging("ParentPage");
				this.SetAttributeValue("adx_parentpageid", value);
				this.OnPropertyChanged("ParentPage");
			}
		}
		
		/// <summary>
		/// 
		/// </summary>
		[AttributeLogicalNameAttribute("adx_partialurl")]
		public string PartialURL
		{
			get
			{
				return this.GetAttributeValue<string>("adx_partialurl");
			}
			set
			{
				this.OnPropertyChanging("PartialURL");
				this.SetAttributeValue("adx_partialurl", value);
				this.OnPropertyChanged("PartialURL");
			}
		}
		
		/// <summary>
		/// Unique identifier for Publishing State associated with Web File.
		/// </summary>
		[AttributeLogicalNameAttribute("adx_publishingstateid")]
		public EntityReference PublishingState
		{
			get
			{
				return this.GetAttributeValue<EntityReference>("adx_publishingstateid");
			}
			set
			{
				this.OnPropertyChanging("PublishingState");
				this.SetAttributeValue("adx_publishingstateid", value);
				this.OnPropertyChanged("PublishingState");
			}
		}
		
		/// <summary>
		/// 
		/// </summary>
		[AttributeLogicalNameAttribute("adx_releasedate")]
		public Nullable<DateTime> ReleaseDate
		{
			get
			{
				return this.GetAttributeValue<Nullable<DateTime>>("adx_releasedate");
			}
			set
			{
				this.OnPropertyChanging("ReleaseDate");
				this.SetAttributeValue("adx_releasedate", value);
				this.OnPropertyChanged("ReleaseDate");
			}
		}
		
		/// <summary>
		/// Unique identifier for Subject associated with Web File.
		/// </summary>
		[AttributeLogicalNameAttribute("adx_subjectid")]
		public EntityReference Subject
		{
			get
			{
				return this.GetAttributeValue<EntityReference>("adx_subjectid");
			}
			set
			{
				this.OnPropertyChanging("Subject");
				this.SetAttributeValue("adx_subjectid", value);
				this.OnPropertyChanged("Subject");
			}
		}
		
		/// <summary>
		/// 
		/// </summary>
		[AttributeLogicalNameAttribute("adx_summary")]
		public string Summary
		{
			get
			{
				return this.GetAttributeValue<string>("adx_summary");
			}
			set
			{
				this.OnPropertyChanging("Summary");
				this.SetAttributeValue("adx_summary", value);
				this.OnPropertyChanged("Summary");
			}
		}
		
		/// <summary>
		/// 
		/// </summary>
		[AttributeLogicalNameAttribute("adx_title")]
		public string Title
		{
			get
			{
				return this.GetAttributeValue<string>("adx_title");
			}
			set
			{
				this.OnPropertyChanging("Title");
				this.SetAttributeValue("adx_title", value);
				this.OnPropertyChanged("Title");
			}
		}
		
		/// <summary>
		/// Unique identifier for entity instances
		/// </summary>
		[AttributeLogicalNameAttribute("adx_webfileid")]
		public Nullable<Guid> WebFileId
		{
			get
			{
				return this.GetAttributeValue<Nullable<Guid>>("adx_webfileid");
			}
			set
			{
				this.OnPropertyChanging("WebFileId");
				this.SetAttributeValue("adx_webfileid", value);
				if (value.HasValue)
				{
					base.Id = value.Value;
				}
				else
				{
					base.Id = Guid.Empty;
				}
				this.OnPropertyChanged("WebFileId");
			}
		}
		
		[AttributeLogicalNameAttribute("adx_webfileid")]
		public override Guid Id
		{
			get
			{
				return base.Id;
			}
			set
			{
				this.WebFileId = value;
			}
		}
		
		/// <summary>
		/// Unique identifier for Website associated with Web File.
		/// </summary>
		[AttributeLogicalNameAttribute("adx_websiteid")]
		public EntityReference Website
		{
			get
			{
				return this.GetAttributeValue<EntityReference>("adx_websiteid");
			}
			set
			{
				this.OnPropertyChanging("Website");
				this.SetAttributeValue("adx_websiteid", value);
				this.OnPropertyChanged("Website");
			}
		}
		
		/// <summary>
		/// Shows the user who created the record.
		/// </summary>
		/// <remarks>
		/// The property is read-only and the setter has been added to assist with unit testing only.
		/// </remarks>
		[AttributeLogicalNameAttribute("createdby")]
		public EntityReference CreatedBy
		{
			get
			{
				return this.GetAttributeValue<EntityReference>("createdby");
			}
			set
			{
				this.OnPropertyChanging("CreatedBy");
				this.SetAttributeValue("createdby", value);
				this.OnPropertyChanged("CreatedBy");
			}
		}
		
		/// <summary>
		/// Shows when the record was created.
		/// </summary>
		/// <remarks>
		/// The property is read-only and the setter has been added to assist with unit testing only.
		/// </remarks>
		[AttributeLogicalNameAttribute("createdon")]
		public Nullable<DateTime> CreatedOn
		{
			get
			{
				return this.GetAttributeValue<Nullable<DateTime>>("createdon");
			}
			set
			{
				this.OnPropertyChanging("CreatedOn");
				this.SetAttributeValue("createdon", value);
				this.OnPropertyChanged("CreatedOn");
			}
		}
		
		/// <summary>
		/// Shows the user who updated the record.
		/// </summary>
		/// <remarks>
		/// The property is read-only and the setter has been added to assist with unit testing only.
		/// </remarks>
		[AttributeLogicalNameAttribute("modifiedby")]
		public EntityReference ModifiedBy
		{
			get
			{
				return this.GetAttributeValue<EntityReference>("modifiedby");
			}
			set
			{
				this.OnPropertyChanging("ModifiedBy");
				this.SetAttributeValue("modifiedby", value);
				this.OnPropertyChanged("ModifiedBy");
			}
		}
		
		/// <summary>
		/// Shows when the record was updated.
		/// </summary>
		/// <remarks>
		/// The property is read-only and the setter has been added to assist with unit testing only.
		/// </remarks>
		[AttributeLogicalNameAttribute("modifiedon")]
		public Nullable<DateTime> ModifiedOn
		{
			get
			{
				return this.GetAttributeValue<Nullable<DateTime>>("modifiedon");
			}
			set
			{
				this.OnPropertyChanging("ModifiedOn");
				this.SetAttributeValue("modifiedon", value);
				this.OnPropertyChanged("ModifiedOn");
			}
		}
		
		/// <summary>
		/// Shows the owner ID.
		/// </summary>
		[AttributeLogicalNameAttribute("ownerid")]
		public EntityReference Owner
		{
			get
			{
				return this.GetAttributeValue<EntityReference>("ownerid");
			}
			set
			{
				this.OnPropertyChanging("Owner");
				this.SetAttributeValue("ownerid", value);
				this.OnPropertyChanged("Owner");
			}
		}
		
		/// <summary>
		/// Unique identifier for the business unit that owns the record
		/// </summary>
		[AttributeLogicalNameAttribute("owningbusinessunit")]
		public EntityReference OwningBusinessUnit
		{
			get
			{
				return this.GetAttributeValue<EntityReference>("owningbusinessunit");
			}
		}
		
		/// <summary>
		/// Unique identifier for the team that owns the record.
		/// </summary>
		[AttributeLogicalNameAttribute("owningteam")]
		public EntityReference OwningTeam
		{
			get
			{
				return this.GetAttributeValue<EntityReference>("owningteam");
			}
		}
		
		/// <summary>
		/// Unique identifier for the user that owns the record.
		/// </summary>
		[AttributeLogicalNameAttribute("owninguser")]
		public EntityReference OwningUser
		{
			get
			{
				return this.GetAttributeValue<EntityReference>("owninguser");
			}
		}
		
		/// <summary>
		/// Status of the Web File
		/// </summary>
		[AttributeLogicalNameAttribute("statecode")]
		public Nullable<WebFileState> State
		{
			get
			{
				OptionSetValue optionSet = this.GetAttributeValue<OptionSetValue>("statecode");
				if (optionSet != null)
				{
					return ((WebFileState)(Enum.ToObject(typeof(WebFileState), optionSet.Value)));
				}
				else
				{
					return null;
				}
			}
			set
			{
				this.OnPropertyChanging("State");
				if (value == null)
				{
					this.SetAttributeValue("statecode", null);
				}
				else
				{
					this.SetAttributeValue("statecode", new OptionSetValue((int)value));
				}
				this.OnPropertyChanged("State");
			}
		}
		
		/// <summary>
		/// Reason for the status of the Web File
		/// </summary>
		[AttributeLogicalNameAttribute("statuscode")]
		public Nullable<WebFileStatusReason> StatusReason
		{
			get
			{
				OptionSetValue optionSet = this.GetAttributeValue<OptionSetValue>("statuscode");
				if (optionSet != null)
				{
					return ((WebFileStatusReason)(Enum.ToObject(typeof(WebFileStatusReason), optionSet.Value)));
				}
				else
				{
					return null;
				}
			}
			set
			{
				this.OnPropertyChanging("StatusReason");
				if (value == null)
				{
					this.SetAttributeValue("statuscode", null);
				}
				else
				{
					this.SetAttributeValue("statuscode", new OptionSetValue((int)value));
				}
				this.OnPropertyChanged("StatusReason");
			}
		}
		
		public struct Fields
		{
			
			public static string AllowOrigin = "adx_alloworigin";
			
			public static string CloudBlobAddress = "adx_cloudblobaddress";
			
			public static string ContentDisposition = "adx_contentdisposition";
			
			public static string CreatedByIPAddress = "adx_createdbyipaddress";
			
			public static string CreatedByUsername = "adx_createdbyusername";
			
			public static string DisplayDate = "adx_displaydate";
			
			public static string DisplayOrder = "adx_displayorder";
			
			public static string EnableTrackingDeprecated = "adx_enabletracking";
			
			public static string ExcludeFromSearch = "adx_excludefromsearch";
			
			public static string ExpirationDate = "adx_expirationdate";
			
			public static string HiddenFromSitemap = "adx_hiddenfromsitemap";
			
			public static string MasterWebFile = "adx_masterwebfileid";
			
			public static string ModifiedByIPAddress = "adx_modifiedbyipaddress";
			
			public static string ModifiedByUsername = "adx_modifiedbyusername";
			
			public static string Name = "adx_name";
			
			public static string ParentPage = "adx_parentpageid";
			
			public static string PartialURL = "adx_partialurl";
			
			public static string PublishingState = "adx_publishingstateid";
			
			public static string ReleaseDate = "adx_releasedate";
			
			public static string Subject = "adx_subjectid";
			
			public static string Summary = "adx_summary";
			
			public static string Title = "adx_title";
			
			public static string WebFileId = "adx_webfileid";
			
			public static string Id = "adx_webfileid";
			
			public static string Website = "adx_websiteid";
			
			public static string CreatedBy = "createdby";
			
			public static string CreatedOn = "createdon";
			
			public static string ModifiedBy = "modifiedby";
			
			public static string ModifiedOn = "modifiedon";
			
			public static string Owner = "ownerid";
			
			public static string OwningBusinessUnit = "owningbusinessunit";
			
			public static string OwningTeam = "owningteam";
			
			public static string OwningUser = "owninguser";
			
			public static string State = "statecode";
			
			public static string StatusReason = "statuscode";
		}
	}
}
