using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Centro.OpenEntity.Mapping;

namespace Centro.OpenEntity.Specs.Mock.Northwind
{
    public class EmployeeMap : ClassMapping<Employee>
    {
        public EmployeeMap()
        {
            this.ForTable("Employees");
            this.Maps(x => x.FirstName);
            this.Maps(x => x.LastName);
            this.Maps(x => x.Title);
        }
    }
}
