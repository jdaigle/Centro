using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenEntity.Tests.Mock.Northwind
{
    public class Shipper : OpenEntity.Model.IDomainObject
    {
        public virtual string Name { get; set; }
    }
}
