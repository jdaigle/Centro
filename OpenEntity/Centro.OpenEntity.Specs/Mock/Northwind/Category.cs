using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Centro.OpenEntity.Model;

namespace Centro.OpenEntity.Specs.Mock.Northwind
{
    public class Category : IDomainObject
    {
        public Category()
        {
            Products = new List<Product>();
        }

        public virtual int Id { get { return default(int); } }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual byte[] Picture { get; set; }
        public virtual IList<Product> Products { get; private set; }
    }
}
