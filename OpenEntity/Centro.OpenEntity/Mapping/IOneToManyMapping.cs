using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Centro.OpenEntity.Mapping
{
    public interface IOneToManyMapping
    {
        IPropertyMapping Property { get; }
        Type ReferenceModelType { get; }
        bool SpecifiedPrimaryKey { get; }
        string ForeignKey { get; }

        IOneToManyMapping AsForeignKey(string columnName);
        IOneToManyMapping OnPrimaryKey(string columnName);
    }
}

