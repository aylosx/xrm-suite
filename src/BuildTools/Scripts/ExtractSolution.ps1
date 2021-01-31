<# 
	Example usage: 
		.\ExtractSolution.ps1
			-SolutionName "MySolution"
	#>

Param(
	[parameter(Mandatory=$false)][String]$BuildToolsPath = "..",
	[parameter(Mandatory=$false)][String]$CoreToolsPath = "..\..\..\Tools\CoreTools",
	[parameter(Mandatory=$false)][String]$SolutionsPath = "..\..\Metadata\Solutions",
	[parameter(Mandatory=$true)][String]$SolutionName
)

<# Unmanaged solutions #>
Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")
Write-Host Extracting the $SolutionName unmanaged solution
Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")

Invoke-Expression "$CoreToolsPath\SolutionPackager.exe /action:extract /allowWrite:Yes /allowDelete:Yes /clobber /l:$SolutionsPath\Unmanaged\$SolutionName.log /errorlevel:Verbose /z:$SolutionsPath\Unmanaged\$SolutionName.zip /f:$SolutionsPath\Unmanaged\$SolutionName"

if ($LastExitCode -gt 0) {
	CD $CurrentPath
	throw "An error occurred whilst extracting the $SolutionName unmanaged solution."
	}

Write-Host

<# Managed solution #>
Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")
Write-Host Extracting the $SolutionName managed solution
Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")

Invoke-Expression "$CoreToolsPath\SolutionPackager.exe /action:extract /allowWrite:Yes /allowDelete:Yes /clobber /l:$SolutionsPath\Managed\$SolutionName.log /errorlevel:Verbose /z:$SolutionsPath\Managed\$SolutionName.zip /f:$SolutionsPath\Managed\$SolutionName"

if ($LastExitCode -gt 0) {
	CD $CurrentPath
	throw "An error occurred whilst extracting the $SolutionName managed solution."
	}

Write-Host
