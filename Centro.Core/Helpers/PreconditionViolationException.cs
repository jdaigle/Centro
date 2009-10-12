namespace Centro.Helpers
{
    [System.Serializable]
    public class PreconditionViolationException : System.Exception
    {
        public PreconditionViolationException()
        {
        }
        public PreconditionViolationException(string message)
            : base(message)
        {
        }
        public PreconditionViolationException(string message, System.Exception inner)
            : base(message, inner)
        {
        }
        protected PreconditionViolationException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
