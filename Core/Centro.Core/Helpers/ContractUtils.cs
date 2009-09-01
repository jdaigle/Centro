using System;

namespace Centro.Core.Helpers
{
    public static class ContractUtils
    {
        /// <summary>
        /// Requires the specified precondition to return true.
        /// </summary>
        /// <param name="precondition">The precondition bool.</param>
        /// <exception cref="ArgumentException">If the method precodition is violated.</exception>
        public static void Requires(bool precondition)
        {
            if (!precondition)
                throw new PreconditionViolationException("Method precondition violated.");
        }

        /// <summary>
        /// Requires the specified precondition to return true.
        /// </summary>
        /// <param name="precondition">The precondition bool.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <exception cref="ArgumentException">If the method precodition is violated.</exception>
        public static void Requires(bool precondition, string parameterName)
        {
            if (!precondition)
                throw new ArgumentException("Method precondition violated.", parameterName);
        }

        /// <summary>
        /// Requires the specified precondition to return true.
        /// </summary>
        /// <param name="precondition">The precondition bool.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentException">If the method precodition is violated.</exception>
        public static void Requires(bool precondition, string parameterName, string message)
        {
            if (!precondition)
                throw new ArgumentException(message, parameterName);
        }
    }
}