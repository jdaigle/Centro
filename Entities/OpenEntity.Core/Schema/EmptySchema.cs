using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Centro.OpenEntity.Schema
{
    /// <summary>
    /// An internal implementation of an empty schema.
    /// </summary>
    internal class EmptySchema : BaseSchema
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmptySchema"/> class.
        /// </summary>
        protected internal EmptySchema()
        {
        }

        /// <summary>
        /// Initializes the schema.
        /// </summary>
        protected override void InitSchema()
        {
            // no-op
        }
    }
}
