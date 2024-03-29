﻿<# 
	Example usage: 
		.\Backup.ps1
			-EnvironmentName "dbf06063-0009-4b43-92dd-f55a3ac5e2f1"
			-BackupLabel "ReleaseName"
			-TenantId "5bbf182b-e07d-4aa4-a752-094a5b8a019c"
			-ServicePrincipalId "af3e5418-d265-40eb-995f-4586a29d5a89"
			-SecureServicePrincipalSecret "***********" or [-ServicePrincipalSecret "secret" -AsPlainText]
	#>

[CmdletBinding(DefaultParameterSetName="AsEncryptedText")]
Param(
	[parameter(Mandatory=$false)][String]$BuildToolsPath = "..",
	[parameter(Mandatory=$false)][String]$Endpoint = "prod",
	[parameter(Mandatory=$true)][String]$EnvironmentName,
	[parameter(Mandatory=$true)][String]$BackupLabel,
	[parameter(Mandatory=$true)][String]$TenantId,
	[parameter(Mandatory=$true)][String]$ServicePrincipalId,
	[parameter(Mandatory=$false, ParameterSetName="AsEncryptedText")][switch]$AsEncryptedText,
	[parameter(Mandatory=$true, ParameterSetName="AsEncryptedText")][SecureString]$SecureServicePrincipalSecret,
	[parameter(Mandatory=$false, ParameterSetName="AsPlainText")][switch]$AsPlainText,
	[parameter(Mandatory=$true, ParameterSetName="AsPlainText")][String]$ServicePrincipalSecret
)

Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")
Write-Host "Backing up the Dataverse organisation."
Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")
Write-Host 

[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12

Install-Module -Name Microsoft.PowerApps.Administration.PowerShell -Force -AllowClobber

switch ($PSCmdlet.ParameterSetName)
{
    "AsEncryptedText"
    {
		$BSTR = [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($SecureServicePrincipalSecret)
		$ServicePrincipalSecret = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($BSTR)
    }
}

Add-PowerAppsAccount -Endpoint "$Endpoint" -TenantID "$TenantId" -ApplicationId "$ServicePrincipalId" -ClientSecret "$ServicePrincipalSecret" -Verbose

Get-AdminPowerAppEnvironment -EnvironmentName $EnvironmentName

Get-PowerAppEnvironmentBackups -EnvironmentName $EnvironmentName

if ((Get-PowerAppEnvironmentBackups -EnvironmentName $EnvironmentName) -contains "$BackupLabel") { Write-Host "A backup named $BackupLabel already exists!" }

Backup-PowerAppEnvironment -EnvironmentName $EnvironmentName -BackupRequestDefinition ([pscustomobject]@{ Label = "$BackupLabel"; Notes = "This is an automated backup triggered by the release."})
