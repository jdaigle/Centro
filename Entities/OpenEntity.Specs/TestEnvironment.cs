using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Centro.OpenEntity.Specs.Mock.Northwind;
using Centro.OpenEntity.DataProviders;

namespace Centro.OpenEntity.Specs
{
    public static class TestEnvironment
    {
        public const string SqlServerConnectionString = "data source=.;Integrated Security=True;Initial Catalog=Northwind";
        public const string SqlServerProviderName = SqlDbClientTypeName.MSSQL;

        public static readonly Type[] EntityTypes = new Type[] { typeof(Category), 
                                                                 typeof(Customer), 
                                                                 typeof(Employee), 
                                                                 typeof(Order), 
                                                                 typeof(Product), 
                                                                 typeof(Shipper), 
                                                                 typeof(Supplier) };

        public static IDataProvider GetSqlServerDataProvider()
        {
            return DataProviderFactory.CreateNewProvider(SqlServerConnectionString, SqlServerProviderName, "dbo");
        }

    }
}
