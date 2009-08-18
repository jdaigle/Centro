using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenEntity.Mapping
{
    internal class ReferenceMapping : IReferenceMapping
    {
        internal ReferenceMapping(IPropertyMapping property, Type referenceModelType)
        {
            Property = property;
            ReferenceModelType = referenceModelType;
            Property.References(this);
        }

        public IPropertyMapping Property { get; private set; }
        public Type ReferenceModelType { get; private set; }
        public bool HasSpecifiedForeignKey { get { return !string.IsNullOrEmpty(ForeignKey); } }
        public string ForeignKey { get; private set; }

        public IReferenceMapping WithColumn(string columnName)
        {
            Property.AsColumn(columnName);
            return this;
        }

        public IReferenceMapping WithForeignKey(string columnName)
        {
            ForeignKey = columnName;
            return this;
        }
    }
}
