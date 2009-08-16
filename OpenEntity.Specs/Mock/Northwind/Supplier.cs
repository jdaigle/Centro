using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenEntity.Specs.Mock.Northwind
{
    public class Supplier : OpenEntity.Model.IDomainObject
    {
        public virtual int Id { get; private set; }
        public virtual string CompanyName { get; set; }
        public virtual string ContactName { get; set; }
        public virtual SimplePhoneNumber Phone { get; set; }
    }
}
