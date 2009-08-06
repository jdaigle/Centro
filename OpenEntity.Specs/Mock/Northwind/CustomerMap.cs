using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenEntity.Mapping;

namespace OpenEntity.Specs.Mock.Northwind
{
    public class CustomerMap : ClassConfiguration<Customer>
    {
        public CustomerMap()
        {
            this.ForTable("Customers");
            this.Maps(x => x.Name).AsColumn("CompanyName");
        }
    }
}
