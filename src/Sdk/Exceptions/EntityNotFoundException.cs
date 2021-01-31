namespace Aylos.Xrm.Sdk.Exceptions
{
	using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class EntityNotFoundException : Exception
    {
        protected EntityNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(string message) : base(message)
        {
        }

        public EntityNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
