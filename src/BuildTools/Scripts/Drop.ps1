<# 
	Example usage: 
		.\Drop.ps1 
	#>

Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")
Write-Host "Copying the artifacts required by the release to the drop folder."
Write-Host "*".PadRight($Host.UI.RawUI.WindowSize.Width, "*")
Write-Host 

$CurrentPath = Get-Location

Remove-Item "$CurrentPath\..\..\Drop\*" -Recurse -Force

Robocopy.exe "$CurrentPath\.." "$CurrentPath\..\..\Drop\BuildTools" *.exe *.dll *.ps1 *.bat *.config /COPY:DAT /E /PURGE /XD sdk obj domain properties services .nuget .tt .vs Logs *.BuildTools.* coretools *.CrmSvcUtilExtensions /XF packages.config app.config *.pdb nuget.*
Robocopy.exe "$CurrentPath\..\..\Metadata\Config" "$CurrentPath\..\..\Drop\Metadata\Config" data.zip /COPY:DAT /E /PURGE /XD data* .nuget .tt .vs
Robocopy.exe "$CurrentPath\..\..\Metadata\Reference" "$CurrentPath\..\..\Drop\Metadata\Reference" data.zip /COPY:DAT /E /PURGE /XD data* .nuget .tt .vs
Robocopy.exe "$CurrentPath\..\..\Metadata\Solutions\Managed" "$CurrentPath\..\..\Drop\Metadata\Solutions\Managed" *.zip /COPY:DAT /E /PURGE /XD *
Robocopy.exe "$CurrentPath\..\..\Metadata\Solutions\Unmanaged" "$CurrentPath\..\..\Drop\Metadata\Solutions\Unmanaged" *.zip /COPY:DAT /E /PURGE /XD *

CD $CurrentPath

Write-Host
