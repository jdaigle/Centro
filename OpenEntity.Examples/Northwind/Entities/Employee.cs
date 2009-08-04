using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenEntity.Mapping;
using OpenEntity.Model;

namespace OpenEntity.Examples.Northwind.Entities
{
    public class Employee : IDomainObject
    {
        public virtual string LastName { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string Title { get; set; }
    }
}
