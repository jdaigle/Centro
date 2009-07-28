using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenEntity.Tests.Mock.Northwind;

namespace OpenEntity.Tests
{
    public static class TestEnvironment
    {
        public const string ConnectionString = "data source=localhost;Integrated Security=True;Initial Catalog=Northwind";
        public const string ProviderName = SqlDbClientTypeName.MSSQL;

        public static readonly Type[] EntityTypes = new Type[] { typeof(Category), 
                                                                 typeof(Customer), 
                                                                 typeof(Employee), 
                                                                 typeof(Order), 
                                                                 typeof(Product), 
                                                                 typeof(Shipper), 
                                                                 typeof(Supplier) };

    }
}
