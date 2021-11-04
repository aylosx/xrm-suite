<# 
	Example usage: 
		.\ExtractReferenceData_v2.ps1 
			-Url "https://aylos.crm11.dynamics.com/"
			-TenantId "5bbf182b-e07d-4aa4-a752-094a5b8a019c"
			-ServicePrincipalId "af3e5418-d265-40eb-995f-4586a29d5a89"
			-SecureServicePrincipalSecret "***********" or [-ServicePrincipalSecret "secret" -AsPlainText]
	#>

[CmdletBinding(DefaultParameterSetName="AsEncryptedText")]
Param(
	[parameter(Mandatory=$false)][String]$BuildToolsPath = "..",
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

$DataFolder = "..\..\..\Metadata\Reference"
$DataFile = "$DataFolder\data\data.xml"

<# Export Data #>
Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")
Write-Host "Exporting reference data"
Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")

CD "$BuildToolsPath\ExportData\bin\"

.\ExportDataApp.exe --organizationUrl "$Url" --tenantId "$TenantId" --servicePrincipalId "$ServicePrincipalId" --servicePrincipalSecret "$ServicePrincipalSecret" --schemaFile "$DataFolder\schema.xml" --outputFile "$DataFolder\data.zip"

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
