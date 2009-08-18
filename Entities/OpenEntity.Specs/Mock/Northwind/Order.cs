using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Centro.OpenEntity.Specs.Mock.Northwind
{
    public class Order : Centro.OpenEntity.Model.IDomainObject
    {
        public virtual DateTime Date { get; set; }
    }
}
