using System;

namespace Centro.DomainModel
{
    public class DomainModelException : Exception
    {
        public DomainModelException()
        {
        }
        public DomainModelException(string message)
            : base(message)
        {
        }
        public DomainModelException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
