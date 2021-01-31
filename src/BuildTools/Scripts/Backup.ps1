<# 
	Example usage: 
		.\Backup.ps1
			-EnvironmentName "dbf06063-0009-4b43-92dd-f55a3ac5e2f1"
			-BackupLabel "ReleaseName"
			-Username "jsmith@aylos.com"
			-SecurePassword "***********" or [-Password="passcode" -AsPlainText]
	#>

[CmdletBinding(DefaultParameterSetName="AsEncryptedText")]
Param(
	[parameter(Mandatory=$false)][String]$BuildToolsPath = "..",
	[parameter(Mandatory=$false)][String]$Endpoint = "prod",
	[parameter(Mandatory=$true)][String]$EnvironmentName,
	[parameter(Mandatory=$true)][String]$BackupLabel,
	[parameter(Mandatory=$true)][String]$Username,
	[parameter(Mandatory=$false, ParameterSetName="AsEncryptedText")][switch]$AsEncryptedText,
	[parameter(Mandatory=$true, ParameterSetName="AsEncryptedText")][SecureString]$SecurePassword,
	[parameter(Mandatory=$false, ParameterSetName="AsPlainText")][switch]$AsPlainText,
	[parameter(Mandatory=$true, ParameterSetName="AsPlainText")][String]$Password
)

Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")
Write-Host "Backing up the MS Dyn365 CE organisation."
Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")
Write-Host 

[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12

Install-Module -Name Microsoft.PowerApps.Administration.PowerShell -Force

switch ($PSCmdlet.ParameterSetName)
{
    "AsEncryptedText"
    {
		$BSTR = [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($SecurePassword)
		$Password = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($BSTR)
		break
    }
}

Add-PowerAppsAccount -Endpoint "$Endpoint" -Username "$Username" -Password (ConvertTo-SecureString -String "$Password" -AsPlainText -Force)

Get-AdminPowerAppEnvironment -EnvironmentName $EnvironmentName

Get-PowerAppEnvironmentBackups -EnvironmentName $EnvironmentName

if ((Get-PowerAppEnvironmentBackups -EnvironmentName $EnvironmentName) -contains "$BackupLabel") { Write-Host "A backup named $BackupLabel already exists!" }

Backup-PowerAppEnvironment -EnvironmentName $EnvironmentName -BackupRequestDefinition ([pscustomobject]@{ Label = "$BackupLabel"; Notes = "This is an automated backup triggered by the release."})

