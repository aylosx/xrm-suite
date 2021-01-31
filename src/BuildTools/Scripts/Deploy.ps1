<# 
	Example usage: 
		.\Deploy.ps1
			-Url="https://aylos.crm11.dynamics.com/"
			-Username="jsmith@aylos.com"
			-SecurePassword="***********" or [-Password="passcode" -AsPlainText]
	#>

[CmdletBinding(DefaultParameterSetName="AsEncryptedText")]
Param(
	[parameter(Mandatory=$false)][String]$BuildToolsPath = "..",
	[parameter(Mandatory=$false)][String]$EnvironmentName = "none",
	[parameter(Mandatory=$false)][String]$SolutionType = "Managed",
	[parameter(Mandatory=$true)][String]$Url,
	[parameter(Mandatory=$true)][String]$Username,
	[parameter(Mandatory=$false, ParameterSetName="AsEncryptedText")][switch]$AsEncryptedText,
	[parameter(Mandatory=$true, ParameterSetName="AsEncryptedText")][SecureString]$SecurePassword,
	[parameter(Mandatory=$false, ParameterSetName="AsPlainText")][switch]$AsPlainText,
	[parameter(Mandatory=$true, ParameterSetName="AsPlainText")][String]$Password
)

Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")
Write-Host "Deploying the components and the artifacts required by the release."
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

.\ImportSolutions.ps1 -SolutionType $SolutionType -Url "$Url" -Username "$Username" -Password "$Password" -AsPlainText

.\ImportConfigData.ps1 -EnvironmentName $EnvironmentName -Url "$Url" -Username "$Username" -Password "$Password" -AsPlainText

.\ImportReferenceData.ps1 -Url "$Url" -Username "$Username" -Password "$Password" -AsPlainText
