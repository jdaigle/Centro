using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace OpenEntity.Mapping
{
    public class PropertyMapping
    {
        public string Name { get; set; }
        public PropertyInfo PropertyInfo { get; set; }
        public string Column { get; set; }
    }
}
