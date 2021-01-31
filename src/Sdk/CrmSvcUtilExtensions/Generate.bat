@ECHO OFF
REM *************************************************************
REM **                                                         **
REM ** Generate the CRM Organization Service Context           ** 
REM **                                                         **
REM ** How to use the command:                                 **
REM **  1. Run CMD.EXE                                         **
REM **  2. Navigate to the folder of the Generate command      **
REM **  3. Type "generate username password [domain]"          **
REM **	   and press enter                                     **
REM **                                                         **
REM **  The executable will make use of the Entities.xml       **
REM **  and generate the CrmServiceContext.cs file             **
REM **                                                         **
REM *************************************************************

CLS
ECHO Generating Early Bound Entity Code

REM Use the below command for D365 On-Premise
REM crmsvcutil.exe /namespace:"Shared.Models.Domain" /out:"CrmServiceContext.cs" /servicecontextname:"CrmServiceContext" /url:"https://aylos.crm11.dynamics.com/xrmservices/2011/organization.svc" /username:%1 /password:%2 /domain:%3 > CrmSvcUtil.exe.output.log

REM Use the below command for D365 Online
crmsvcutil.exe /namespace:"Shared.Models.Domain" /out:"CrmServiceContext.cs" /servicecontextname:"CrmServiceContext" /interactivelogin:true > CrmSvcUtil.exe.output.log

ECHO Logs have been saved to the CrmSvcUtil.exe.output.log file
