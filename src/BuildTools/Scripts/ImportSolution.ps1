<# 
	Example usage: 
		.\ImportSolution.ps1 
			-SolutionName "MySolution"
			-InputFile "..\..\Metadata\Solutions\Managed\MySolution.zip" 
			-Url="https://aylos.crm11.dynamics.com/"
			-Username="jsmith@aylos.com"
			-SecurePassword="***********" or [-Password="passcode" -AsPlainText]
	#>

[CmdletBinding(DefaultParameterSetName="AsEncryptedText")]
Param(
	[parameter(Mandatory=$false)][String]$BuildToolsPath = "..",
	[parameter(Mandatory=$false)][String]$SolutionType = "Managed",
	[parameter(Mandatory=$true)][String]$SolutionName,
	[parameter(Mandatory=$true)][String]$InputFile,
	[parameter(Mandatory=$true)][int]$ConnectionTimeout,
	[parameter(Mandatory=$false)][int]$PollingInterval = 3,
	[parameter(Mandatory=$false)][int]$PollingTimeout = 600,
	[parameter(Mandatory=$true)][String]$Url,
	[parameter(Mandatory=$true)][String]$Username,
	[parameter(Mandatory=$false, ParameterSetName="AsEncryptedText")][switch]$AsEncryptedText,
	[parameter(Mandatory=$true, ParameterSetName="AsEncryptedText")][SecureString]$SecurePassword,
	[parameter(Mandatory=$false, ParameterSetName="AsPlainText")][switch]$AsPlainText,
	[parameter(Mandatory=$true, ParameterSetName="AsPlainText")][String]$Password
)

Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")
Write-Host Importing the solution $SolutionName 
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

$HoldingSolution = switch ($SolutionType.ToLower())
{
	"managed" { $true }
	"unmanaged" { $false }
}

Write-Host $HoldingSolution 

$ConnectionString = "AuthType=Office365;Username=$Username;Password=$Password;Url=$Url"

CD "$BuildToolsPath\ImportSolution\bin\"

.\ImportSolutionApp.exe --connectionString "$ConnectionString" --solutionName "$SolutionName" --inputFile "$InputFile" --holdingSolution $HoldingSolution --connectionTimeout $ConnectionTimeout --pollingInterval $PollingInterval --pollingTimeout $PollingTimeout

if ($LastExitCode -gt 0) {
	CD $CurrentPath
	throw "An error occurred whilst importing the $SolutionName solution."
	}

CD $CurrentPath

Write-Host
