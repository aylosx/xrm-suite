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
	[GeneratedCodeAttribute("CrmSvcUtil", "9.1.0.95")]
	public enum AppointmentStatusReason
	{
		
		[EnumMemberAttribute()]
		Free = 1,
		
		[EnumMemberAttribute()]
		Tentative = 2,
		
		[EnumMemberAttribute()]
		Completed = 3,
		
		[EnumMemberAttribute()]
		Canceled = 4,
		
		[EnumMemberAttribute()]
		Busy = 5,
		
		[EnumMemberAttribute()]
		OutOfOffice = 6,
	}
}
