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
	public enum GlobalModelTrainingStatus2
	{
		
		[EnumMemberAttribute()]
		NotTrained = 100000000,
		
		[EnumMemberAttribute()]
		TrainingInProgress = 100000001,
		
		[EnumMemberAttribute()]
		TrainingCompleted = 100000002,
		
		[EnumMemberAttribute()]
		TrainingFailed = 100000003,
		
		[EnumMemberAttribute()]
		PublishInProgress = 100000004,
		
		[EnumMemberAttribute()]
		PublishFailed = 100000005,
		
		[EnumMemberAttribute()]
		PublishCompleted = 100000006,
		
		[EnumMemberAttribute()]
		LoadingData = 100000007,
	}
}
