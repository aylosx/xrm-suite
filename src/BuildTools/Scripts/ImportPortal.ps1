<# 
	Example usage: 
		.\ImportPortal.ps1
			-PortalName "walsall-my-account"
			-WebSiteId "0f39bab4-83e1-ec11-bb3d-0022481add31"
			-Url "https://wmbc-ce.crm11.dynamics.com/"
			-Username="jsmith@walsall.gov.uk"
			-SecurePassword="***********" or [-Password="passcode" -AsPlainText]
	#>

[CmdletBinding(DefaultParameterSetName="AsEncryptedText")]
Param(
	[parameter(Mandatory=$false)][String]$BuildToolsPath = "..",
	[parameter(Mandatory=$false)][String]$EnvironmentName = "none",
	[parameter(Mandatory=$true)][String]$PortalName,
	[parameter(Mandatory=$true)][String]$WebSiteId,
	[parameter(Mandatory=$true)][String]$Url,
	[parameter(Mandatory=$true)][String]$Username,
	[parameter(Mandatory=$false, ParameterSetName="AsEncryptedText")][switch]$AsEncryptedText,
	[parameter(Mandatory=$true, ParameterSetName="AsEncryptedText")][SecureString]$SecurePassword,
	[parameter(Mandatory=$false, ParameterSetName="AsPlainText")][switch]$AsPlainText,
	[parameter(Mandatory=$true, ParameterSetName="AsPlainText")][String]$Password
)

Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")
Write-Host Importing the portal $WebSiteId 
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

."$CurrentPath\CommonFunctions.ps1"

<# Initialise variables #>
initializeEnvironmentVariables -envName $EnvironmentName
if (!$env:D365_OrganizationUnitId) {
	CD $CurrentPath
	throw "The global variables have not been initialized."
}

$PortalPath1 = "..\..\..\..\Portals\$PortalName"
$PortalPath2 = "..\..\..\Portals\$PortalName"

<# Replaces certain values in the target files with the relevant tokens #>
Write-Host
Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")
Write-Host "Replace key values with tokens"
Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")

CD "$BuildToolsPath\ReplaceData\bin\"

.\ReplaceDataApp.exe --inputFile "$PortalPath2\sitesetting.yml" --outputFile "$PortalPath2\sitesetting.yml" --findText "AAD_B2C_1_ValidIssuers" --replaceWithText "$env:AAD_B2C_1_ValidIssuers"
.\ReplaceDataApp.exe --inputFile "$PortalPath2\sitesetting.yml" --outputFile "$PortalPath2\sitesetting.yml" --findText "AAD_B2C_1_MetadataAddress" --replaceWithText "$env:AAD_B2C_1_MetadataAddress"
.\ReplaceDataApp.exe --inputFile "$PortalPath2\sitesetting.yml" --outputFile "$PortalPath2\sitesetting.yml" --findText "AAD_B2C_1_Authority" --replaceWithText "$env:AAD_B2C_1_Authority"
.\ReplaceDataApp.exe --inputFile "$PortalPath2\sitesetting.yml" --outputFile "$PortalPath2\sitesetting.yml" --findText "AAD_B2C_1_ClientId" --replaceWithText "$env:AAD_B2C_1_ClientId"
.\ReplaceDataApp.exe --inputFile "$PortalPath2\sitesetting.yml" --outputFile "$PortalPath2\sitesetting.yml" --findText "AAD_B2C_1_RedirectUri" --replaceWithText "$env:AAD_B2C_1_RedirectUri"
.\ReplaceDataApp.exe --inputFile "$PortalPath2\.portalconfig\websitebinding.yml" --outputFile "$PortalPath2\.portalconfig\websitebinding.yml" --findText "PowerApps_CP_PortalDomain" --replaceWithText "$env:PowerApps_CP_PortalDomain"
.\ReplaceDataApp.exe --inputFile "$PortalPath2\.portalconfig\websitebinding.yml" --outputFile "$PortalPath2\.portalconfig\websitebinding.yml" --findText "PowerApps_CP_PortalPrimaryDomain" --replaceWithText "$env:PowerApps_CP_PortalPrimaryDomain"

if ($LastExitCode -gt 0) {
	CD $CurrentPath
	throw "An error occurred whilst replacing text in the data file."
}

<# Clean up the PowerApps CLI if exists #>
$CliFolder = (Get-ChildItem -recurse | Where-Object {$_.PSIsContainer -eq $true -and $_.Name -match "Microsoft.PowerApps.CLI"}).Name

if (-Not ([string]::IsNullOrEmpty($CliFolder))) {
	Write-Host Removing $CliFolder
	Remove-Item -Path $CliFolder -Recurse -Force
}

<# Restore the PowerApps CLI #>
Write-Host Restoring $CliFolder
Invoke-Expression "$CurrentPath\..\nuget restore packages.config -ConfigFile $CurrentPath\..\nuget.config -OutputDirectory $CurrentPath -NonInteractive -Force"

$CliFolder = (Get-ChildItem -recurse | Where-Object {$_.PSIsContainer -eq $true -and $_.Name -match "Microsoft.PowerApps.CLI"}).Name

CD "$CurrentPath\$CliFolder\tools"

.\pac.exe auth create --name "release-pipeline" --url "$Url" --username "$Username" --password "$Password"
if ($LastExitCode -gt 0) {
	CD $CurrentPath
	throw "An error occurred whilst connecting to the portal."
}

.\pac.exe paportal upload --path "$PortalPath1"
if ($LastExitCode -gt 0) {
	CD $CurrentPath
	throw "An error occurred whilst uploading the portal content."
}

.\pac.exe auth clear
if ($LastExitCode -gt 0) {
	CD $CurrentPath
	throw "An error occurred whilst clearing the portal connection."
}

CD $CurrentPath

Write-Host
