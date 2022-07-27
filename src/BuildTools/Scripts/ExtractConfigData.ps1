<# 
	Example usage: 
		.\ExtractConfigData.ps1 
			-Url="https://aylos.crm11.dynamics.com/"
			-Username="jsmith@aylos.com"
			-SecurePassword="***********" or [-Password="passcode" -AsPlainText]
	#>

[CmdletBinding(DefaultParameterSetName="AsEncryptedText")]
Param(
	[parameter(Mandatory=$false)][String]$BuildToolsPath = "..",
	[parameter(Mandatory=$false)][String]$EnvironmentName = "none",
	[parameter(Mandatory=$false)][int]$HideData = 1,
	[parameter(Mandatory=$false)][int]$RemoveData = 1,
	[parameter(Mandatory=$false)][int]$ReplaceData = 1,
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

<# using Common Functions #>
."$CurrentPath\CommonFunctions.ps1"

<# Initialise variables #>
initializeEnvironmentVariables -envName $EnvironmentName
if (!$env:D365_OrganizationUnitId) {
	CD $CurrentPath
	throw "The global variables have not been initialized."
}

$ConnectionString = "AuthType=OAuth;Url=$Url;Username=$Username;Password=$Password;AppId=51f81489-12ee-4a9e-aaae-a2591f45987d;RedirectUri=app://58145B91-0C36-4500-8554-080854F2AC97;LoginPrompt=Auto"
$DataFolder = "..\..\..\Metadata\Config"
$OutputFile = "$DataFolder\data.zip"
$DataFile = "$DataFolder\data\data.xml"

<# Export Data #>
Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")
Write-Host "Exporting configuration data"
Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")

CD "$BuildToolsPath\ExportData\bin\"

.\ExportDataApp.exe --connectionString "$ConnectionString" --schemaFile "$DataFolder\schema.xml" --outputFile "$OutputFile"

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

<# Removes certain nodes from the XML file #>
if ($RemoveData) {
	Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")
	Write-Host "Remove XML nodes contain specific field values"
	Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")

	CD "$BuildToolsPath\RemoveData\bin\"

	.\RemoveDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --attributeName "queueviewtype" --attributeValue "1"
	.\RemoveDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --attributeName "businessunitid" --attributeValue "$env:D365_OrganizationUnitId"
	.\RemoveDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --attributeName "isdefault" --attributeValue "True"

	if ($LastExitCode -gt 0) {
		CD $CurrentPath
		throw "An error occurred whilst removing XML nodes from the data file."
	}

	CD $CurrentPath

	Write-Host
}

<# Replaces certain attribute values of specific nodes in the XML file #>
if ($HideData) {
	Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")
	Write-Host "Hide key values with tokens"
	Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")

	CD "$BuildToolsPath\HideData\bin\"

	.\HideDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --attributeName "aylos_supermode" --attributeValue "D365_ABCMode" --primaryKey "cd7b3e07-2f59-e911-a98d-00224800ce20"
	.\HideDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --attributeName "aylos_anothermode" --attributeValue "D365_XYZMode" --primaryKey "cd7b3e07-2f59-e911-a98d-00224800ce20"
	.\HideDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --attributeName "another_apikey" --attributeValue "Another_ClientApiKey" --primaryKey "79ef318d-ab25-eb11-a813-0022481b0419"
	.\HideDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --attributeName "another_serverapikey" --attributeValue "Another_ServerApiKey" --primaryKey "79ef318d-ab25-eb11-a813-0022481b0419"

	if ($LastExitCode -gt 0) {
		CD $CurrentPath
		throw "An error occurred whilst hiding text in the data file."
	}

	CD $CurrentPath

	Write-Host
}

<# Replaces certain attribute values in the XML file #>
if ($ReplaceData) {
	Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")
	Write-Host "Replace key values with tokens"
	Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")

	CD "$BuildToolsPath\ReplaceData\bin\"

	.\ReplaceDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --findText "$env:D365_AdministratorId" --replaceWithText "D365_AdministratorId"
	.\ReplaceDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --findText "$env:D365_AdministratorName" --replaceWithText "D365_AdministratorName"
	.\ReplaceDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --findText "$env:D365_OrganizationUnitId" --replaceWithText "D365_OrganizationUnitId"
	.\ReplaceDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --findText "$env:D365_OrganizationUnitName" --replaceWithText "D365_OrganizationUnitName"
	.\ReplaceDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --findText "$env:D365_OrganizationUnitTeamId" --replaceWithText "D365_OrganizationUnitTeamId"
	.\ReplaceDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --findText "$env:D365_OrganizationUnitTeamName" --replaceWithText "D365_OrganizationUnitTeamName"
	.\ReplaceDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --findText "$env:D365_THE_API_UserId" --replaceWithText "D365_THE_API_UserId"
	.\ReplaceDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --findText "$env:D365_THE_API_UserName" --replaceWithText "D365_THE_API_UserName"
	.\ReplaceDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --findText "$env:D365_Default_THE_XX1_UserId" --replaceWithText "D365_Default_THE_XX1_UserId"
	.\ReplaceDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --findText "$env:D365_Default_THE_XX1_UserName" --replaceWithText "D365_Default_THE_XX1_UserName"
	.\ReplaceDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --findText "$env:D365_Default_THE_XX2_UserId" --replaceWithText "D365_Default_THE_XX2_UserId"
	.\ReplaceDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --findText "$env:D365_Default_THE_XX2_UserName" --replaceWithText "D365_Default_THE_XX2_UserName"
	.\ReplaceDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --findText "$env:D365_MigrationSupportUserId" --replaceWithText "D365_MigrationSupportUserId"
	.\ReplaceDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --findText "$env:D365_MigrationSupportUserName" --replaceWithText "D365_MigrationSupportUserName"
	.\ReplaceDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --findText "$env:D365_ABC_API_Endpoint" --replaceWithText "D365_ABC_API_Endpoint"
	.\ReplaceDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --findText "$env:D365_ABCSecurityValue" --replaceWithText "D365_ABCSecurityValue"
	.\ReplaceDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --findText "$env:D365_XYZ_API_Endpoint" --replaceWithText "D365_XYZ_API_Endpoint"
	.\ReplaceDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --findText "$env:D365_XYZSecurityValue" --replaceWithText "D365_XYZSecurityValue"
	.\ReplaceDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --findText "$env:D365_AnotherApiPrefix" --replaceWithText "D365_AnotherApiPrefix"

	if ($LastExitCode -gt 0) {
		CD $CurrentPath
		throw "An error occurred whilst replacing text in the data file."
	}

	CD $CurrentPath

	Write-Host
}

<# Compress the Zip file #>
if ($RemoveData -or $ReplaceData) {
	Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")
	Write-Host "Compressing the Zip file"
	Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")

	CD "$BuildToolsPath\CompressFile\bin\"

	.\CompressFileApp.exe --inputPath "$DataFolder\data" --outputFile "$OutputFile"

	if ($LastExitCode -gt 0) {
		CD $CurrentPath
		throw "An error occurred whilst compressing the data files."
	}

	CD $CurrentPath

	Write-Host
}

<# Clean up the Common Functions #>
removeCommonFunctions
