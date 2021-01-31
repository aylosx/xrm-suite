namespace Aylos.Xrm.Sdk.ConsoleApps
{
    using Aylos.Xrm.Sdk.Common;
    using Microsoft.Xrm.Sdk;
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.ServiceModel;

    public sealed class ExceptionHandlingService : ConsoleApp
    {
        string SystemTypeName { get { return GetType().UnderlyingSystemType.Name; } }

        public Exception Exception { get; private set; }

        public ExitCode ExitCode { get; private set; }

        public ExceptionHandlingService(Exception exception)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            Exception = exception ?? throw new ArgumentNullException(nameof(exception));

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        public ExitCode GetExitCode()
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            Logger.Error(Exception);

            void le(Exception n) { Logger.Error(n); if (n.InnerException != null) le(n.InnerException); }

            le(Exception);

            if (Exception is InvalidPluginExecutionException)
            {
                ExitCode = ExitCode.ExecutionError;
            }
            else if (Exception is FaultException<OrganizationServiceFault>)
            {
                var ex = (FaultException<OrganizationServiceFault>)Exception;
                if (ex.Detail != null)
                {
                    Logger.Error(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExceptionErrorCode, ex.Detail.ErrorCode));
                    Logger.Error(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExceptionMessage, ex.Detail.Message));
                    Logger.Error(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExceptionTrace, ex.Detail.TraceText));
                    foreach (var errorDetail in ex.Detail.ErrorDetails)
                    {
                        Logger.Error(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExceptionDetailKey, errorDetail.Key, errorDetail.Value));
                    }
                }
                ExitCode = ExitCode.OrganizationServiceFault;
            }
            else if (Exception is ArgumentException)
            {
                ExitCode = ExitCode.ArgumentSyntaxError;
            }
            else if (Exception is FormatException)
            {
                ExitCode = ExitCode.ArgumentSyntaxError;
            }
            else if (Exception is NotImplementedException)
            {
                ExitCode = ExitCode.NotImplemented;
            }
            else
            {
                ExitCode = ExitCode.ExecutionError;
            }

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            return ExitCode;
        }
    }
}