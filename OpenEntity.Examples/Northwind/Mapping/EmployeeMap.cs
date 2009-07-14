using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenEntity.Examples.Northwind.Entities;
using OpenEntity.Mapping;

namespace OpenEntity.Examples.Northwind.Mapping
{
    public class EmployeeMap : ClassMapping<Employee>
    {
        public EmployeeMap()
        {
            this.ForTable("Employees");
            this.Map(x => x.FirstName, "FirstName");
            this.Map(x => x.LastName, "LastName");
            this.Map(x => x.Title, "Title");
        }
    }
}
