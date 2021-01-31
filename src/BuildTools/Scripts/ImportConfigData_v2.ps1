﻿<# 
	Example usage: 
		.\ImportConfigData_v2.ps1 
			-Url "https://aylos.crm11.dynamics.com/"
			-TenantId "5bbf182b-e07d-4aa4-a752-094a5b8a019c"
			-ServicePrincipalId "af3e5418-d265-40eb-995f-4586a29d5a89"
			-SecureServicePrincipalSecret "***********" or [-ServicePrincipalSecret "secret" -AsPlainText]
	#>

[CmdletBinding(DefaultParameterSetName="AsEncryptedText")]
Param(
	[parameter(Mandatory=$false)][String]$BuildToolsPath = "..",
	[parameter(Mandatory=$false)][String]$EnvironmentName = "none",
	[parameter(Mandatory=$false)][int]$RemoveData = 1,
	[parameter(Mandatory=$false)][int]$ReplaceData = 1,
	[parameter(Mandatory=$true)][String]$Url,
	[parameter(Mandatory=$true)][String]$TenantId,
	[parameter(Mandatory=$true)][String]$ServicePrincipalId,
	[parameter(Mandatory=$false, ParameterSetName="AsEncryptedText")][switch]$AsEncryptedText,
	[parameter(Mandatory=$true, ParameterSetName="AsEncryptedText")][SecureString]$SecureServicePrincipalSecret,
	[parameter(Mandatory=$false, ParameterSetName="AsPlainText")][switch]$AsPlainText,
	[parameter(Mandatory=$true, ParameterSetName="AsPlainText")][String]$ServicePrincipalSecret
)

$CurrentPath = Get-Location

switch ($PSCmdlet.ParameterSetName)
{
    "AsEncryptedText"
    {
		$BSTR = [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($SecureServicePrincipalSecret)
		$ServicePrincipalSecret = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($BSTR)
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

$DataFolder = "..\..\..\Metadata\Config"
$InputFile = "$DataFolder\data.zip"
$DataFile = "$DataFolder\data\data.xml"

<# Extract Zip File #>
Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")
Write-Host "Decompressing the Zip file"
Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")

CD "$BuildToolsPath\DecompressFile\bin\"

.\DecompressFileApp.exe --inputFile "$InputFile" --outputPath "$DataFolder\data"

if ($LastExitCode -gt 0) {
	CD $CurrentPath
	throw "An error occurred whilst decompressing the Zip file."
}

CD $CurrentPath

Write-Host

<# Replaces certain attribute values in the XML file #>
Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")
Write-Host "Replace tokens with key values"
Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")

CD "$BuildToolsPath\ReplaceData\bin\"

.\ReplaceDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --findText "D365_AdministratorId" --replaceWithText "$env:D365_AdministratorId"
.\ReplaceDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --findText "D365_AdministratorName" --replaceWithText "$env:D365_AdministratorName"
.\ReplaceDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --findText "D365_OrganizationUnitId" --replaceWithText "$env:D365_OrganizationUnitId"
.\ReplaceDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --findText "D365_OrganizationUnitName" --replaceWithText "$env:D365_OrganizationUnitName"
.\ReplaceDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --findText "D365_OrganizationUnitTeamId" --replaceWithText "$env:D365_OrganizationUnitTeamId"
.\ReplaceDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --findText "D365_OrganizationUnitTeamName" --replaceWithText "$env:D365_OrganizationUnitTeamName"
.\ReplaceDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --findText "D365_THE_API_UserId" --replaceWithText "$env:D365_THE_API_UserId"
.\ReplaceDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --findText "D365_THE_API_UserName" --replaceWithText "$env:D365_THE_API_UserName"
.\ReplaceDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --findText "D365_Default_THE_XX1_UserId" --replaceWithText "$env:D365_Default_THE_XX1_UserId"
.\ReplaceDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --findText "D365_Default_THE_XX1_UserName" --replaceWithText "$env:D365_Default_THE_XX1_UserName"
.\ReplaceDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --findText "D365_Default_THE_XX2_UserId" --replaceWithText "$env:D365_Default_THE_XX2_UserId"
.\ReplaceDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --findText "D365_Default_THE_XX2_UserName" --replaceWithText "$env:D365_Default_THE_XX2_UserName"
.\ReplaceDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --findText "D365_MigrationSupportUserId" --replaceWithText "$env:D365_MigrationSupportUserId"
.\ReplaceDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --findText "D365_MigrationSupportUserName" --replaceWithText "$env:D365_MigrationSupportUserName"
.\ReplaceDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --findText "D365_ABC_API_Endpoint" --replaceWithText "$env:D365_ABC_API_Endpoint"
.\ReplaceDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --findText "D365_ABCMode" --replaceWithText "$env:D365_ABCMode"
.\ReplaceDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --findText "D365_ABCSecurityValue" --replaceWithText "$env:D365_ABCSecurityValue"
.\ReplaceDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --findText "D365_XYZ_API_Endpoint" --replaceWithText "$env:D365_XYZ_API_Endpoint"
.\ReplaceDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --findText "D365_XYZMode" --replaceWithText "$env:D365_XYZMode"
.\ReplaceDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --findText "D365_XYZSecurityValue" --replaceWithText "$env:D365_XYZSecurityValue"
.\ReplaceDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --findText "D365_AnotherApiPrefix" --replaceWithText "$env:D365_AnotherApiPrefix"
.\ReplaceDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --findText "Another_ClientApiKey" --replaceWithText "$env:Another_ClientApiKey"
.\ReplaceDataApp.exe --inputFile "$DataFile" --outputFile "$DataFile" --findText "Another_ServerApiKey" --replaceWithText "$env:Another_ServerApiKey"

if ($LastExitCode -gt 0) {
	CD $CurrentPath
	throw "An error occurred whilst replacing text in the data file."
}

CD $CurrentPath

Write-Host

<# Compress Zip File #>
Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")
Write-Host "Compressing the Zip file"
Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")

CD "$BuildToolsPath\CompressFile\bin\"

.\CompressFileApp.exe --inputPath "$DataFolder\data" --outputFile "$InputFile"

if ($LastExitCode -gt 0) {
	CD $CurrentPath
	throw "An error occurred whilst compressing the data files."
}

CD $CurrentPath

Write-Host

<# Import Data #>
Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")
Write-Host "Importing configuration data"
Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")

CD "$BuildToolsPath\ImportData\bin\"

.\ImportDataApp.exe --organizationUrl "$Url" --tenantId "$TenantId" --servicePrincipalId "$ServicePrincipalId" --servicePrincipalSecret "$ServicePrincipalSecret" --inputFile "$InputFile"

if ($LastExitCode -gt 0) {
	CD $CurrentPath
	throw "An error occurred whilst importing the configuration data."
}

CD $CurrentPath

Write-Host

<# Clean up the Common Functions #>
removeCommonFunctions
