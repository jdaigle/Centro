using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NorthwindTests.Model
{
    public class Category
    {
        public virtual int Id { get; private set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual byte[] Picture { get; set; }
    }
}
