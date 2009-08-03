using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenEntity.Tests.Mock.Northwind;
using OpenEntity.DataProviders;

namespace OpenEntity.Specs
{
    public static class TestEnvironment
    {
        public const string SqlServerConnectionString = "data source=.\\SQLEXPRESS;Integrated Security=True;Initial Catalog=Northwind";
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
