<# 
	Example usage: 
		.\ImportSolutions_v2.ps1 
			-Url="https://aylos.crm11.dynamics.com/"
			-TenantId "5bbf182b-e07d-4aa4-a752-094a5b8a019c"
			-ServicePrincipalId "af3e5418-d265-40eb-995f-4586a29d5a89"
			-SecureServicePrincipalSecret "***********" or [-ServicePrincipalSecret "secret" -AsPlainText]
	#>

[CmdletBinding(DefaultParameterSetName="AsEncryptedText")]
Param(
	[parameter(Mandatory=$false)][String]$BuildToolsPath = "..",
	[parameter(Mandatory=$false)][String]$SolutionType = "Managed",
	[parameter(Mandatory=$true)][String]$Url,
	[parameter(Mandatory=$true)][String]$TenantId,
	[parameter(Mandatory=$true)][String]$ServicePrincipalId,
	[parameter(Mandatory=$false, ParameterSetName="AsEncryptedText")][switch]$AsEncryptedText,
	[parameter(Mandatory=$true, ParameterSetName="AsEncryptedText")][SecureString]$SecureServicePrincipalSecret,
	[parameter(Mandatory=$false, ParameterSetName="AsPlainText")][switch]$AsPlainText,
	[parameter(Mandatory=$true, ParameterSetName="AsPlainText")][String]$ServicePrincipalSecret
)

Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")
Write-Host Importing the $SolutionType.ToLower() solutions
Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")
Write-Host 

switch ($PSCmdlet.ParameterSetName)
{
    "AsEncryptedText"
    {
		$BSTR = [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($SecureServicePrincipalSecret)
		$ServicePrincipalSecret = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($BSTR)
    }
}

.\ImportSolution_v2.ps1 -SolutionType $SolutionType -SolutionName "AylosSchema" -InputFile "..\..\..\Metadata\Solutions\$SolutionType\AylosSchema.zip" -ConnectionTimeout 300 -PollingInterval 20 -PollingTimeout 3600 -Url "$Url" -TenantId "$TenantId" -ServicePrincipalId "$ServicePrincipalId" -ServicePrincipalSecret "$ServicePrincipalSecret" -AsPlainText

.\ImportSolution_v2.ps1 -SolutionType $SolutionType -SolutionName "AylosSecurity" -InputFile "..\..\..\Metadata\Solutions\$SolutionType\AylosSecurity.zip" -ConnectionTimeout 300 -Url "$Url" -TenantId "$TenantId" -ServicePrincipalId "$ServicePrincipalId" -ServicePrincipalSecret "$ServicePrincipalSecret" -AsPlainText

.\ImportSolution_v2.ps1 -SolutionType $SolutionType -SolutionName "AylosAnalytics" -InputFile "..\..\..\Metadata\Solutions\$SolutionType\AylosAnalytics.zip" -ConnectionTimeout 300 -Url "$Url" -TenantId "$TenantId" -ServicePrincipalId "$ServicePrincipalId" -ServicePrincipalSecret "$ServicePrincipalSecret" -AsPlainText

.\ImportSolution_v2.ps1 -SolutionType $SolutionType -SolutionName "AylosTemplates" -InputFile "..\..\..\Metadata\Solutions\$SolutionType\AylosTemplates.zip" -ConnectionTimeout 300 -Url "$Url" -TenantId "$TenantId" -ServicePrincipalId "$ServicePrincipalId" -ServicePrincipalSecret "$ServicePrincipalSecret" -AsPlainText

.\ImportSolution_v2.ps1 -SolutionType $SolutionType -SolutionName "AylosExtensions" -InputFile "..\..\..\Metadata\Solutions\$SolutionType\AylosExtensions.zip" -ConnectionTimeout 300 -Url "$Url" -TenantId "$TenantId" -ServicePrincipalId "$ServicePrincipalId" -ServicePrincipalSecret "$ServicePrincipalSecret" -AsPlainText

.\ImportSolution_v2.ps1 -SolutionType $SolutionType -SolutionName "AylosProcesses" -InputFile "..\..\..\Metadata\Solutions\$SolutionType\AylosProcesses.zip" -ConnectionTimeout 300 -Url "$Url" -TenantId "$TenantId" -ServicePrincipalId "$ServicePrincipalId" -ServicePrincipalSecret "$ServicePrincipalSecret" -AsPlainText

.\ImportSolution_v2.ps1 -SolutionType $SolutionType -SolutionName "AylosUserInterface" -InputFile "..\..\..\Metadata\Solutions\$SolutionType\AylosUserInterface.zip" -ConnectionTimeout 300 -Url "$Url" -TenantId "$TenantId" -ServicePrincipalId "$ServicePrincipalId" -ServicePrincipalSecret "$ServicePrincipalSecret" -AsPlainText
