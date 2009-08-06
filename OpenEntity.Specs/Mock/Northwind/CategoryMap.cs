using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenEntity.Mapping;

namespace OpenEntity.Specs.Mock.Northwind
{
    public class CategoryMap : ClassConfiguration<Category>
    {
        public CategoryMap()
        {
            this.ForTable("Categories");
            this.Maps(x => x.Id).AsColumn("CategoryId");
            this.Maps(x => x.Name).AsColumn("CategoryName");
            this.Maps(x => x.Description).AsColumn("Description");
            this.Maps(x => x.Picture).AsColumn("Picture");
        }
    }
}
