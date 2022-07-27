<# 
	Example usage: 
		.\UpdateSolutionVersions_v2.ps1
			-Url "https://aylos.crm11.dynamics.com/"
			-TenantId "5bbf182b-e07d-4aa4-a752-094a5b8a019c"
			-ServicePrincipalId "af3e5418-d265-40eb-995f-4586a29d5a89"
			-SecureServicePrincipalSecret "***********" or [-ServicePrincipalSecret "secret" -AsPlainText]
	#>

[CmdletBinding(DefaultParameterSetName="AsEncryptedText")]
Param(
	[parameter(Mandatory=$false)][String]$BuildToolsPath = "..",
	[parameter(Mandatory=$true)][String]$Url,
	[parameter(Mandatory=$true)][String]$TenantId,
	[parameter(Mandatory=$true)][String]$ServicePrincipalId,
	[parameter(Mandatory=$false, ParameterSetName="AsEncryptedText")][switch]$AsEncryptedText,
	[parameter(Mandatory=$true, ParameterSetName="AsEncryptedText")][SecureString]$SecureServicePrincipalSecret,
	[parameter(Mandatory=$false, ParameterSetName="AsPlainText")][switch]$AsPlainText,
	[parameter(Mandatory=$true, ParameterSetName="AsPlainText")][String]$ServicePrincipalSecret
)

Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")
Write-Host Updating the version of the solutions
Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")
Write-Host 

$CurrentPath = Get-Location

switch ($PSCmdlet.ParameterSetName)
{
    "AsEncryptedText"
    {
		$BSTR = [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($SecureServicePrincipalSecret)
		$ServicePrincipalSecret = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($BSTR)
    }
}

."$CurrentPath\CommonFunctions.ps1"

<# Initialise variables #>
initializeEnvironmentVariables -envName $EnvironmentName
if (!$env:D365_OrganizationUnitId) {
	CD $CurrentPath
	throw "The global variables have not been initialized."
}

$Version = "$env:D365_MajorReleaseNumber." + ([System.TimeZoneInfo]::ConvertTimeBySystemTimeZoneId((Get-Date), "GMT Standard Time")).tostring("yy.MM.ddHHmm")

.\UpdateSolutionVersion_v2.ps1 -SolutionName "AylosSchema" -Version "$Version" -Url "$Url" -TenantId "$TenantId" -ServicePrincipalId "$ServicePrincipalId" -ServicePrincipalSecret "$ServicePrincipalSecret" -AsPlainText
.\UpdateSolutionVersion_v2.ps1 -SolutionName "AylosSecurity" -Version "$Version" -Url "$Url" -TenantId "$TenantId" -ServicePrincipalId "$ServicePrincipalId" -ServicePrincipalSecret "$ServicePrincipalSecret" -AsPlainText
.\UpdateSolutionVersion_v2.ps1 -SolutionName "AylosAnalytics" -Version "$Version" -Url "$Url" -TenantId "$TenantId" -ServicePrincipalId "$ServicePrincipalId" -ServicePrincipalSecret "$ServicePrincipalSecret" -AsPlainText
.\UpdateSolutionVersion_v2.ps1 -SolutionName "AylosTemplates" -Version "$Version" -Url "$Url" -TenantId "$TenantId" -ServicePrincipalId "$ServicePrincipalId" -ServicePrincipalSecret "$ServicePrincipalSecret" -AsPlainText
.\UpdateSolutionVersion_v2.ps1 -SolutionName "AylosExtensions" -Version "$Version" -Url "$Url" -TenantId "$TenantId" -ServicePrincipalId "$ServicePrincipalId" -ServicePrincipalSecret "$ServicePrincipalSecret" -AsPlainText
.\UpdateSolutionVersion_v2.ps1 -SolutionName "AylosProcesses" -Version "$Version" -Url "$Url" -TenantId "$TenantId" -ServicePrincipalId "$ServicePrincipalId" -ServicePrincipalSecret "$ServicePrincipalSecret" -AsPlainText
.\UpdateSolutionVersion_v2.ps1 -SolutionName "AylosUserInterface" -Version "$Version" -Url "$Url" -TenantId "$TenantId" -ServicePrincipalId "$ServicePrincipalId" -ServicePrincipalSecret "$ServicePrincipalSecret" -AsPlainText
