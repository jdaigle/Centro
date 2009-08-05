using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenEntity.Validation
{
    [Serializable]
    public class InvalidValue
    {
        public InvalidValue(string message, Type entityType, string propertyName, object value, object entity)
        {
            Message = message;
            EntityType = entityType;
            PropertyName = propertyName;
            Value = value;
            Entity = entity;
        }
             
        public object Entity { get; private set; }
        public Type EntityType { get; private set; }
        public string Message { get; private set; }
        public string PropertyName { get; private set; }
        public object Value { get; private set; }


        public override string ToString()
        {
            return PropertyName + "[" + Message + "]";
        }
    }


}
