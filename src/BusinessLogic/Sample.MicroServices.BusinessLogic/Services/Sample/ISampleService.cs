namespace Sample.MicroServices.BusinessLogic.Services.Sample
{
    using System;

    public interface ISampleService : IDisposable
    {
        /// <summary>
        /// Called by the webhook to do something (create/update example). 
        /// </summary>
        void DoSomething();
    }
}
