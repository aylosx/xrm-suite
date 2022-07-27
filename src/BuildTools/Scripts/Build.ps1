<# 
	Example usage: 
		.\Build.ps1
			-Url="https://aylos.crm11.dynamics.com/"
			-Username="jsmith@aylos.com"
			-SecurePassword="***********" or [-Password="passcode" -AsPlainText]
	#>

[CmdletBinding(DefaultParameterSetName="AsEncryptedText")]
Param(
	[parameter(Mandatory=$false)][String]$BuildToolsPath = "..",
	[parameter(Mandatory=$false)][String]$EnvironmentName = "none",
	[parameter(Mandatory=$false)][int]$PublishCustomizations = 0,
	[parameter(Mandatory=$true)][String]$Url,
	[parameter(Mandatory=$true)][String]$Username,
	[parameter(Mandatory=$false, ParameterSetName="AsEncryptedText")][switch]$AsEncryptedText,
	[parameter(Mandatory=$true, ParameterSetName="AsEncryptedText")][SecureString]$SecurePassword,
	[parameter(Mandatory=$false, ParameterSetName="AsPlainText")][switch]$AsPlainText,
	[parameter(Mandatory=$true, ParameterSetName="AsPlainText")][String]$Password
)

Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")
Write-Host "Building the components and preparing all the artifacts required by the release."
Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")
Write-Host 

switch ($PSCmdlet.ParameterSetName)
{
    "AsEncryptedText"
    {
		$BSTR = [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($SecurePassword)
		$Password = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($BSTR)
    }
}

<# By default we are not publishing the customisations #>
if ($PublishCustomizations) {
	.\PublishCustomizations.ps1 -Url "$Url" -Username "$Username" -Password "$Password" -AsPlainText
}

.\UpdateSolutionVersions.ps1 -Url "$Url" -Username "$Username" -Password "$Password" -AsPlainText

.\ExportSolutions.ps1 -Url "$Url" -Username "$Username" -Password "$Password" -AsPlainText

.\ExtractSolutions.ps1

.\PackSolutions.ps1

.\ExtractConfigData.ps1 -EnvironmentName $EnvironmentName -Url "$Url" -Username "$Username" -Password "$Password" -AsPlainText

.\ExtractReferenceData.ps1 -Url "$Url" -Username "$Username" -Password "$Password" -AsPlainText

.\ExportPortals.ps1 -EnvironmentName $EnvironmentName -Url "$Url" -Username "$Username" -Password "$Password" -AsPlainText
