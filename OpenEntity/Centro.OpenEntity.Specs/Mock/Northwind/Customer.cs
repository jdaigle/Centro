using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Centro.OpenEntity.Specs.Mock.Northwind
{
    public class Customer : Centro.OpenEntity.Model.IDomainObject
    {
        public virtual string Name { get; set; }
        public virtual DateTime? SomeNullableDate { get; set; }
    }
}
