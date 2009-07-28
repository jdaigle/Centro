using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenEntity.Mapping;

namespace OpenEntity.Tests.Mock.Northwind
{
    public class SupplierMap : ClassMapping<Supplier>
    {
        public SupplierMap()
        {
            this.ForTable("Suppliers");
            this.Map(x => x.Name, "CompanyName");
        }
    }
}
