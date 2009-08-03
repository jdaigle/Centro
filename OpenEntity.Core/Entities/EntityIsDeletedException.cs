using System;
using System.Data;

namespace OpenEntity.Entities
{
    [Serializable]
    public class EntityIsDeletedException : DataException
    {
        public EntityIsDeletedException()
        {
        }
        public EntityIsDeletedException(string message)
            : base(message)
        {
        }
        public EntityIsDeletedException(string message, Exception inner)
            : base(message, inner)
        {
        }
        protected EntityIsDeletedException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
