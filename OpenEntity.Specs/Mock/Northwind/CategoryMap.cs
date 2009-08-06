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
            this.Map(x => x.Name, "CategoryName");
            this.Map(x => x.Description, "Description");
            this.Map(x => x.Picture, "Picture");
        }
    }
}
