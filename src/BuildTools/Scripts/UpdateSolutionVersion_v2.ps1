<# 
	Example usage: 
		.\UpdateSolutionVersion_v2.ps1
			-SolutionName "MySolution"
			-Url "https://aylos.crm11.dynamics.com/"
			-TenantId "5bbf182b-e07d-4aa4-a752-094a5b8a019c"
			-ServicePrincipalId "af3e5418-d265-40eb-995f-4586a29d5a89"
			-SecureServicePrincipalSecret "***********" or [-ServicePrincipalSecret "secret" -AsPlainText]
	#>

[CmdletBinding(DefaultParameterSetName="AsEncryptedText")]
Param(
	[parameter(Mandatory=$false)][String]$BuildToolsPath = "..",
	[parameter(Mandatory=$true)][String]$SolutionName,
	[parameter(Mandatory=$true)][String]$Url,
	[parameter(Mandatory=$true)][String]$TenantId,
	[parameter(Mandatory=$true)][String]$ServicePrincipalId,
	[parameter(Mandatory=$false, ParameterSetName="AsEncryptedText")][switch]$AsEncryptedText,
	[parameter(Mandatory=$true, ParameterSetName="AsEncryptedText")][SecureString]$SecureServicePrincipalSecret,
	[parameter(Mandatory=$false, ParameterSetName="AsPlainText")][switch]$AsPlainText,
	[parameter(Mandatory=$true, ParameterSetName="AsPlainText")][String]$ServicePrincipalSecret
)

Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")
Write-Host Updating the version of the $SolutionName solution
Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")

$CurrentPath = Get-Location

switch ($PSCmdlet.ParameterSetName)
{
    "AsEncryptedText"
    {
		$BSTR = [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($SecureServicePrincipalSecret)
		$ServicePrincipalSecret = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($BSTR)
    }
}

$Version = ([System.TimeZoneInfo]::ConvertTimeBySystemTimeZoneId((Get-Date), "GMT Standard Time")).tostring("yy.MM.dd.HHmm")

CD "$BuildToolsPath\UpdateSolutionVersion\bin\"

.\UpdateSolutionVersionApp.exe --organizationUrl "$Url" --tenantId "$TenantId" --servicePrincipalId "$ServicePrincipalId" --servicePrincipalSecret "$ServicePrincipalSecret" --solutionName "$SolutionName" --version "$Version"

if ($LastExitCode -gt 0) {
	CD $CurrentPath
	throw "An error occurred whilst updating the version of the $SolutionName solution."
	}

CD $CurrentPath

Write-Host
