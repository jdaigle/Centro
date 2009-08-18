using System;
using System.Data;

namespace OpenEntity.Query
{
    [Serializable]
    public class QueryGenerationException : DataException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryGenerationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public QueryGenerationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryGenerationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public QueryGenerationException(string message, Exception inner)
            :base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryGenerationException"/> class.
        /// </summary>
        public QueryGenerationException()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryGenerationException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the information to deserialize.</param>
        /// <param name="context">Contextual information about the source or destination.</param>
        /// <exception cref="T:System.InvalidOperationException">
        /// The current type is not a <see cref="T:System.Configuration.ConfigurationException"/> or a <see cref="T:System.Configuration.ConfigurationErrorsException"/>.
        /// </exception>
        protected QueryGenerationException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
