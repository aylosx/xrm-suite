<# 
	Example usage: 
		.\ExtractSolutions.ps1
	#>

Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")
Write-Host Unpacking the solutions
Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")
Write-Host 

.\ExtractSolution.ps1 -SolutionName "YourOrganisationNameSchema"
.\ExtractSolution.ps1 -SolutionName "YourOrganisationNameSecurity"
.\ExtractSolution.ps1 -SolutionName "YourOrganisationNameAnalytics"
.\ExtractSolution.ps1 -SolutionName "YourOrganisationNameTemplates"
.\ExtractSolution.ps1 -SolutionName "YourOrganisationNameExtensions"
.\ExtractSolution.ps1 -SolutionName "YourOrganisationNameProcesses"
.\ExtractSolution.ps1 -SolutionName "YourOrganisationNameUserInterface"
