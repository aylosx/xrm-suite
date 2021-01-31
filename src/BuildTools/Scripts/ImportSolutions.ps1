<# 
	Example usage: 
		.\ImportSolutions.ps1 
			-Url="https://aylos.crm11.dynamics.com/"
			-Username="jsmith@aylos.com"
			-SecurePassword="***********" or [-Password="passcode" -AsPlainText]
	#>

[CmdletBinding(DefaultParameterSetName="AsEncryptedText")]
Param(
	[parameter(Mandatory=$false)][String]$BuildToolsPath = "..",
	[parameter(Mandatory=$false)][String]$SolutionType = "Managed",
	[parameter(Mandatory=$true)][String]$Url,
	[parameter(Mandatory=$true)][String]$Username,
	[parameter(Mandatory=$false, ParameterSetName="AsEncryptedText")][switch]$AsEncryptedText,
	[parameter(Mandatory=$true, ParameterSetName="AsEncryptedText")][SecureString]$SecurePassword,
	[parameter(Mandatory=$false, ParameterSetName="AsPlainText")][switch]$AsPlainText,
	[parameter(Mandatory=$true, ParameterSetName="AsPlainText")][String]$Password
)

Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")
Write-Host Importing the $SolutionType.ToLower() solutions
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

.\ImportSolution.ps1 -SolutionType $SolutionType -SolutionName "AylosSchema" -InputFile "..\..\..\Metadata\Solutions\$SolutionType\AylosSchema.zip" -ConnectionTimeout 300 -PollingInterval 10 -PollingTimeout 3600 -Url "$Url" -Username "$Username" -Password "$Password" -AsPlainText

.\ImportSolution.ps1 -SolutionType $SolutionType -SolutionName "AylosSecurity" -InputFile "..\..\..\Metadata\Solutions\$SolutionType\AylosSecurity.zip" -ConnectionTimeout 300 -Url "$Url" -Username "$Username" -Password "$Password" -AsPlainText

.\ImportSolution.ps1 -SolutionType $SolutionType -SolutionName "AylosAnalytics" -InputFile "..\..\..\Metadata\Solutions\$SolutionType\AylosAnalytics.zip" -ConnectionTimeout 300 -Url "$Url" -Username "$Username" -Password "$Password" -AsPlainText

.\ImportSolution.ps1 -SolutionType $SolutionType -SolutionName "AylosTemplates" -InputFile "..\..\..\Metadata\Solutions\$SolutionType\AylosTemplates.zip" -ConnectionTimeout 300 -Url "$Url" -Username "$Username" -Password "$Password" -AsPlainText

.\ImportSolution.ps1 -SolutionType $SolutionType -SolutionName "AylosExtensions" -InputFile "..\..\..\Metadata\Solutions\$SolutionType\AylosExtensions.zip" -ConnectionTimeout 300 -Url "$Url" -Username "$Username" -Password "$Password" -AsPlainText

.\ImportSolution.ps1 -SolutionType $SolutionType -SolutionName "AylosProcesses" -InputFile "..\..\..\Metadata\Solutions\$SolutionType\AylosProcesses.zip" -ConnectionTimeout 300 -Url "$Url" -Username "$Username" -Password "$Password" -AsPlainText

.\ImportSolution.ps1 -SolutionType $SolutionType -SolutionName "AylosUserInterface" -InputFile "..\..\..\Metadata\Solutions\$SolutionType\AylosUserInterface.zip" -ConnectionTimeout 300 -Url "$Url" -Username "$Username" -Password "$Password" -AsPlainText
