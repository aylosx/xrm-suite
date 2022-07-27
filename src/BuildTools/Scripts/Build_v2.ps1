<# 
	Example usage: 
		.\Build_v2.ps1
			-Url "https://aylos.crm11.dynamics.com/"
			-TenantId "5bbf182b-e07d-4aa4-a752-094a5b8a019c"
			-ServicePrincipalId "af3e5418-d265-40eb-995f-4586a29d5a89"
			-SecureServicePrincipalSecret "***********" or [-ServicePrincipalSecret "secret" -AsPlainText]
	#>

[CmdletBinding(DefaultParameterSetName="AsEncryptedText")]
Param(
	[parameter(Mandatory=$false)][String]$BuildToolsPath = "..",
	[parameter(Mandatory=$false)][String]$EnvironmentName = "none",
	[parameter(Mandatory=$false)][int]$PublishCustomizations = 0,
	[parameter(Mandatory=$true)][String]$Url,
	[parameter(Mandatory=$true)][String]$TenantId,
	[parameter(Mandatory=$true)][String]$ServicePrincipalId,
	[parameter(Mandatory=$false, ParameterSetName="AsEncryptedText")][switch]$AsEncryptedText,
	[parameter(Mandatory=$true, ParameterSetName="AsEncryptedText")][SecureString]$SecureServicePrincipalSecret,
	[parameter(Mandatory=$false, ParameterSetName="AsPlainText")][switch]$AsPlainText,
	[parameter(Mandatory=$true, ParameterSetName="AsPlainText")][String]$ServicePrincipalSecret
)

Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")
Write-Host "Building the components and preparing all the artifacts required by the release."
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

<# By default we are not publishing the customisations #>
if ($PublishCustomizations) {
	.\PublishCustomizations_v2.ps1 -Url "$Url" -TenantId "$TenantId" -ServicePrincipalId "$ServicePrincipalId" -ServicePrincipalSecret "$ServicePrincipalSecret" -AsPlainText
}

.\UpdateSolutionVersions_v2.ps1 -Url "$Url" -TenantId "$TenantId" -ServicePrincipalId "$ServicePrincipalId" -ServicePrincipalSecret "$ServicePrincipalSecret" -AsPlainText

.\ExportSolutions_v2.ps1 -Url "$Url" -TenantId "$TenantId" -ServicePrincipalId "$ServicePrincipalId" -ServicePrincipalSecret "$ServicePrincipalSecret" -AsPlainText

.\ExtractSolutions.ps1

.\PackSolutions.ps1

.\ExtractConfigData_v2.ps1 -EnvironmentName $EnvironmentName -Url "$Url" -TenantId "$TenantId" -ServicePrincipalId "$ServicePrincipalId" -ServicePrincipalSecret "$ServicePrincipalSecret" -AsPlainText

.\ExtractReferenceData_v2.ps1 -Url "$Url" -TenantId "$TenantId" -ServicePrincipalId "$ServicePrincipalId" -ServicePrincipalSecret "$ServicePrincipalSecret" -AsPlainText

.\ExportPortals_v2.ps1 -EnvironmentName $EnvironmentName -Url "$Url" -TenantId "$TenantId" -ServicePrincipalId "$ServicePrincipalId" -ServicePrincipalSecret "$ServicePrincipalSecret" -AsPlainText
