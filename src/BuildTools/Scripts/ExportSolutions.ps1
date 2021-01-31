<# 
	Example usage: 
		.\ExportSolutions.ps1
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

Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")
Write-Host Updating the version of the solutions
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

<# Unmanaged solutions #>
.\ExportSolution.ps1 -SolutionName "YourOrganisationName*" -SolutionType 0 -OutputPath "..\..\..\Metadata\Solutions\Unmanaged" -ConnectionTimeout 600 -Url "$Url" -Username "$Username" -Password "$Password" -AsPlainText

<# Managed solutions #>
.\ExportSolution.ps1 -SolutionName "YourOrganisationName*" -SolutionType 1 -OutputPath "..\..\..\Metadata\Solutions\Managed" -ConnectionTimeout 600 -Url "$Url" -Username "$Username" -Password "$Password" -AsPlainText
