using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenEntity.Mapping;

namespace OpenEntity.Specs.Mock.Northwind
{
    public class OrderInfoMap : ClassConfiguration<OrderInfo>
    {
        public OrderInfoMap()
        {
            this.ForTable("Order Details");
            this.Map(c => c.UnitPrice, "UnitPrice");
            this.Map(c => c.Quantity, "Quantity");
            this.Map(c => c.Discount, "Discount");
        }
    }
}
