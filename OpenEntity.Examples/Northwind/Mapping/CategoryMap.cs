using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenEntity.Examples.Northwind.Entities;
using OpenEntity.Mapping;

namespace OpenEntity.Examples.Northwind.Mapping
{
    public class CategoryMap : ClassConfiguration<Category>
    {
        public CategoryMap()
        {
            this.ForTable("Categories");
            this.Map(x => x.Id, "CategoryID");
        }
    }
}
