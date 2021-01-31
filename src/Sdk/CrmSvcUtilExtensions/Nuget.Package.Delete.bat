@ECHO OFF
"..\..\.nuget\nuget.exe" delete Aylos.Xrm.Sdk.CrmSvcUtilExtensions 20.2.2514 -ConfigFile "..\..\.nuget\nuget.config" -Source "%CD:~0,2%\packages-aylos"
