//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Shared.Models.Domain
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
	/// Note that is attached to one or more objects, including other notes.
	/// </summary>
	[DataContractAttribute()]
	[EntityLogicalNameAttribute("annotation")]
	[GeneratedCodeAttribute("CrmSvcUtil", "9.1.0.95")]
	public partial class Note : Entity, INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public Note() : 
				base(EntityLogicalName)
		{
		}
		
		public const string EntityLogicalName = "annotation";
		
		public const string EntityLogicalCollectionName = "annotations";
		
		public const string EntitySetName = "annotations";
		
		public const int EntityTypeCode = 5;
		
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
		/// Unique identifier of the note.
		/// </summary>
		[AttributeLogicalNameAttribute("annotationid")]
		public Nullable<Guid> AnnotationId
		{
			get
			{
				return this.GetAttributeValue<Nullable<Guid>>("annotationid");
			}
			set
			{
				this.OnPropertyChanging("AnnotationId");
				this.SetAttributeValue("annotationid", value);
				if (value.HasValue)
				{
					base.Id = value.Value;
				}
				else
				{
					base.Id = Guid.Empty;
				}
				this.OnPropertyChanged("AnnotationId");
			}
		}
		
		[AttributeLogicalNameAttribute("annotationid")]
		public override Guid Id
		{
			get
			{
				return base.Id;
			}
			set
			{
				this.AnnotationId = value;
			}
		}
		
		/// <summary>
		/// Unique identifier of the user who created the note.
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
		/// Date and time when the note was created.
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
		/// Contents of the note's attachment.
		/// </summary>
		[AttributeLogicalNameAttribute("documentbody")]
		public string Document
		{
			get
			{
				return this.GetAttributeValue<string>("documentbody");
			}
			set
			{
				this.OnPropertyChanging("Document");
				this.SetAttributeValue("documentbody", value);
				this.OnPropertyChanged("Document");
			}
		}
		
		/// <summary>
		/// File name of the note.
		/// </summary>
		[AttributeLogicalNameAttribute("filename")]
		public string FileName
		{
			get
			{
				return this.GetAttributeValue<string>("filename");
			}
			set
			{
				this.OnPropertyChanging("FileName");
				this.SetAttributeValue("filename", value);
				this.OnPropertyChanged("FileName");
			}
		}
		
		/// <summary>
		/// File size of the note.
		/// </summary>
		[AttributeLogicalNameAttribute("filesize")]
		public Nullable<int> FileSizeBytes
		{
			get
			{
				return this.GetAttributeValue<Nullable<int>>("filesize");
			}
		}
		
		/// <summary>
		/// Specifies whether the note is an attachment.
		/// </summary>
		[AttributeLogicalNameAttribute("isdocument")]
		public Nullable<bool> IsDocument
		{
			get
			{
				return this.GetAttributeValue<Nullable<bool>>("isdocument");
			}
			set
			{
				this.OnPropertyChanging("IsDocument");
				this.SetAttributeValue("isdocument", value);
				this.OnPropertyChanged("IsDocument");
			}
		}
		
		/// <summary>
		/// Language identifier for the note.
		/// </summary>
		[AttributeLogicalNameAttribute("langid")]
		public string LanguageID
		{
			get
			{
				return this.GetAttributeValue<string>("langid");
			}
			set
			{
				this.OnPropertyChanging("LanguageID");
				this.SetAttributeValue("langid", value);
				this.OnPropertyChanged("LanguageID");
			}
		}
		
		/// <summary>
		/// MIME type of the note's attachment.
		/// </summary>
		[AttributeLogicalNameAttribute("mimetype")]
		public string MimeType
		{
			get
			{
				return this.GetAttributeValue<string>("mimetype");
			}
			set
			{
				this.OnPropertyChanging("MimeType");
				this.SetAttributeValue("mimetype", value);
				this.OnPropertyChanged("MimeType");
			}
		}
		
		/// <summary>
		/// Unique identifier of the user who last modified the note.
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
		/// Date and time when the note was last modified.
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
		/// Text of the note.
		/// </summary>
		[AttributeLogicalNameAttribute("notetext")]
		public string Description
		{
			get
			{
				return this.GetAttributeValue<string>("notetext");
			}
			set
			{
				this.OnPropertyChanging("Description");
				this.SetAttributeValue("notetext", value);
				this.OnPropertyChanged("Description");
			}
		}
		
		/// <summary>
		/// Unique identifier of the object with which the note is associated.
		/// </summary>
		[AttributeLogicalNameAttribute("objectid")]
		public EntityReference Regarding
		{
			get
			{
				return this.GetAttributeValue<EntityReference>("objectid");
			}
			set
			{
				this.OnPropertyChanging("Regarding");
				this.SetAttributeValue("objectid", value);
				this.OnPropertyChanged("Regarding");
			}
		}
		
		/// <summary>
		/// Type of entity with which the note is associated.
		/// </summary>
		[AttributeLogicalNameAttribute("objecttypecode")]
		public string ObjectType
		{
			get
			{
				return this.GetAttributeValue<string>("objecttypecode");
			}
			set
			{
				this.OnPropertyChanging("ObjectType");
				this.SetAttributeValue("objecttypecode", value);
				this.OnPropertyChanged("ObjectType");
			}
		}
		
		/// <summary>
		/// Unique identifier of the user or team who owns the note.
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
		/// Unique identifier of the business unit that owns the note.
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
		/// Unique identifier of the team who owns the note.
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
		/// Unique identifier of the user who owns the note.
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
		/// Prefix of the file pointer in blob storage.
		/// </summary>
		[AttributeLogicalNameAttribute("prefix")]
		public string Prefix
		{
			get
			{
				return this.GetAttributeValue<string>("prefix");
			}
		}
		
		/// <summary>
		/// workflow step id associated with the note.
		/// </summary>
		[AttributeLogicalNameAttribute("stepid")]
		public string StepId
		{
			get
			{
				return this.GetAttributeValue<string>("stepid");
			}
			set
			{
				this.OnPropertyChanging("StepId");
				this.SetAttributeValue("stepid", value);
				this.OnPropertyChanged("StepId");
			}
		}
		
		/// <summary>
		/// Subject associated with the note.
		/// </summary>
		[AttributeLogicalNameAttribute("subject")]
		public string Title
		{
			get
			{
				return this.GetAttributeValue<string>("subject");
			}
			set
			{
				this.OnPropertyChanging("Title");
				this.SetAttributeValue("subject", value);
				this.OnPropertyChanged("Title");
			}
		}
		
		public struct Fields
		{
			
			public static string AnnotationId = "annotationid";
			
			public static string Id = "annotationid";
			
			public static string CreatedBy = "createdby";
			
			public static string CreatedOn = "createdon";
			
			public static string Document = "documentbody";
			
			public static string FileName = "filename";
			
			public static string FileSizeBytes = "filesize";
			
			public static string IsDocument = "isdocument";
			
			public static string LanguageID = "langid";
			
			public static string MimeType = "mimetype";
			
			public static string ModifiedBy = "modifiedby";
			
			public static string ModifiedOn = "modifiedon";
			
			public static string Description = "notetext";
			
			public static string Regarding = "objectid";
			
			public static string ObjectType = "objecttypecode";
			
			public static string Owner = "ownerid";
			
			public static string OwningBusinessUnit = "owningbusinessunit";
			
			public static string OwningTeam = "owningteam";
			
			public static string OwningUser = "owninguser";
			
			public static string Prefix = "prefix";
			
			public static string StepId = "stepid";
			
			public static string Title = "subject";
		}
	}
}