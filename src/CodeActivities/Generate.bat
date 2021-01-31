@ECHO OFF
REM *************************************************************
REM **                                                         **
REM ** Generate the CRM Organization Service Context           ** 
REM **                                                         **
REM ** How to use the command:                                 **
REM **  1. Run CMD.EXE                                         **
REM **  2. Navigate to the folder of the Generate command      **
REM **  3. Type "generate username password" and press enter   **
REM **                                                         **
REM **  The command will loop all the Domain subfolders        **
REM **  under the upper folder and call the Generate           **
REM **  command that exists in the Domain folder.              **
REM **                                                         **
REM *************************************************************

SET mypath=%CD%
CD..
FOR /R /D %%s IN (*Domain) DO (
	ECHO -----------------------------------------------------------------------------------------------------------------------------------
	CD %%s
	ECHO %%s
	IF EXIST Generate.bat (
		CALL Generate.bat %1 %2
	) ELSE (
		ECHO The 'Generate.bat' file does not exist.
	)
	CD %mypath%
)
ECHO -----------------------------------------------------------------------------------------------------------------------------------
ECHO ON