using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Centro.OpenEntity.Mapping;

namespace Centro.OpenEntity.Specs.Mock.Northwind
{
    public class Employee : Centro.OpenEntity.Model.IDomainObject
    {
        public virtual string LastName { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string Title { get; set; }
    }
}
