using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenEntity.Model;

namespace OpenEntity.Specs.Mock.Northwind
{
    public class Product : IDomainObject
    {
        public virtual int Id { get { return default(int); } }
        public virtual string Name { get; set; }
        public virtual bool Discontinued { get; set; }
        public virtual Category Category { get; set; }
    }
}
