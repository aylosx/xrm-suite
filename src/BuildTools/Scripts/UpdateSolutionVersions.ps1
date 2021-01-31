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

switch ($PSCmdlet.ParameterSetName)
{
    "AsEncryptedText"
    {
		$BSTR = [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($SecurePassword)
		$Password = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($BSTR)
    }
}

.\UpdateSolutionVersion.ps1 -SolutionName "AylosSchema" -Url "$Url" -Username "$Username" -Password "$Password" -AsPlainText
.\UpdateSolutionVersion.ps1 -SolutionName "AylosSecurity" -Url "$Url" -Username "$Username" -Password "$Password" -AsPlainText
.\UpdateSolutionVersion.ps1 -SolutionName "AylosAnalytics" -Url "$Url" -Username "$Username" -Password "$Password" -AsPlainText
.\UpdateSolutionVersion.ps1 -SolutionName "AylosTemplates" -Url "$Url" -Username "$Username" -Password "$Password" -AsPlainText
.\UpdateSolutionVersion.ps1 -SolutionName "AylosExtensions" -Url "$Url" -Username "$Username" -Password "$Password" -AsPlainText
.\UpdateSolutionVersion.ps1 -SolutionName "AylosProcesses" -Url "$Url" -Username "$Username" -Password "$Password" -AsPlainText
.\UpdateSolutionVersion.ps1 -SolutionName "AylosUserInterface" -Url "$Url" -Username "$Username" -Password "$Password" -AsPlainText
