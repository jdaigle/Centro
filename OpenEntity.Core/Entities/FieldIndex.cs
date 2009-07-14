using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenEntity.Entities
{
    /// <summary>
    /// Represents a Name/Index tuple.
    /// </summary>
    public class FieldIndex
    {
        /// <summary>
        /// The name of the field used internally for fast-access.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Index/Ordinal of the field used internally for fast-access.
        /// </summary>
        public int Index { get; set; }
    }
}