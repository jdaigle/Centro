using System;
using System.Data;

namespace Centro.OpenEntity.Entities
{
    [Serializable]
    public class FieldIsReadOnlyException : DataException
    {
        public FieldIsReadOnlyException()
            :this("The field is marked readonly and cannot be changed.")
        {
        }
        public FieldIsReadOnlyException(string message)
            :base(message)
        {
        }
        public FieldIsReadOnlyException(string message, Exception innerException)
            :base(message, innerException)
        {
        }
        protected FieldIsReadOnlyException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
