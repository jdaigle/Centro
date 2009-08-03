using System;
using System.Collections;
using System.Data;
using OpenEntity.Entities;

namespace OpenEntity.DataProviders
{
    [Serializable]
    public class CommandExecutionException : DataException
    {
        public string QueryExecuted { get; private set; }
        public IList Parameters { get; private set; }
        public IEntity EntityInvolved { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandExecutionException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="queryExecuted">The query executed.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="innerException">The inner exception.</param>
        public CommandExecutionException(string message, string queryExecuted, IList parameters,
				Exception innerException)
			: base(message, innerException)
		{
			this.QueryExecuted = queryExecuted;
			this.Parameters = parameters;
		}
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandExecutionException"/> class.
        /// </summary>
        public CommandExecutionException()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandExecutionException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public CommandExecutionException(string message)
            : base(message)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandExecutionException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public CommandExecutionException(string message, Exception inner)
            : base(message, inner)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandExecutionException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the information to deserialize.</param>
        /// <param name="context">Contextual information about the source or destination.</param>
        /// <exception cref="T:System.InvalidOperationException">
        /// The current type is not a <see cref="T:System.Configuration.ConfigurationException"/> or a <see cref="T:System.Configuration.ConfigurationErrorsException"/>.
        /// </exception>
        protected CommandExecutionException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
