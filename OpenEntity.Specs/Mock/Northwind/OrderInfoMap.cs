using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenEntity.Mapping;

namespace OpenEntity.Specs.Mock.Northwind
{
    public class OrderInfoMap : ClassMapping<OrderInfo>
    {
        public OrderInfoMap()
        {
            this.ForTable("Order Details");
            this.Maps(c => c.UnitPrice).AsColumn("UnitPrice");
            this.Maps(c => c.Quantity).AsColumn("Quantity");
            this.Maps(c => c.Discount).AsColumn("Discount");
        }
    }
}
