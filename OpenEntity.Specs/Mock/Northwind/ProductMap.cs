using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenEntity.Mapping;

namespace OpenEntity.Specs.Mock.Northwind
{
    public class ProductMap : ClassConfiguration<Product>
    {
        public ProductMap()
        {
            this.ForTable("Products");

            this.Maps(x => x.Id).AsColumn("ProductId");
            this.Maps(x => x.Name).AsColumn("ProductName");
            this.Maps(x => x.Discontinued);
            this.Maps(x => x.CategoryId);
        }
    }
}
