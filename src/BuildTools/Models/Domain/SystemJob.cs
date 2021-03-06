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
	/// Process whose execution can proceed independently or in the background.
	/// </summary>
	[DataContractAttribute()]
	[EntityLogicalNameAttribute("asyncoperation")]
	[GeneratedCodeAttribute("CrmSvcUtil", "9.1.0.25")]
	public partial class SystemJob : Entity, INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public SystemJob() : 
				base(EntityLogicalName)
		{
		}
		
		public const string EntityLogicalName = "asyncoperation";
		
		public const int EntityTypeCode = 4700;
		
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
		/// Unique identifier of the system job.
		/// </summary>
		[AttributeLogicalNameAttribute("asyncoperationid")]
		public Nullable<Guid> AsyncOperationId
		{
			get
			{
				return this.GetAttributeValue<Nullable<Guid>>("asyncoperationid");
			}
			set
			{
				this.OnPropertyChanging("AsyncOperationId");
				this.SetAttributeValue("asyncoperationid", value);
				if (value.HasValue)
				{
					base.Id = value.Value;
				}
				else
				{
					base.Id = Guid.Empty;
				}
				this.OnPropertyChanged("AsyncOperationId");
			}
		}
		
		[AttributeLogicalNameAttribute("asyncoperationid")]
		public override Guid Id
		{
			get
			{
				return base.Id;
			}
			set
			{
				this.AsyncOperationId = value;
			}
		}
		
		/// <summary>
		/// The breadcrumb record ID.
		/// </summary>
		[AttributeLogicalNameAttribute("breadcrumbid")]
		public Nullable<Guid> BreadcrumbID
		{
			get
			{
				return this.GetAttributeValue<Nullable<Guid>>("breadcrumbid");
			}
			set
			{
				this.OnPropertyChanging("BreadcrumbID");
				this.SetAttributeValue("breadcrumbid", value);
				this.OnPropertyChanged("BreadcrumbID");
			}
		}
		
		/// <summary>
		/// The origin of the caller.
		/// </summary>
		[AttributeLogicalNameAttribute("callerorigin")]
		public string CallerOrigin
		{
			get
			{
				return this.GetAttributeValue<string>("callerorigin");
			}
			set
			{
				this.OnPropertyChanging("CallerOrigin");
				this.SetAttributeValue("callerorigin", value);
				this.OnPropertyChanged("CallerOrigin");
			}
		}
		
		/// <summary>
		/// Date and time when the system job was completed.
		/// </summary>
		[AttributeLogicalNameAttribute("completedon")]
		public Nullable<DateTime> CompletedOn
		{
			get
			{
				return this.GetAttributeValue<Nullable<DateTime>>("completedon");
			}
		}
		
		/// <summary>
		/// Unique identifier used to correlate between multiple SDK requests and system jobs.
		/// </summary>
		[AttributeLogicalNameAttribute("correlationid")]
		public Nullable<Guid> CorrelationId
		{
			get
			{
				return this.GetAttributeValue<Nullable<Guid>>("correlationid");
			}
			set
			{
				this.OnPropertyChanging("CorrelationId");
				this.SetAttributeValue("correlationid", value);
				this.OnPropertyChanged("CorrelationId");
			}
		}
		
		/// <summary>
		/// Last time the correlation depth was updated.
		/// </summary>
		[AttributeLogicalNameAttribute("correlationupdatedtime")]
		public Nullable<DateTime> CorrelationUpdatedTime
		{
			get
			{
				return this.GetAttributeValue<Nullable<DateTime>>("correlationupdatedtime");
			}
			set
			{
				this.OnPropertyChanging("CorrelationUpdatedTime");
				this.SetAttributeValue("correlationupdatedtime", value);
				this.OnPropertyChanged("CorrelationUpdatedTime");
			}
		}
		
		/// <summary>
		/// Unique identifier of the user who created the system job.
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
		/// Date and time when the system job was created.
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
		/// Unstructured data associated with the system job.
		/// </summary>
		[AttributeLogicalNameAttribute("data")]
		public string Data
		{
			get
			{
				return this.GetAttributeValue<string>("data");
			}
			set
			{
				this.OnPropertyChanging("Data");
				this.SetAttributeValue("data", value);
				this.OnPropertyChanged("Data");
			}
		}
		
		/// <summary>
		/// Execution of all operations with the same dependency token is serialized.
		/// </summary>
		[AttributeLogicalNameAttribute("dependencytoken")]
		public string DependencyToken
		{
			get
			{
				return this.GetAttributeValue<string>("dependencytoken");
			}
			set
			{
				this.OnPropertyChanging("DependencyToken");
				this.SetAttributeValue("dependencytoken", value);
				this.OnPropertyChanged("DependencyToken");
			}
		}
		
		/// <summary>
		/// Number of SDK calls made since the first call.
		/// </summary>
		[AttributeLogicalNameAttribute("depth")]
		public Nullable<int> Depth
		{
			get
			{
				return this.GetAttributeValue<Nullable<int>>("depth");
			}
			set
			{
				this.OnPropertyChanging("Depth");
				this.SetAttributeValue("depth", value);
				this.OnPropertyChanged("Depth");
			}
		}
		
		/// <summary>
		/// Error code returned from a canceled system job.
		/// </summary>
		[AttributeLogicalNameAttribute("errorcode")]
		public Nullable<int> ErrorCode
		{
			get
			{
				return this.GetAttributeValue<Nullable<int>>("errorcode");
			}
		}
		
		/// <summary>
		/// Time that the system job has taken to execute.
		/// </summary>
		[AttributeLogicalNameAttribute("executiontimespan")]
		public Nullable<double> ExecutionTimeSpan
		{
			get
			{
				return this.GetAttributeValue<Nullable<double>>("executiontimespan");
			}
		}
		
		/// <summary>
		/// The datetime when the Expander pipeline started.
		/// </summary>
		[AttributeLogicalNameAttribute("expanderstarttime")]
		public Nullable<DateTime> ExpanderStartTime
		{
			get
			{
				return this.GetAttributeValue<Nullable<DateTime>>("expanderstarttime");
			}
			set
			{
				this.OnPropertyChanging("ExpanderStartTime");
				this.SetAttributeValue("expanderstarttime", value);
				this.OnPropertyChanged("ExpanderStartTime");
			}
		}
		
		/// <summary>
		/// Message provided by the system job.
		/// </summary>
		[AttributeLogicalNameAttribute("friendlymessage")]
		public string FriendlyMessage
		{
			get
			{
				return this.GetAttributeValue<string>("friendlymessage");
			}
			set
			{
				this.OnPropertyChanging("FriendlyMessage");
				this.SetAttributeValue("friendlymessage", value);
				this.OnPropertyChanged("FriendlyMessage");
			}
		}
		
		/// <summary>
		/// Unique identifier of the host that owns this system job.
		/// </summary>
		[AttributeLogicalNameAttribute("hostid")]
		public string Host
		{
			get
			{
				return this.GetAttributeValue<string>("hostid");
			}
			set
			{
				this.OnPropertyChanging("Host");
				this.SetAttributeValue("hostid", value);
				this.OnPropertyChanged("Host");
			}
		}
		
		/// <summary>
		/// Indicates that the system job is waiting for an event.
		/// </summary>
		[AttributeLogicalNameAttribute("iswaitingforevent")]
		public Nullable<bool> WaitingForEvent
		{
			get
			{
				return this.GetAttributeValue<Nullable<bool>>("iswaitingforevent");
			}
		}
		
		/// <summary>
		/// Message related to the system job.
		/// </summary>
		[AttributeLogicalNameAttribute("message")]
		public string Message
		{
			get
			{
				return this.GetAttributeValue<string>("message");
			}
		}
		
		/// <summary>
		/// Name of the message that started this system job.
		/// </summary>
		[AttributeLogicalNameAttribute("messagename")]
		public string MessageName
		{
			get
			{
				return this.GetAttributeValue<string>("messagename");
			}
			set
			{
				this.OnPropertyChanging("MessageName");
				this.SetAttributeValue("messagename", value);
				this.OnPropertyChanged("MessageName");
			}
		}
		
		/// <summary>
		/// Unique identifier of the user who last modified the system job.
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
		/// Date and time when the system job was last modified.
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
		/// Name of the system job.
		/// </summary>
		[AttributeLogicalNameAttribute("name")]
		public string SystemJobName
		{
			get
			{
				return this.GetAttributeValue<string>("name");
			}
			set
			{
				this.OnPropertyChanging("SystemJobName");
				this.SetAttributeValue("name", value);
				this.OnPropertyChanged("SystemJobName");
			}
		}
		
		/// <summary>
		/// Type of the system job.
		/// </summary>
		[AttributeLogicalNameAttribute("operationtype")]
		public Nullable<SystemJobSystemJobType> SystemJobType
		{
			get
			{
				var optionSet = this.GetAttributeValue<OptionSetValue>("operationtype");
				if (optionSet == null)
				{
					return null;
				}
				else
				{
					return (SystemJobSystemJobType)(Enum.ToObject(typeof(SystemJobSystemJobType), optionSet.Value));
				}
			}
			set
			{
				this.OnPropertyChanging("SystemJobType");
				if (value == null)
				{
					this.SetAttributeValue("operationtype", null);
				}
				else
				{
					this.SetAttributeValue("operationtype", new OptionSetValue((int)value));
				}
				this.OnPropertyChanged("SystemJobType");
			}
		}
		
		/// <summary>
		/// Unique identifier of the user or team who owns the system job.
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
		/// Unique identifier of the business unit that owns the system job.
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
		/// Unique identifier of the owning extension with which the system job is associated.
		/// </summary>
		[AttributeLogicalNameAttribute("owningextensionid")]
		public EntityReference OwningExtension
		{
			get
			{
				return this.GetAttributeValue<EntityReference>("owningextensionid");
			}
			set
			{
				this.OnPropertyChanging("OwningExtension");
				this.SetAttributeValue("owningextensionid", value);
				this.OnPropertyChanged("OwningExtension");
			}
		}
		
		/// <summary>
		/// Unique identifier of the team who owns the record.
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
		/// Unique identifier of the user who owns the record.
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
		/// 
		/// </summary>
		[AttributeLogicalNameAttribute("parentpluginexecutionid")]
		public Nullable<Guid> ParentPluginExecutionId
		{
			get
			{
				return this.GetAttributeValue<Nullable<Guid>>("parentpluginexecutionid");
			}
			set
			{
				this.OnPropertyChanging("ParentPluginExecutionId");
				this.SetAttributeValue("parentpluginexecutionid", value);
				this.OnPropertyChanged("ParentPluginExecutionId");
			}
		}
		
		/// <summary>
		/// Indicates whether the system job should run only after the specified date and time.
		/// </summary>
		[AttributeLogicalNameAttribute("postponeuntil")]
		public Nullable<DateTime> PostponeUntil
		{
			get
			{
				return this.GetAttributeValue<Nullable<DateTime>>("postponeuntil");
			}
			set
			{
				this.OnPropertyChanging("PostponeUntil");
				this.SetAttributeValue("postponeuntil", value);
				this.OnPropertyChanged("PostponeUntil");
			}
		}
		
		/// <summary>
		/// Type of entity with which the system job is primarily associated.
		/// </summary>
		[AttributeLogicalNameAttribute("primaryentitytype")]
		public string PrimaryEntityType
		{
			get
			{
				return this.GetAttributeValue<string>("primaryentitytype");
			}
			set
			{
				this.OnPropertyChanging("PrimaryEntityType");
				this.SetAttributeValue("primaryentitytype", value);
				this.OnPropertyChanged("PrimaryEntityType");
			}
		}
		
		/// <summary>
		/// Pattern of the system job's recurrence.
		/// </summary>
		[AttributeLogicalNameAttribute("recurrencepattern")]
		public string RecurrencePattern
		{
			get
			{
				return this.GetAttributeValue<string>("recurrencepattern");
			}
			set
			{
				this.OnPropertyChanging("RecurrencePattern");
				this.SetAttributeValue("recurrencepattern", value);
				this.OnPropertyChanged("RecurrencePattern");
			}
		}
		
		/// <summary>
		/// Starting time in UTC for the recurrence pattern.
		/// </summary>
		[AttributeLogicalNameAttribute("recurrencestarttime")]
		public Nullable<DateTime> RecurrenceStart
		{
			get
			{
				return this.GetAttributeValue<Nullable<DateTime>>("recurrencestarttime");
			}
			set
			{
				this.OnPropertyChanging("RecurrenceStart");
				this.SetAttributeValue("recurrencestarttime", value);
				this.OnPropertyChanged("RecurrenceStart");
			}
		}
		
		/// <summary>
		/// Unique identifier of the object with which the system job is associated.
		/// </summary>
		[AttributeLogicalNameAttribute("regardingobjectid")]
		public EntityReference Regarding
		{
			get
			{
				return this.GetAttributeValue<EntityReference>("regardingobjectid");
			}
			set
			{
				this.OnPropertyChanging("Regarding");
				this.SetAttributeValue("regardingobjectid", value);
				this.OnPropertyChanged("Regarding");
			}
		}
		
		/// <summary>
		/// Unique identifier of the request that generated the system job.
		/// </summary>
		[AttributeLogicalNameAttribute("requestid")]
		public Nullable<Guid> Request
		{
			get
			{
				return this.GetAttributeValue<Nullable<Guid>>("requestid");
			}
			set
			{
				this.OnPropertyChanging("Request");
				this.SetAttributeValue("requestid", value);
				this.OnPropertyChanged("Request");
			}
		}
		
		/// <summary>
		/// Retain job history.
		/// </summary>
		[AttributeLogicalNameAttribute("retainjobhistory")]
		public Nullable<bool> RetainJobHistory
		{
			get
			{
				return this.GetAttributeValue<Nullable<bool>>("retainjobhistory");
			}
			set
			{
				this.OnPropertyChanging("RetainJobHistory");
				this.SetAttributeValue("retainjobhistory", value);
				this.OnPropertyChanged("RetainJobHistory");
			}
		}
		
		/// <summary>
		/// Number of times to retry the system job.
		/// </summary>
		[AttributeLogicalNameAttribute("retrycount")]
		public Nullable<int> RetryCount
		{
			get
			{
				return this.GetAttributeValue<Nullable<int>>("retrycount");
			}
		}
		
		/// <summary>
		/// Root execution context of the job that trigerred async job.
		/// </summary>
		[AttributeLogicalNameAttribute("rootexecutioncontext")]
		public string RootExecutionContext
		{
			get
			{
				return this.GetAttributeValue<string>("rootexecutioncontext");
			}
			set
			{
				this.OnPropertyChanging("RootExecutionContext");
				this.SetAttributeValue("rootexecutioncontext", value);
				this.OnPropertyChanged("RootExecutionContext");
			}
		}
		
		/// <summary>
		/// Order in which operations were submitted.
		/// </summary>
		[AttributeLogicalNameAttribute("sequence")]
		public Nullable<long> Sequence
		{
			get
			{
				return this.GetAttributeValue<Nullable<long>>("sequence");
			}
		}
		
		/// <summary>
		/// Date and time when the system job was started.
		/// </summary>
		[AttributeLogicalNameAttribute("startedon")]
		public Nullable<DateTime> StartedOn
		{
			get
			{
				return this.GetAttributeValue<Nullable<DateTime>>("startedon");
			}
		}
		
		/// <summary>
		/// Status of the system job.
		/// </summary>
		[AttributeLogicalNameAttribute("statecode")]
		public Nullable<SystemJobState> State
		{
			get
			{
				OptionSetValue optionSet = this.GetAttributeValue<OptionSetValue>("statecode");
				if (optionSet != null)
				{
					return ((SystemJobState)(Enum.ToObject(typeof(SystemJobState), optionSet.Value)));
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
		/// Reason for the status of the system job.
		/// </summary>
		[AttributeLogicalNameAttribute("statuscode")]
		public Nullable<SystemJobStatusReason> StatusReason
		{
			get
			{
				OptionSetValue optionSet = this.GetAttributeValue<OptionSetValue>("statuscode");
				if (optionSet != null)
				{
					return ((SystemJobStatusReason)(Enum.ToObject(typeof(SystemJobStatusReason), optionSet.Value)));
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
		
		/// <summary>
		/// The Subtype of the Async Job
		/// </summary>
		[AttributeLogicalNameAttribute("subtype")]
		public Nullable<int> Subtype
		{
			get
			{
				return this.GetAttributeValue<Nullable<int>>("subtype");
			}
		}
		
		/// <summary>
		/// Unique identifier of the workflow activation related to the system job.
		/// </summary>
		[AttributeLogicalNameAttribute("workflowactivationid")]
		public EntityReference WorkflowActivationId
		{
			get
			{
				return this.GetAttributeValue<EntityReference>("workflowactivationid");
			}
			set
			{
				this.OnPropertyChanging("WorkflowActivationId");
				this.SetAttributeValue("workflowactivationid", value);
				this.OnPropertyChanged("WorkflowActivationId");
			}
		}
		
		/// <summary>
		/// Name of a workflow stage.
		/// </summary>
		[AttributeLogicalNameAttribute("workflowstagename")]
		public string WorkflowStage
		{
			get
			{
				return this.GetAttributeValue<string>("workflowstagename");
			}
		}
		
		/// <summary>
		/// The workload name.
		/// </summary>
		[AttributeLogicalNameAttribute("workload")]
		public string Workload
		{
			get
			{
				return this.GetAttributeValue<string>("workload");
			}
			set
			{
				this.OnPropertyChanging("Workload");
				this.SetAttributeValue("workload", value);
				this.OnPropertyChanged("Workload");
			}
		}
		
		public struct Fields
		{
			
			public static string AsyncOperationId = "asyncoperationid";
			
			public static string Id = "asyncoperationid";
			
			public static string BreadcrumbID = "breadcrumbid";
			
			public static string CallerOrigin = "callerorigin";
			
			public static string CompletedOn = "completedon";
			
			public static string CorrelationId = "correlationid";
			
			public static string CorrelationUpdatedTime = "correlationupdatedtime";
			
			public static string CreatedBy = "createdby";
			
			public static string CreatedOn = "createdon";
			
			public static string Data = "data";
			
			public static string DependencyToken = "dependencytoken";
			
			public static string Depth = "depth";
			
			public static string ErrorCode = "errorcode";
			
			public static string ExecutionTimeSpan = "executiontimespan";
			
			public static string ExpanderStartTime = "expanderstarttime";
			
			public static string FriendlyMessage = "friendlymessage";
			
			public static string Host = "hostid";
			
			public static string WaitingForEvent = "iswaitingforevent";
			
			public static string Message = "message";
			
			public static string MessageName = "messagename";
			
			public static string ModifiedBy = "modifiedby";
			
			public static string ModifiedOn = "modifiedon";
			
			public static string SystemJobName = "name";
			
			public static string SystemJobType = "operationtype";
			
			public static string Owner = "ownerid";
			
			public static string OwningBusinessUnit = "owningbusinessunit";
			
			public static string OwningExtension = "owningextensionid";
			
			public static string OwningTeam = "owningteam";
			
			public static string OwningUser = "owninguser";
			
			public static string ParentPluginExecutionId = "parentpluginexecutionid";
			
			public static string PostponeUntil = "postponeuntil";
			
			public static string PrimaryEntityType = "primaryentitytype";
			
			public static string RecurrencePattern = "recurrencepattern";
			
			public static string RecurrenceStart = "recurrencestarttime";
			
			public static string Regarding = "regardingobjectid";
			
			public static string Request = "requestid";
			
			public static string RetainJobHistory = "retainjobhistory";
			
			public static string RetryCount = "retrycount";
			
			public static string RootExecutionContext = "rootexecutioncontext";
			
			public static string Sequence = "sequence";
			
			public static string StartedOn = "startedon";
			
			public static string State = "statecode";
			
			public static string StatusReason = "statuscode";
			
			public static string Subtype = "subtype";
			
			public static string WorkflowActivationId = "workflowactivationid";
			
			public static string WorkflowStage = "workflowstagename";
			
			public static string Workload = "workload";
		}
	}
}
