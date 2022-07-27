<# 
	Example usage: 
		.\ExportPortal_v2.ps1
			-PortalName "walsall-my-account"
			-WebSiteId "0f39bab4-83e1-ec11-bb3d-0022481add31"
			-Url "https://aylos.crm11.dynamics.com/"
			-TenantId "5bbf182b-e07d-4aa4-a752-094a5b8a019c"
			-ServicePrincipalId "af3e5418-d265-40eb-995f-4586a29d5a89"
			-SecureServicePrincipalSecret "***********" or [-ServicePrincipalSecret "secret" -AsPlainText]
	#>

[CmdletBinding(DefaultParameterSetName="AsEncryptedText")]
Param(
	[parameter(Mandatory=$false)][String]$BuildToolsPath = "..",
	[parameter(Mandatory=$false)][String]$EnvironmentName = "none",
	[parameter(Mandatory=$false)][int]$ReplaceData = 1,
	[parameter(Mandatory=$true)][String]$PortalName,
	[parameter(Mandatory=$true)][String]$WebSiteId,
	[parameter(Mandatory=$true)][String]$Url,
	[parameter(Mandatory=$true)][String]$TenantId,
	[parameter(Mandatory=$true)][String]$ServicePrincipalId,
	[parameter(Mandatory=$false, ParameterSetName="AsEncryptedText")][switch]$AsEncryptedText,
	[parameter(Mandatory=$true, ParameterSetName="AsEncryptedText")][SecureString]$SecureServicePrincipalSecret,
	[parameter(Mandatory=$false, ParameterSetName="AsPlainText")][switch]$AsPlainText,
	[parameter(Mandatory=$true, ParameterSetName="AsPlainText")][String]$ServicePrincipalSecret
)

Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")
Write-Host Exporting the portal $WebSiteId 
Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")

$CurrentPath = Get-Location

switch ($PSCmdlet.ParameterSetName)
{
    "AsEncryptedText"
    {
		$BSTR = [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($SecureServicePrincipalSecret)
		$ServicePrincipalSecret = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($BSTR)
    }
}

."$CurrentPath\CommonFunctions.ps1"

<# Initialise variables #>
initializeEnvironmentVariables -envName $EnvironmentName
if (!$env:D365_OrganizationUnitId) {
	CD $CurrentPath
	throw "The global variables have not been initialized."
}

$PortalPath = "..\..\..\..\Portals"
$PortalPath1 = "..\..\Portals\$PortalName"
$PortalPath2 = "..\..\..\Portals\$PortalName"

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

.\pac.exe auth create --name "build-pipeline" --url "$Url" --tenant "$TenantId" --applicationId "$ServicePrincipalId" --clientSecret "$ServicePrincipalSecret"
if ($LastExitCode -gt 0) {
	CD $CurrentPath
	throw "An error occurred whilst connecting to the portal."
}

.\pac.exe paportal download --path "$PortalPath" -id "$WebSiteId" --overwrite
if ($LastExitCode -gt 0) {
	CD $CurrentPath
	throw "An error occurred whilst downloading the portal."
}

.\pac.exe auth clear
if ($LastExitCode -gt 0) {
	CD $CurrentPath
	throw "An error occurred whilst clearing the portal connection."
}

CD $CurrentPath

<# Replaces certain values in the target files with the relevant tokens #>
if ($ReplaceData) {
	Write-Host
	Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")
	Write-Host "Replace key values with tokens"
	Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")

	CD "$BuildToolsPath\ReplaceData\bin\"

	.\ReplaceDataApp.exe --inputFile "$PortalPath2\sitesetting.yml" --outputFile "$PortalPath2\sitesetting.yml" --findText "$env:AAD_B2C_1_ValidIssuers" --replaceWithText "AAD_B2C_1_ValidIssuers"
	.\ReplaceDataApp.exe --inputFile "$PortalPath2\sitesetting.yml" --outputFile "$PortalPath2\sitesetting.yml" --findText "$env:AAD_B2C_1_MetadataAddress" --replaceWithText "AAD_B2C_1_MetadataAddress"
	.\ReplaceDataApp.exe --inputFile "$PortalPath2\sitesetting.yml" --outputFile "$PortalPath2\sitesetting.yml" --findText "$env:AAD_B2C_1_Authority" --replaceWithText "AAD_B2C_1_Authority"
	.\ReplaceDataApp.exe --inputFile "$PortalPath2\sitesetting.yml" --outputFile "$PortalPath2\sitesetting.yml" --findText "$env:AAD_B2C_1_ClientId" --replaceWithText "AAD_B2C_1_ClientId"
	.\ReplaceDataApp.exe --inputFile "$PortalPath2\sitesetting.yml" --outputFile "$PortalPath2\sitesetting.yml" --findText "$env:AAD_B2C_1_RedirectUri" --replaceWithText "AAD_B2C_1_RedirectUri"
	.\ReplaceDataApp.exe --inputFile "$PortalPath2\.portalconfig\websitebinding.yml" --outputFile "$PortalPath2\.portalconfig\websitebinding.yml" --findText "$env:PowerApps_CP_PortalDomain" --replaceWithText "PowerApps_CP_PortalDomain"
	.\ReplaceDataApp.exe --inputFile "$PortalPath2\.portalconfig\websitebinding.yml" --outputFile "$PortalPath2\.portalconfig\websitebinding.yml" --findText "$env:PowerApps_CP_PortalPrimaryDomain" --replaceWithText "PowerApps_CP_PortalPrimaryDomain"

	if ($LastExitCode -gt 0) {
		CD $CurrentPath
		throw "An error occurred whilst replacing text in the data file."
	}

	CD $CurrentPath
}

<# Rename the manifest file #>
if (Test-Path "$PortalPath1\.portalconfig\$(([System.Uri]"$Url").DnsSafeHost)-manifest.yml") {
	Write-Host
	Write-Host "Renaming the manifest file $PortalPath1\.portalconfig\$(([System.Uri]"$Url").DnsSafeHost)-manifest.yml"

	Rename-Item "$PortalPath1\.portalconfig\$(([System.Uri]"$Url").DnsSafeHost)-manifest.yml" "development-manifest.yml"
}

CD $CurrentPath

Write-Host
