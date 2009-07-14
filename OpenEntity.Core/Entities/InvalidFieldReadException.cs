using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace OpenEntity.Entities
{
    [Serializable]
    public class InvalidFieldReadException : DataException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidFieldReadException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public InvalidFieldReadException(string message)
            : base(message)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidFieldReadException"/> class.
        /// </summary>
        public InvalidFieldReadException()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidFieldReadException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public InvalidFieldReadException(string message, Exception innerException)
            :base(message, innerException)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidFieldReadException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the information to deserialize.</param>
        /// <param name="context">Contextual information about the source or destination.</param>
        /// <exception cref="T:System.InvalidOperationException">
        /// The current type is not a <see cref="T:System.Configuration.ConfigurationException"/> or a <see cref="T:System.Configuration.ConfigurationErrorsException"/>.
        /// </exception>
        protected InvalidFieldReadException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
