<# 
	Example usage: 
		.\ImportReferenceData.ps1 
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

$DataFolder = "..\..\..\Metadata\Reference"
$InputFile = "$DataFolder\data.zip"
$DataFile = "$DataFolder\data\data.xml"

<# Import Data #>
Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")
Write-Host "Importing reference data"
Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")

CD "$BuildToolsPath\ImportData\bin\"

.\ImportDataApp.exe --connectionString "$ConnectionString" --inputFile "$InputFile"

if ($LastExitCode -gt 0) {
	CD $CurrentPath
	throw "An error occurred whilst importing the reference data."
}

CD $CurrentPath

Write-Host
