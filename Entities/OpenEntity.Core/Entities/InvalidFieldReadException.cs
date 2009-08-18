using System;
using System.Data;

namespace OpenEntity.Entities
{
    [Serializable]
    public class InvalidFieldReadException : DataException{
        public InvalidFieldReadException(string message)
            : base(message)
        {
        }
        public InvalidFieldReadException()
        {
        }
        public InvalidFieldReadException(string message, Exception innerException)
            :base(message, innerException)
        {
        }
        protected InvalidFieldReadException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
