namespace Sample.MicroServices.BusinessLogic.Services.Sample
{
    using global::Sample.MicroServices.Models.Domain.Sample;

    using Microsoft.Extensions.Logging;

    using Sdk;

    using System;
    using System.Globalization;
    using System.Reflection;

    public class SampleService : MicroService<SampleInput, SampleOutput>, ISampleService
    {
        #region Static Members 

        public const string TextFormat = "Code-{0}";
        public const string DateTimeFormat = "yyyyMMddHHmmss";
        public static readonly DateTime DateTimeNow = DateTime.UtcNow;

        #endregion

        #region Properties

        #endregion

        #region Constructor

        public SampleService(ILoggerFactory loggerFactory) : base(loggerFactory)
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Called by the webhook to do something (create/update example). 
        /// </summary>
        public void DoSomething()
        {
            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

            Output = new SampleOutput
            {
                Id = Input.Id.Value,
                Xyz = Input.Abc + Input.Efg
            };

            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
        }

        #endregion

        #region Private Members

        #endregion

        #region IDisposable Support

        private bool _disposed;

        /// <summary>
        /// Consider disposing any unmanaged resources within the dispose method
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                
            }

            // TODO: free unmanaged resources (unmanaged objects).

            _disposed = true;
        }

        ~SampleService()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
