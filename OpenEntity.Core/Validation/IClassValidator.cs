using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenEntity.Validation
{
    public interface IClassValidator
    {
        /// <exception cref="InvalidStateException">Thrown when invalid values are found on the entity.</exception>
        void AssertValid(object entity);
        IList<InvalidValue> GetInvalidValues(object entity);
        IList<InvalidValue> GetInvalidValues(object entity, string propertyName);
        IList<IRule> GetValidationRules(string propertyName);
        bool HasValidationRules { get; }
    }
}
