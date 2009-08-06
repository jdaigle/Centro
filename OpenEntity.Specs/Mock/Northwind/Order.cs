using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenEntity.Specs.Mock.Northwind
{
    public class Order : OpenEntity.Model.IDomainObject
    {
        public virtual DateTime Date { get; set; }
    }
}
