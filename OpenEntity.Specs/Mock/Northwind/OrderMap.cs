using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenEntity.Mapping;

namespace OpenEntity.Tests.Mock.Northwind
{
    public class OrderMap : ClassMapping<Order>
    {
        public OrderMap()
        {
            this.ForTable("Orders");
            this.Map(x => x.Date, "OrderDate");
        }
    }
}
