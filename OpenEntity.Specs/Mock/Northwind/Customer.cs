using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenEntity.Specs.Mock.Northwind
{
    public class Customer : OpenEntity.Model.IDomainObject
    {
        public virtual string Name { get; set; }
    }
}
