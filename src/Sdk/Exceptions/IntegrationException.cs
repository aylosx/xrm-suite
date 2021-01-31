namespace Aylos.Xrm.Sdk.Exceptions
{
	using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class IntegrationException : Exception
    {
        protected IntegrationException(SerializationInfo info, StreamingContext context) : base(info, context)
        { 
        }

        public IntegrationException()
        {
        }

        public IntegrationException(string message) : base(message)
        {
        }

        public IntegrationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
