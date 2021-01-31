namespace Aylos.Xrm.Sdk.AzureFunctions
{
    using Microsoft.Xrm.Sdk;
    using System;
    using System.Globalization;
    using System.Net;
    using System.ServiceModel;
    using System.Text;

    public static class ExceptionHelper
    {
        public static HttpStatusCode HandleException(Exception exception, StringBuilder text, ILogger logger)
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception));
            if (text == null) throw new ArgumentNullException(nameof(text));
            if (logger == null) throw new ArgumentNullException(nameof(logger));

            text.AppendLine(exception.Message);
            text.AppendLine(exception.StackTrace);
            text.AppendLine();

            if (exception is FaultException<OrganizationServiceFault> fault)
            {
                if (fault.Detail != null)
                {
                    text.AppendLine(string.Format(CultureInfo.InvariantCulture, TraceMessages.ExceptionErrorCode, fault.Detail.ErrorCode));
                    text.AppendLine(string.Format(CultureInfo.InvariantCulture, TraceMessages.ExceptionMessage, fault.Detail.Message));
                    text.AppendLine(string.Format(CultureInfo.InvariantCulture, TraceMessages.ExceptionTrace, fault.Detail.TraceText));
                    foreach (var errorDetail in fault.Detail.ErrorDetails)
                    {
                        text.AppendLine(string.Format(CultureInfo.InvariantCulture, TraceMessages.ExceptionDetailKey, errorDetail.Key, errorDetail.Value));
                    }
                }
                logger.Error(text.ToString());
                return HttpStatusCode.InternalServerError;
            }
            else if (exception is ApplicationException)
            {
                if (exception.InnerException != null) return HandleException(exception.InnerException, text, logger);
                logger.Error(text.ToString());
                return HttpStatusCode.ExpectationFailed;
            }
            else if (exception is NotImplementedException)
            {
                if (exception.InnerException != null) return HandleException(exception.InnerException, text, logger);
                logger.Error(text.ToString());
                return HttpStatusCode.NotImplemented;
            }
            else
            {
                if (exception.InnerException != null) return HandleException(exception.InnerException, text, logger);
                logger.Error(text.ToString());
                return HttpStatusCode.BadRequest;
            }
        }
    }
}
