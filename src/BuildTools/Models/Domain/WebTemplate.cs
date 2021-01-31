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
	/// 
	/// </summary>
	[DataContractAttribute()]
	[EntityLogicalNameAttribute("adx_webtemplate")]
	[GeneratedCodeAttribute("CrmSvcUtil", "9.0.0.9479")]
	public partial class WebTemplate : Entity, INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public WebTemplate() : 
				base(EntityLogicalName)
		{
		}
		
		public const string EntityLogicalName = "adx_webtemplate";
		
		public const int EntityTypeCode = 10217;
		
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
		/// Shows the MIME type of the web template content.
		/// </summary>
		[AttributeLogicalNameAttribute("adx_mimetype")]
		public string MIMEType
		{
			get
			{
				return this.GetAttributeValue<string>("adx_mimetype");
			}
			set
			{
				this.OnPropertyChanging("MIMEType");
				this.SetAttributeValue("adx_mimetype", value);
				this.OnPropertyChanged("MIMEType");
			}
		}
		
		/// <summary>
		/// Type the name of the custom entity.
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
		/// 
		/// </summary>
		[AttributeLogicalNameAttribute("adx_source")]
		public string Source
		{
			get
			{
				return this.GetAttributeValue<string>("adx_source");
			}
			set
			{
				this.OnPropertyChanging("Source");
				this.SetAttributeValue("adx_source", value);
				this.OnPropertyChanged("Source");
			}
		}
		
		/// <summary>
		/// Unique identifier for Website associated with Web Template
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
		/// Shows the entity instance.
		/// </summary>
		[AttributeLogicalNameAttribute("adx_webtemplateid")]
		public Nullable<Guid> WebTemplateId
		{
			get
			{
				return this.GetAttributeValue<Nullable<Guid>>("adx_webtemplateid");
			}
			set
			{
				this.OnPropertyChanging("WebTemplateId");
				this.SetAttributeValue("adx_webtemplateid", value);
				if (value.HasValue)
				{
					base.Id = value.Value;
				}
				else
				{
					base.Id = Guid.Empty;
				}
				this.OnPropertyChanged("WebTemplateId");
			}
		}
		
		[AttributeLogicalNameAttribute("adx_webtemplateid")]
		public override Guid Id
		{
			get
			{
				return base.Id;
			}
			set
			{
				this.WebTemplateId = value;
			}
		}
		
		/// <summary>
		/// Shows who created the record.
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
		/// Shows the date and time when the record was created.
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
		/// Shows who last updated the record.
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
		/// Shows the date and time when the record was modified.
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
		/// Shows the organization.
		/// </summary>
		[AttributeLogicalNameAttribute("organizationid")]
		public EntityReference ShowsTheOrganization
		{
			get
			{
				return this.GetAttributeValue<EntityReference>("organizationid");
			}
		}
		
		/// <summary>
		/// Status of the Web Template
		/// </summary>
		[AttributeLogicalNameAttribute("statecode")]
		public Nullable<WebTemplateState> State
		{
			get
			{
				OptionSetValue optionSet = this.GetAttributeValue<OptionSetValue>("statecode");
				if (optionSet != null)
				{
					return ((WebTemplateState)(Enum.ToObject(typeof(WebTemplateState), optionSet.Value)));
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
		/// Select the web template's status.
		/// </summary>
		[AttributeLogicalNameAttribute("statuscode")]
		public Nullable<WebTemplateStatusReason> StatusReason
		{
			get
			{
				OptionSetValue optionSet = this.GetAttributeValue<OptionSetValue>("statuscode");
				if (optionSet != null)
				{
					return ((WebTemplateStatusReason)(Enum.ToObject(typeof(WebTemplateStatusReason), optionSet.Value)));
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
			
			public static string MIMEType = "adx_mimetype";
			
			public static string Name = "adx_name";
			
			public static string Source = "adx_source";
			
			public static string Website = "adx_websiteid";
			
			public static string WebTemplateId = "adx_webtemplateid";
			
			public static string Id = "adx_webtemplateid";
			
			public static string CreatedBy = "createdby";
			
			public static string CreatedOn = "createdon";
			
			public static string ModifiedBy = "modifiedby";
			
			public static string ModifiedOn = "modifiedon";
			
			public static string ShowsTheOrganization = "organizationid";
			
			public static string State = "statecode";
			
			public static string StatusReason = "statuscode";
		}
	}
}
