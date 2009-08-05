using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenEntity.Mapping;

namespace OpenEntity.Tests.Mock.Northwind
{
    public class EmployeeMap : ClassConfiguration<Employee>
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
