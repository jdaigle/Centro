using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenEntity.Validation
{
    [Serializable]
    public class InvalidStateException : InvalidOperationException
    {
        public InvalidStateException(IList<InvalidValue> invalidValues)
        {
            InvalidValues = invalidValues;
        }
        public InvalidStateException(IList<InvalidValue> invalidValues, string message)
            : base(message)
        {
            InvalidValues = invalidValues;
        }
        public InvalidStateException(IList<InvalidValue> invalidValues, string message, Exception inner)
            : base(message, inner)
        {
            InvalidValues = invalidValues;
        }
        protected InvalidStateException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
        public IList<InvalidValue> InvalidValues { get; private set; }
    }
}
