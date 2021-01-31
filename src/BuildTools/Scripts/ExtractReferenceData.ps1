<# 
	Example usage: 
		.\ExtractReferenceData.ps1 
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
$DataFile = "$DataFolder\data\data.xml"

<# Export Data #>
Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")
Write-Host "Exporting configuration data"
Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")

CD "$BuildToolsPath\ExportData\bin\"

.\ExportDataApp.exe --connectionString "$ConnectionString" --schemaFile "$DataFolder\schema.xml" --outputFile "$DataFolder\data.zip"

if ($LastExitCode -gt 0) {
	CD $CurrentPath
	throw "An error occurred whilst exporting the configuration data."
}

CD $CurrentPath

Write-Host

<# Extract Zip File #>
Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")
Write-Host "Decompressing the Zip file"
Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")

CD "$BuildToolsPath\DecompressFile\bin\"

.\DecompressFileApp.exe --inputFile "$DataFolder\data.zip" --outputPath "$DataFolder\data"

if ($LastExitCode -gt 0) {
	CD $CurrentPath
	throw "An error occurred whilst decompressing the Zip file."
}

CD $CurrentPath

Write-Host
