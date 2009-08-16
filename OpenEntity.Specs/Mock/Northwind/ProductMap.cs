using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenEntity.Mapping;

namespace OpenEntity.Specs.Mock.Northwind
{
    public class ProductMap : ClassMapping<Product>
    {
        public ProductMap()
        {
            this.ForTable("Products");

            Maps(x => x.Id).AsColumn("ProductId");
            Maps(x => x.Name).AsColumn("ProductName");
            Maps(x => x.Discontinued);

            References(x => x.Category).WithColumn("CategoryId");
            References(x => x.Supplier).WithColumn("SupplierId");
        }
    }
}
