﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Centro.OpenEntity.Mapping
{
    public interface IReferenceMapping
    {
        IPropertyMapping Property { get; }
        Type ReferenceModelType { get; }
        bool HasSpecifiedForeignKey { get; }
        string ForeignKey { get; }

        IReferenceMapping AsColumn(string columnName);
        IReferenceMapping WithForeignKey(string columnName);
    }
}
