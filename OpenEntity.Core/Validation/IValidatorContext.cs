using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace OpenEntity.Validation
{
    public interface IValidatorContext
    {
        string DefaultErrorMessage { get; }
        void AddInvalid(string message);        
        void AddInvalid(string message, string property);
        void AddInvalid<TEntity, TProperty>(string message, Expression<Func<TEntity, TProperty>> property);
        void DisableDefaultErrorMessage();
        IList<InvalidMessage> InvalidMessages { get; }
    }
}