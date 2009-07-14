using System;

namespace OpenEntity.Helpers
{
    [Serializable]
    public class PreconditionViolationException : Exception
    {
         /// <summary>
        /// Initializes a new instance of the <see cref="PreconditionViolationException"/> class.
        /// </summary>
        public PreconditionViolationException()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="PreconditionViolationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public PreconditionViolationException(string message)
            : base(message)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="PreconditionViolationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public PreconditionViolationException(string message, Exception inner)
            : base(message, inner)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="PreconditionViolationException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="info"/> parameter is null.
        /// </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">
        /// The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0).
        /// </exception>
        protected PreconditionViolationException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
