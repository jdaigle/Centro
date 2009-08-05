using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenEntity.Validation.Rules
{
    public class CollectionSizeValidator : IValidator, IInitializableValidator<CollectionSizeAttribute>
    {
        public void Initialize(CollectionSizeAttribute parameters)
        {
            throw new NotImplementedException();
        }

        public bool IsValid(object value, IValidatorContext validatorContext)
        {
            throw new NotImplementedException();
        }
    }
}
