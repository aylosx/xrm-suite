<# 
	Example usage: 
		.\PackSolutions.ps1
	#>

Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")
Write-Host Unpacking the solutions
Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")
Write-Host 

.\PackSolution.ps1 -SolutionName "AylosSchema"
.\PackSolution.ps1 -SolutionName "AylosSecurity"
.\PackSolution.ps1 -SolutionName "AylosAnalytics"
.\PackSolution.ps1 -SolutionName "AylosTemplates"
.\PackSolution.ps1 -SolutionName "AylosExtensions"
.\PackSolution.ps1 -SolutionName "AylosProcesses"
.\PackSolution.ps1 -SolutionName "AylosUserInterface"
