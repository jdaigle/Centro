using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace OpenEntity.Entities
{
    [Serializable]
    public class EntityOutOfSyncException : DataException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityOutOfSyncException"/> class.
        /// </summary>
        public EntityOutOfSyncException()
            : base("The entity is out of sync with its data in the database. Refetch this entity before using this in-memory instance.")
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityOutOfSyncException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public EntityOutOfSyncException(string message)
            :base(message)
        {        
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityOutOfSyncException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public EntityOutOfSyncException(string message, Exception inner)
            : base(message, inner)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityOutOfSyncException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the information to deserialize.</param>
        /// <param name="context">Contextual information about the source or destination.</param>
        /// <exception cref="T:System.InvalidOperationException">
        /// The current type is not a <see cref="T:System.Configuration.ConfigurationException"/> or a <see cref="T:System.Configuration.ConfigurationErrorsException"/>.
        /// </exception>
        protected EntityOutOfSyncException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
