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
	/// Represents a source of entities bound to a CRM service. It tracks and manages changes made to the retrieved entities.
	/// </summary>
	[GeneratedCodeAttribute("CrmSvcUtil", "9.1.0.91")]
	public partial class CrmServiceContext : OrganizationServiceContext
	{
		
		/// <summary>
		/// Constructor.
		/// </summary>
		public CrmServiceContext(IOrganizationService service) : 
				base(service)
		{
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="Account"/> entities.
		/// </summary>
		public IQueryable<Account> AccountSet
		{
			get
			{
				return this.CreateQuery<Account>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="ActivityParty"/> entities.
		/// </summary>
		public IQueryable<ActivityParty> ActivityPartySet
		{
			get
			{
				return this.CreateQuery<ActivityParty>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="Activity"/> entities.
		/// </summary>
		public IQueryable<Activity> ActivitySet
		{
			get
			{
				return this.CreateQuery<Activity>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="Appointment"/> entities.
		/// </summary>
		public IQueryable<Appointment> AppointmentSet
		{
			get
			{
				return this.CreateQuery<Appointment>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="ExecutionContext"/> entities.
		/// </summary>
		public IQueryable<ExecutionContext> ExecutionContextSet
		{
			get
			{
				return this.CreateQuery<ExecutionContext>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="Contact"/> entities.
		/// </summary>
		public IQueryable<Contact> ContactSet
		{
			get
			{
				return this.CreateQuery<Contact>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="Task"/> entities.
		/// </summary>
		public IQueryable<Task> TaskSet
		{
			get
			{
				return this.CreateQuery<Task>();
			}
		}
	}
}
