using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace OpenEntity.Entities
{
    [Serializable]
    public class FieldIsReadOnlyException : DataException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FieldIsReadOnlyException"/> class.
        /// </summary>
        public FieldIsReadOnlyException()
            :this("The field is marked readonly and cannot be changed.")
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FieldIsReadOnlyException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public FieldIsReadOnlyException(string message)
            :base(message)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FieldIsReadOnlyException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public FieldIsReadOnlyException(string message, Exception innerException)
            :base(message, innerException)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FieldIsReadOnlyException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the information to deserialize.</param>
        /// <param name="context">Contextual information about the source or destination.</param>
        /// <exception cref="T:System.InvalidOperationException">
        /// The current type is not a <see cref="T:System.Configuration.ConfigurationException"/> or a <see cref="T:System.Configuration.ConfigurationErrorsException"/>.
        /// </exception>
        protected FieldIsReadOnlyException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
