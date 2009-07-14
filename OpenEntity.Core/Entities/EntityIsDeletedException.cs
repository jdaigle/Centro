using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace OpenEntity.Entities
{
    [Serializable]
    public class EntityIsDeletedException : DataException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityIsDeletedException"/> class.
        /// </summary>
        public EntityIsDeletedException()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityIsDeletedException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public EntityIsDeletedException(string message)
            : base(message)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityIsDeletedException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public EntityIsDeletedException(string message, Exception inner)
            : base(message, inner)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityIsDeletedException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected EntityIsDeletedException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
