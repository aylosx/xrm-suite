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

$ConnectionString = "AuthType=OAuth;Url=$Url;Username=$Username;Password=$Password;AppId=51f81489-12ee-4a9e-aaae-a2591f45987d;RedirectUri=app://58145B91-0C36-4500-8554-080854F2AC97;LoginPrompt=Auto"
$DataFolder = "..\..\..\Metadata\Reference"
$DataFile = "$DataFolder\data\data.xml"

<# Export Data #>
Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")
Write-Host "Exporting reference data"
Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")

CD "$BuildToolsPath\ExportData\bin\"

.\ExportDataApp.exe --connectionString "$ConnectionString" --schemaFile "$DataFolder\schema.xml" --outputFile "$DataFolder\data.zip"

if ($LastExitCode -gt 0) {
	CD $CurrentPath
	throw "An error occurred whilst exporting the reference data."
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

<# Removes the timestamp attribute from the XML file #>
Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")
Write-Host "Remove the timestamp attribute from the data file."
Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")

CD "$BuildToolsPath\RemoveData\bin\"

.\RemoveDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --attributeName "noname" --attributeValue "novalue"

if ($LastExitCode -gt 0) {
	CD $CurrentPath
	throw "An error occurred whilst removing the timestamp attribute from the data file."
}

CD $CurrentPath

Write-Host
