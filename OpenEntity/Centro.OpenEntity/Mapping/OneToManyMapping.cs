using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Centro.OpenEntity.Mapping
{
    public class OneToManyMapping : IOneToManyMapping
    {
        internal OneToManyMapping(IPropertyMapping property, Type referenceModelType)
        {
            Property = property;
            ReferenceModelType = referenceModelType;
            Property.HasMany(this);
        }

        public IPropertyMapping Property { get; private set; }
        public Type ReferenceModelType { get; private set; }
        public bool SpecifiedPrimaryKey { get; private set; }
        public string ForeignKey { get; private set; }

        public IOneToManyMapping OnPrimaryKey(string columnName)
        {
            Property.AsColumn(columnName);
            SpecifiedPrimaryKey = true;
            return this;
        }

        public IOneToManyMapping AsForeignKey(string columnName)
        {
            ForeignKey = columnName;
            return this;
        }
    }
}