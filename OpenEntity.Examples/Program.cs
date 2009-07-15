using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenEntity.CodeDom;
using OpenEntity.DataProviders;
using OpenEntity.Repository;
using OpenEntity.Examples.Northwind.Entities;
using OpenEntity.Examples.Northwind.Mapping;
using OpenEntity.Mapping;
using OpenEntity.Entities;

namespace OpenEntity.Examples
{
    public class Foo
    {
        public virtual int IntegerProp { get; set; }
        public virtual double DoubleProp { get; set; }
        public virtual string StringProp { get; set; }
        public virtual object ObjectProp { get; set; }
        public virtual DateTime DateTimeProp { get; set; }
        public virtual Bar BarProp { get; set; }
    }

    public class Bar
    {
        //Something in the test branch
    }

    class Program
    {
        static void Main(string[] args)
        {
            MappingConfig.AddAssembly(typeof(EmployeeMap).Assembly);
            var dataProvider = DataProviderFactory.CreateNewProvider("Northwind");
            var repository = new BaseRepository<Employee>(dataProvider);
            var employees = repository.FetchAll(null);
            //var gen = new ProxyGenerator(typeof(Foo));
            //var foo = (Foo)gen.Build();
            //foo.StringProp = "Hello World";
            //foo.StringProp.ToString();
        }
    }
}
