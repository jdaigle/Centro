using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Validator.Engine;

namespace Centro.Data.Validation
{
    public class StructureMapSharedEngineProvider : ISharedEngineProvider
    {
        private readonly ValidatorEngine validatorEngine;

        public StructureMapSharedEngineProvider(ValidatorEngine validatorEngine)
        {
            this.validatorEngine = validatorEngine;
        }

        public ValidatorEngine GetEngine()
        {
            return validatorEngine;
        }
    }
}
