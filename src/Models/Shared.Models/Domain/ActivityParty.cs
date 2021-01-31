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
	/// Person or group associated with an activity. An activity can have multiple activity parties.
	/// </summary>
	[DataContractAttribute()]
	[EntityLogicalNameAttribute("activityparty")]
	[GeneratedCodeAttribute("CrmSvcUtil", "9.1.0.71")]
	public partial class ActivityParty : Entity, INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public ActivityParty() : 
				base(EntityLogicalName)
		{
		}
		
		public const string EntityLogicalName = "activityparty";
		
		public const string EntityLogicalCollectionName = "activityparties";
		
		public const string EntitySetName = "activityparties";
		
		public const int EntityTypeCode = 135;
		
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
		/// Unique identifier of the activity associated with the activity party. (A "party" is any person who is associated with an activity.)
		/// </summary>
		[AttributeLogicalNameAttribute("activityid")]
		public EntityReference Activity
		{
			get
			{
				return this.GetAttributeValue<EntityReference>("activityid");
			}
			set
			{
				this.OnPropertyChanging("Activity");
				this.SetAttributeValue("activityid", value);
				this.OnPropertyChanged("Activity");
			}
		}
		
		/// <summary>
		/// Unique identifier of the activity party.
		/// </summary>
		[AttributeLogicalNameAttribute("activitypartyid")]
		public Nullable<Guid> ActivityPartyId
		{
			get
			{
				return this.GetAttributeValue<Nullable<Guid>>("activitypartyid");
			}
			set
			{
				this.OnPropertyChanging("ActivityPartyId");
				this.SetAttributeValue("activitypartyid", value);
				if (value.HasValue)
				{
					base.Id = value.Value;
				}
				else
				{
					base.Id = Guid.Empty;
				}
				this.OnPropertyChanged("ActivityPartyId");
			}
		}
		
		[AttributeLogicalNameAttribute("activitypartyid")]
		public override Guid Id
		{
			get
			{
				return base.Id;
			}
			set
			{
				this.ActivityPartyId = value;
			}
		}
		
		/// <summary>
		/// Email address to which an email is delivered, and which is associated with the target entity.
		/// </summary>
		[AttributeLogicalNameAttribute("addressused")]
		public string Address
		{
			get
			{
				return this.GetAttributeValue<string>("addressused");
			}
			set
			{
				this.OnPropertyChanging("Address");
				this.SetAttributeValue("addressused", value);
				this.OnPropertyChanged("Address");
			}
		}
		
		/// <summary>
		/// Email address column number from associated party.
		/// </summary>
		[AttributeLogicalNameAttribute("addressusedemailcolumnnumber")]
		public Nullable<int> EmailColumnNumberOfParty
		{
			get
			{
				return this.GetAttributeValue<Nullable<int>>("addressusedemailcolumnnumber");
			}
		}
		
		/// <summary>
		/// Information about whether to allow sending email to the activity party.
		/// </summary>
		[AttributeLogicalNameAttribute("donotemail")]
		public Nullable<bool> DoNotAllowEmails
		{
			get
			{
				return this.GetAttributeValue<Nullable<bool>>("donotemail");
			}
		}
		
		/// <summary>
		/// Information about whether to allow sending faxes to the activity party.
		/// </summary>
		[AttributeLogicalNameAttribute("donotfax")]
		public Nullable<bool> DoNotAllowFaxes
		{
			get
			{
				return this.GetAttributeValue<Nullable<bool>>("donotfax");
			}
		}
		
		/// <summary>
		/// Information about whether to allow phone calls to the lead.
		/// </summary>
		[AttributeLogicalNameAttribute("donotphone")]
		public Nullable<bool> DoNotAllowPhoneCalls
		{
			get
			{
				return this.GetAttributeValue<Nullable<bool>>("donotphone");
			}
		}
		
		/// <summary>
		/// Information about whether to allow sending postal mail to the lead.
		/// </summary>
		[AttributeLogicalNameAttribute("donotpostalmail")]
		public Nullable<bool> DoNotAllowPostalMails
		{
			get
			{
				return this.GetAttributeValue<Nullable<bool>>("donotpostalmail");
			}
		}
		
		/// <summary>
		/// Amount of effort used by the resource in a service appointment activity.
		/// </summary>
		[AttributeLogicalNameAttribute("effort")]
		public Nullable<double> Effort
		{
			get
			{
				return this.GetAttributeValue<Nullable<double>>("effort");
			}
			set
			{
				this.OnPropertyChanging("Effort");
				this.SetAttributeValue("effort", value);
				this.OnPropertyChanged("Effort");
			}
		}
		
		/// <summary>
		/// For internal use only.
		/// </summary>
		[AttributeLogicalNameAttribute("exchangeentryid")]
		public string ExchangeEntry
		{
			get
			{
				return this.GetAttributeValue<string>("exchangeentryid");
			}
			set
			{
				this.OnPropertyChanging("ExchangeEntry");
				this.SetAttributeValue("exchangeentryid", value);
				this.OnPropertyChanged("ExchangeEntry");
			}
		}
		
		/// <summary>
		/// Type of instance of a recurring series.
		/// </summary>
		[AttributeLogicalNameAttribute("instancetypecode")]
		public Nullable<ActivityPartyAppointmentType> AppointmentType
		{
			get
			{
				var optionSet = this.GetAttributeValue<OptionSetValue>("instancetypecode");
				if (optionSet == null)
				{
					return null;
				}
				else
				{
					return (ActivityPartyAppointmentType)(Enum.ToObject(typeof(ActivityPartyAppointmentType), optionSet.Value));
				}
			}
		}
		
		/// <summary>
		/// Information about whether the underlying entity record is deleted.
		/// </summary>
		[AttributeLogicalNameAttribute("ispartydeleted")]
		public Nullable<bool> IsPartyDeleted
		{
			get
			{
				return this.GetAttributeValue<Nullable<bool>>("ispartydeleted");
			}
		}
		
		/// <summary>
		/// Unique identifier of the user or team who owns the activity_party.
		/// </summary>
		[AttributeLogicalNameAttribute("ownerid")]
		public EntityReference Owner
		{
			get
			{
				return this.GetAttributeValue<EntityReference>("ownerid");
			}
		}
		
		/// <summary>
		/// Role of the person in the activity, such as sender, to, cc, bcc, required, optional, organizer, regarding, or owner.
		/// </summary>
		[AttributeLogicalNameAttribute("participationtypemask")]
		public Nullable<ActivityPartyParticipationType> ParticipationType
		{
			get
			{
				var optionSet = this.GetAttributeValue<OptionSetValue>("participationtypemask");
				if (optionSet == null)
				{
					return null;
				}
				else
				{
					return (ActivityPartyParticipationType)(Enum.ToObject(typeof(ActivityPartyParticipationType), optionSet.Value));
				}
			}
			set
			{
				this.OnPropertyChanging("ParticipationType");
				if (value == null)
				{
					this.SetAttributeValue("participationtypemask", null);
				}
				else
				{
					this.SetAttributeValue("participationtypemask", new OptionSetValue((int)value));
				}
				this.OnPropertyChanged("ParticipationType");
			}
		}
		
		/// <summary>
		/// Unique identifier of the party associated with the activity.
		/// </summary>
		[AttributeLogicalNameAttribute("partyid")]
		public EntityReference Party
		{
			get
			{
				return this.GetAttributeValue<EntityReference>("partyid");
			}
			set
			{
				this.OnPropertyChanging("Party");
				this.SetAttributeValue("partyid", value);
				this.OnPropertyChanged("Party");
			}
		}
		
		/// <summary>
		/// Scheduled end time of the activity.
		/// </summary>
		[AttributeLogicalNameAttribute("scheduledend")]
		public Nullable<DateTime> ScheduledEnd
		{
			get
			{
				return this.GetAttributeValue<Nullable<DateTime>>("scheduledend");
			}
		}
		
		/// <summary>
		/// Scheduled start time of the activity.
		/// </summary>
		[AttributeLogicalNameAttribute("scheduledstart")]
		public Nullable<DateTime> ScheduledStart
		{
			get
			{
				return this.GetAttributeValue<Nullable<DateTime>>("scheduledstart");
			}
		}
		
		public struct Fields
		{
			
			public static string Activity = "activityid";
			
			public static string ActivityPartyId = "activitypartyid";
			
			public static string Id = "activitypartyid";
			
			public static string Address = "addressused";
			
			public static string EmailColumnNumberOfParty = "addressusedemailcolumnnumber";
			
			public static string DoNotAllowEmails = "donotemail";
			
			public static string DoNotAllowFaxes = "donotfax";
			
			public static string DoNotAllowPhoneCalls = "donotphone";
			
			public static string DoNotAllowPostalMails = "donotpostalmail";
			
			public static string Effort = "effort";
			
			public static string ExchangeEntry = "exchangeentryid";
			
			public static string AppointmentType = "instancetypecode";
			
			public static string IsPartyDeleted = "ispartydeleted";
			
			public static string Owner = "ownerid";
			
			public static string ParticipationType = "participationtypemask";
			
			public static string Party = "partyid";
			
			public static string ScheduledEnd = "scheduledend";
			
			public static string ScheduledStart = "scheduledstart";
		}
	}
}
