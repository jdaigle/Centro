using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenEntity.Mapping;

namespace OpenEntity.Tests.Mock.Northwind
{
    public class ProductMap : ClassMapping<Product>
    {
        public ProductMap()
        {
            this.ForTable("Products");
            this.Map(x => x.Id, "ProductId");
            this.Map(x => x.Name, "ProductName");
            this.Map(x => x.Discontinued);
        }
    }
}
