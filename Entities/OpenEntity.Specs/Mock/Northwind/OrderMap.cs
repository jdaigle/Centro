using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Centro.OpenEntity.Mapping;

namespace Centro.OpenEntity.Specs.Mock.Northwind
{
    public class OrderMap : ClassMapping<Order>
    {
        public OrderMap()
        {
            this.ForTable("Orders");
            this.Maps(x => x.Date).AsColumn("OrderDate");
        }
    }
}
