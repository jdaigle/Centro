using System;
using System.Data;

namespace Centro.OpenEntity.Entities
{
    [Serializable]
    public class ValueTypeMismatchException : DataException
    {
        public ValueTypeMismatchException(string message)
            :base(message)
        {
        }
        public ValueTypeMismatchException()
        {
        }
        public ValueTypeMismatchException(string message, Exception innerException)
            :base(message, innerException)
        {
        }
        protected ValueTypeMismatchException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
