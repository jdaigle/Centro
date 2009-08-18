using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenEntity.Mapping
{
    public interface IReferenceMapping
    {
        IPropertyMapping Property { get; }
        Type ReferenceModelType { get; }
        bool HasSpecifiedForeignKey { get; }
        string ForeignKey { get; }

        IReferenceMapping WithColumn(string columnName);
        IReferenceMapping WithForeignKey(string columnName);
    }
}
