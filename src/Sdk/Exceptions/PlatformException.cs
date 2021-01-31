namespace Aylos.Xrm.Sdk.Exceptions
{
	using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class PlatformException : Exception
    {
        protected PlatformException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public PlatformException()
        {
        }

        public PlatformException(string message) : base(message)
        {
        }

        public PlatformException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
