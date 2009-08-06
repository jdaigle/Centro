using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenEntity.Model;

namespace OpenEntity.Specs.Mock.Northwind
{
    public class Category : IDomainObject
    {
        public virtual int Id { get { return default(int); } }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual byte[] Picture { get; set; }
    }
}
