<# 
	Example usage: 
		.\ExportSolution.ps1
			-SolutionName "MySolution"
			-SolutionType 0 
			-OutputPath "..\..\Metadata\Solutions\Unmanaged"
			-Url="https://aylos.crm11.dynamics.com/"
			-Username="jsmith@aylos.com"
			-SecurePassword="***********" or [-Password="passcode" -AsPlainText]
	#>

[CmdletBinding(DefaultParameterSetName="AsEncryptedText")]
Param(
	[parameter(Mandatory=$false)][String]$BuildToolsPath = "..",
	[parameter(Mandatory=$true)][String]$SolutionName,
	[parameter(Mandatory=$true)][int]$SolutionType,
	[parameter(Mandatory=$true)][String]$OutputPath,
	[parameter(Mandatory=$true)][int]$ConnectionTimeout,
	[parameter(Mandatory=$true)][String]$Url,
	[parameter(Mandatory=$true)][String]$Username,
	[parameter(Mandatory=$false, ParameterSetName="AsEncryptedText")][switch]$AsEncryptedText,
	[parameter(Mandatory=$true, ParameterSetName="AsEncryptedText")][SecureString]$SecurePassword,
	[parameter(Mandatory=$false, ParameterSetName="AsPlainText")][switch]$AsPlainText,
	[parameter(Mandatory=$true, ParameterSetName="AsPlainText")][String]$Password
)

Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")
Write-Host Exporting the solutions following the pattern $SolutionName 
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

$ConnectionString = "AuthType=Office365;Username=$Username;Password=$Password;Url=$Url"

CD "$BuildToolsPath\ExportSolution\bin\"

.\ExportSolutionApp.exe --connectionString "$ConnectionString" --solutionName "$SolutionName" --solutionType $SolutionType --outputPath "$OutputPath" --connectionTimeout $ConnectionTimeout

if ($LastExitCode -gt 0) {
	CD $CurrentPath
	throw "An error occurred whilst exporting the $SolutionName solution."
	}

CD $CurrentPath

Write-Host
