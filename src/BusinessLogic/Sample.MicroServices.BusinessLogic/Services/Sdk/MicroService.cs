namespace Sample.MicroServices.BusinessLogic.Services.Sdk
{
    using Microsoft.Azure.WebJobs.Logging;
    using Microsoft.Extensions.Logging;

    using System;
    using System.Net.Http;

    public abstract class MicroService
    {
        #region Constructor

        public MicroService(ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));

            Logger = loggerFactory.CreateLogger(LogCategories.CreateFunctionUserCategory(UnderlyingSystemTypeName));
        }

        #endregion

        #region Properties

        public ILogger Logger { get; set; }

        public ILoggerFactory LoggerFactory { get; set; }

        public HttpRequestMessage HttpRequestMessage { get; protected set; }

        public string UnderlyingSystemTypeName { get { return GetType().UnderlyingSystemType.Name; } }

        #endregion
    }

    public abstract class MicroService<I, O> : MicroService
    {
        #region Constructor

        public MicroService(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }

        #endregion

        #region Properties

        public I Input { get; set; }

        public O Output { get; set; }

        #endregion
    }
}
