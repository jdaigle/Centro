using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenEntity.Validation
{
    public class InvalidMessage
    {
        public InvalidMessage(string message, string propertyName)
        {
            Message = message ?? string.Empty;
            PropertyName = propertyName ?? string.Empty;
        }
        public string Message { get; private set; }
        public string PropertyName { get; private set; }
    }
}
