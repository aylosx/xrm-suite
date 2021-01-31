<# 
	Example usage: 
		.\PackSolution.ps1
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

$MappingDirective = ""
if (Test-Path "$SolutionsPath\Unmanaged\$SolutionName.xml") {
	$MappingDirective = "/m:$SolutionsPath\Unmanaged\$SolutionName.xml"
	}

Invoke-Expression "$CoreToolsPath\SolutionPackager.exe /action:Pack /packagetype:Unmanaged /errorlevel:Verbose /l:$SolutionsPath\Unmanaged\$SolutionName.log /z:$SolutionsPath\Unmanaged\$SolutionName.zip /f:$SolutionsPath\Unmanaged\$SolutionName $MappingDirective"

if ($LastExitCode -gt 0) {
	CD $CurrentPath
	throw "An error occurred whilst extracting the $SolutionName unmanaged solution."
	}

Write-Host

<# Managed solution #>
Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")
Write-Host Extracting the $SolutionName managed solution
Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")

$MappingDirective = ""
if (Test-Path "$SolutionsPath\Managed\$SolutionName.xml") {
	$MappingDirective = "/m:$SolutionsPath\Managed\$SolutionName.xml"
	}

Invoke-Expression "$CoreToolsPath\SolutionPackager.exe /action:Pack /packagetype:Managed /errorlevel:Verbose /l:$SolutionsPath\Managed\$SolutionName.log /z:$SolutionsPath\Managed\$SolutionName.zip /f:$SolutionsPath\Managed\$SolutionName $MappingDirective"

if ($LastExitCode -gt 0) {
	CD $CurrentPath
	throw "An error occurred whilst extracting the $SolutionName managed solution."
	}

Write-Host
