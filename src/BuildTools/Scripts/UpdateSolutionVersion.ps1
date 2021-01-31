<# 
	Example usage: 
		.\UpdateSolutionVersion.ps1
			-SolutionName "MySolution"
			-Url="https://aylos.crm11.dynamics.com/"
			-Username="jsmith@aylos.com"
			-SecurePassword="***********" or [-Password="passcode" -AsPlainText]
	#>

[CmdletBinding(DefaultParameterSetName="AsEncryptedText")]
Param(
	[parameter(Mandatory=$false)][String]$BuildToolsPath = "..",
	[parameter(Mandatory=$true)][String]$SolutionName,
	[parameter(Mandatory=$true)][String]$Url,
	[parameter(Mandatory=$true)][String]$Username,
	[parameter(Mandatory=$false, ParameterSetName="AsEncryptedText")][switch]$AsEncryptedText,
	[parameter(Mandatory=$true, ParameterSetName="AsEncryptedText")][SecureString]$SecurePassword,
	[parameter(Mandatory=$false, ParameterSetName="AsPlainText")][switch]$AsPlainText,
	[parameter(Mandatory=$true, ParameterSetName="AsPlainText")][String]$Password
)

Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")
Write-Host Updating the version of the $SolutionName solution
Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")

$CurrentPath = Get-Location

switch ($PSCmdlet.ParameterSetName)
{
    "AsEncryptedText"
    {
		$BSTR = [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($SecurePassword)
		$Password = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($BSTR)
    }
}

$ConnectionString = "AuthType=Office365;Username=$Username;Password=$Password;Url=$Url"
$Version = ([System.TimeZoneInfo]::ConvertTimeBySystemTimeZoneId((Get-Date), "GMT Standard Time")).tostring("yy.MM.dd.HHmm")

CD "$BuildToolsPath\UpdateSolutionVersion\bin\"

.\UpdateSolutionVersionApp.exe --connectionString "$ConnectionString" --solutionName "$SolutionName" --version "$Version"

if ($LastExitCode -gt 0) {
	CD $CurrentPath
	throw "An error occurred whilst updating the version of the $SolutionName solution."
	}

CD $CurrentPath

Write-Host
