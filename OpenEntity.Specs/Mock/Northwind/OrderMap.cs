using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenEntity.Mapping;

namespace OpenEntity.Specs.Mock.Northwind
{
    public class OrderMap : ClassConfiguration<Order>
    {
        public OrderMap()
        {
            this.ForTable("Orders");
            this.Maps(x => x.Date).AsColumn("OrderDate");
        }
    }
}
