using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenEntity.Mapping;

namespace OpenEntity.Tests.Mock.Northwind
{
    public class ShipperMap : ClassMapping<Shipper>
    {
        public ShipperMap()
        {
            this.ForTable("Shippers");
            this.Map(x => x.Name, "CompanyName");
        }
    }
}
