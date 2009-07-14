using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenEntity.Mapping
{
    public interface IClassMapping
    {
        string Table { get; }

        Type ClassType { get; }

        IList<PropertyMapping> PropertyMappings { get; }
    }
}
