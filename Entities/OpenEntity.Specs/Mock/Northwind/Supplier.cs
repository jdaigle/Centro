using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Centro.OpenEntity.Specs.Mock.Northwind
{
    public class Supplier : Centro.OpenEntity.Model.IDomainObject
    {
        public virtual int Id { get; private set; }
        public virtual string CompanyName { get; set; }
        public virtual string ContactName { get; set; }
        public virtual SimplePhoneNumber Phone { get; set; }
    }
}
