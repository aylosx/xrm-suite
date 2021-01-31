namespace Aylos.Xrm.Sdk.Exceptions
{
	using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class BusinessException : Exception
    {
        protected BusinessException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            
        }

        public BusinessException()
        {
        }

        public BusinessException(string message) : base(message)
        {
        }

        public BusinessException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
