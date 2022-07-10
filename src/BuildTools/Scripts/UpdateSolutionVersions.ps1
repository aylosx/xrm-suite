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

<# using Common Functions #>
."$CurrentPath\CommonFunctions.ps1"

<# Initialise variables #>
initializeEnvironmentVariables -envName $EnvironmentName
if (!$env:D365_OrganizationUnitId) {
	CD $CurrentPath
	throw "The global variables have not been initialized."
}

$Version = "$MajorRelease." + ([System.TimeZoneInfo]::ConvertTimeBySystemTimeZoneId((Get-Date), "GMT Standard Time")).tostring("yy.MM.ddHHmm")

.\UpdateSolutionVersion.ps1 -SolutionName "AylosSchema" -Version "$Version" -Url "$Url" -Username "$Username" -Password "$Password" -AsPlainText
.\UpdateSolutionVersion.ps1 -SolutionName "AylosSecurity" -Version "$Version" -Url "$Url" -Username "$Username" -Password "$Password" -AsPlainText
.\UpdateSolutionVersion.ps1 -SolutionName "AylosAnalytics" -Version "$Version" -Url "$Url" -Username "$Username" -Password "$Password" -AsPlainText
.\UpdateSolutionVersion.ps1 -SolutionName "AylosTemplates" -Version "$Version" -Url "$Url" -Username "$Username" -Password "$Password" -AsPlainText
.\UpdateSolutionVersion.ps1 -SolutionName "AylosExtensions" -Version "$Version" -Url "$Url" -Username "$Username" -Password "$Password" -AsPlainText
.\UpdateSolutionVersion.ps1 -SolutionName "AylosProcesses" -Version "$Version" -Url "$Url" -Username "$Username" -Password "$Password" -AsPlainText
.\UpdateSolutionVersion.ps1 -SolutionName "AylosUserInterface" -Version "$Version" -Url "$Url" -Username "$Username" -Password "$Password" -AsPlainText
