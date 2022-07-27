<# 
	Example usage: 
		.\UpdateSolutionVersions.ps1
			-Url="https://aylos.crm11.dynamics.com/"
			-Username="jsmith@aylos.com"
			-SecurePassword="***********" or [-Password="passcode" -AsPlainText]
	#>

[CmdletBinding(DefaultParameterSetName="AsEncryptedText")]
Param(
	[parameter(Mandatory=$false)][String]$BuildToolsPath = "..",
	[parameter(Mandatory=$true)][String]$Url,
	[parameter(Mandatory=$true)][String]$Username,
	[parameter(Mandatory=$false, ParameterSetName="AsEncryptedText")][switch]$AsEncryptedText,
	[parameter(Mandatory=$true, ParameterSetName="AsEncryptedText")][SecureString]$SecurePassword,
	[parameter(Mandatory=$false, ParameterSetName="AsPlainText")][switch]$AsPlainText,
	[parameter(Mandatory=$true, ParameterSetName="AsPlainText")][String]$Password
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
		$BSTR = [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($SecurePassword)
		$Password = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($BSTR)
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

.\UpdateSolutionVersion.ps1 -SolutionName "WMBCSchema" -Version "$Version" -Url "$Url" -Username "$Username" -Password "$Password" -AsPlainText
.\UpdateSolutionVersion.ps1 -SolutionName "WMBCSecurity" -Version "$Version" -Url "$Url" -Username "$Username" -Password "$Password" -AsPlainText
.\UpdateSolutionVersion.ps1 -SolutionName "WMBCAnalytics" -Version "$Version" -Url "$Url" -Username "$Username" -Password "$Password" -AsPlainText
.\UpdateSolutionVersion.ps1 -SolutionName "WMBCTemplates" -Version "$Version" -Url "$Url" -Username "$Username" -Password "$Password" -AsPlainText
.\UpdateSolutionVersion.ps1 -SolutionName "WMBCExtensions" -Version "$Version" -Url "$Url" -Username "$Username" -Password "$Password" -AsPlainText
.\UpdateSolutionVersion.ps1 -SolutionName "WMBCProcesses" -Version "$Version" -Url "$Url" -Username "$Username" -Password "$Password" -AsPlainText
.\UpdateSolutionVersion.ps1 -SolutionName "WMBCUserInterface" -Version "$Version" -Url "$Url" -Username "$Username" -Password "$Password" -AsPlainText
