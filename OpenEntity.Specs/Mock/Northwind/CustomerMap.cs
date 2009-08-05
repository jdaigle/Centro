using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenEntity.Mapping;

namespace OpenEntity.Tests.Mock.Northwind
{
    public class CustomerMap : ClassConfiguration<Customer>
    {
        public CustomerMap()
        {
            this.ForTable("Customers");
            this.Map(x => x.Name, "CompanyName");
        }
    }
}
