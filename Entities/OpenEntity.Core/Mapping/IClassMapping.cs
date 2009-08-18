using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace OpenEntity.Mapping
{
    public interface IClassMapping
    {
        string Table { get; }
        Type ClassType { get; }
        IList<IPropertyMapping> Properties { get; }
    }
}
