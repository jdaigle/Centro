using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NorthwindTests.Model;
using FluentNHibernate.Mapping;

namespace NorthwindTests.Mapping
{
    public class ProductMap : ClassMap<Product>
    {
        public ProductMap()
        {
            Table("Products");
            Id(x => x.Id)
                .Column("ProductID")
                .GeneratedBy.Identity()
                .Not.Nullable();
            References(x => x.Category)
                .Column("CategoryID")
                .ForeignKey("CategoryID")
                .Cascade.None();
            Map(x => x.Name)
                .Column("ProductName");
            Map(x => x.QuantityPerUnit);
            Map(x => x.UnitPrice);
            Map(x => x.UnitsInStock);
            Map(x => x.UnitsOnOrder);
            Map(x => x.ReorderLevel);
            Map(x => x.Discontinued);
        }
    }
}
