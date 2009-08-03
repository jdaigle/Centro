using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenEntity.Tests.Mock.Northwind
{
    public class OrderInfo : OpenEntity.Model.IDomainObject
    {
        public virtual decimal UnitPrice { get; set; }
        public virtual int Quantity { get; set; }
        public virtual double Discount { get; set; }  
    }
}
