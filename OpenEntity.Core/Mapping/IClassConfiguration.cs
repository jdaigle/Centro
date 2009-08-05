using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace OpenEntity.Mapping
{
    public interface IClassConfiguration
    {
        string Table { get; }
        Type ClassType { get; }
        IList<PropertyConfiguration> Properties { get; }
    }
}
