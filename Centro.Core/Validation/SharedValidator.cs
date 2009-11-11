using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Centro.Validation
{
    public class SharedValidator
    {
        private static SharedValidator defaultInstance;
        public static SharedValidator Default
        {
            get
            {
                if (defaultInstance == null)
                    defaultInstance = new SharedValidator();
                return defaultInstance;
            }
        }

        private SharedValidator()
        {
        }

        public IValidator Validator { get; set; }

        internal void AssertValidatorNotNull()
        {
            if (Validator == null)
                throw new InvalidOperationException("Validator is null.");
        }
    }
}
