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
	using System.CodeDom.Compiler;
	using System.Runtime.Serialization;
	
	
	[DataContractAttribute()]
	[GeneratedCodeAttribute("CrmSvcUtil", "9.1.0.71")]
	public enum AppointmentAppointmentType
	{
		
		[EnumMemberAttribute()]
		NotRecurring = 0,
		
		[EnumMemberAttribute()]
		RecurringMaster = 1,
		
		[EnumMemberAttribute()]
		RecurringInstance = 2,
		
		[EnumMemberAttribute()]
		RecurringException = 3,
		
		[EnumMemberAttribute()]
		RecurringFutureException = 4,
	}
}
