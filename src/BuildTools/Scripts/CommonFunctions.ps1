Function initializeEnvironmentVariables($envName) {

	switch ($envName)
	{
		"aylos" 
		{
			$env:D365_AdministratorId = "bc7b9274-7577-4f19-8a47-3cd07f6d4cbb"
			$env:D365_AdministratorName = "Platform Admin"
			$env:D365_ABC_API_Endpoint = "https://web-hook-api-dev.aylos.com/api/dosomething"
			$env:D365_ABCMode = "172400001"
			$env:D365_ABCSecurityValue = "d7d4a2d1-d8bc-47e3-a095-rt5481f5f27c"
			$env:D365_Default_THE_XX1_UserId = "9a35b1e2-85e7-sd23-re34-000d3a86a1cd"
			$env:D365_Default_THE_XX1_UserName = "Jason Smith"
			$env:D365_Default_THE_XX2_UserId = "9a35b1e2-85e7-sd23-re34-000d3a86a1cd"
			$env:D365_Default_THE_XX2_UserName = "Jason Smith"
			$env:D365_THE_API_UserId = "125d13a0-afe7-sd23-re34-000d3a86a85d"
			$env:D365_THE_API_UserName = "THE API User"
			$env:D365_MigrationSupportUserId = "bc7b9274-7577-4f19-8a47-3cd07f6d4cbb"
			$env:D365_MigrationSupportUserName = "Platform Admin"
			$env:D365_OrganizationUnitId = "d7f33d66-e7e4-sd23-re34-000d3a86a6c0"
			$env:D365_OrganizationUnitName = "aylos"
			$env:D365_OrganizationUnitTeamId = "e5f33d66-e7e4-sd23-re34-000d3a86a6c0"
			$env:D365_OrganizationUnitTeamName = "aylos"
			$env:D365_XYZ_API_Endpoint = "https://web-hook-api-dev.aylos.com/api/anotherthing"
			$env:D365_XYZMode = "172400001"
			$env:D365_XYZSecurityValue = "d7d4a2d1-d8bc-47e3-a095-rt5481f5f27c"
			$env:D365_AnotherApiPrefix = "https://another-api-dev.aylos.com/api/somethingelse?giveme="
			$env:Another_ClientApiKey = "ABC1-DEF2-GHI3-JKL4"
			$env:Another_ServerApiKey = "MNO5-PQR6-STU7-VWX8"
			break
		}
	}
}

Function removeCommonFunctions() {
	Remove-Item function:\initializeEnvironmentVariables
	Remove-Item function:\removeCommonFunctions
}