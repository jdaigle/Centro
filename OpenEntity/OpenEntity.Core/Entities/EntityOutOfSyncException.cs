using System;
using System.Data;

namespace Centro.OpenEntity.Entities
{
    [Serializable]
    public class EntityOutOfSyncException : DataException
    {
        public EntityOutOfSyncException()
            : base("The entity is out of sync with its data in the database. Refetch this entity before using this in-memory instance.")
        {
        }
        public EntityOutOfSyncException(string message)
            :base(message)
        {        
        }
        public EntityOutOfSyncException(string message, Exception inner)
            : base(message, inner)
        {
        }
        protected EntityOutOfSyncException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
