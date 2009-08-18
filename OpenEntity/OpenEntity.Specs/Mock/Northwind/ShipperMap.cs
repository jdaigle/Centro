using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Centro.OpenEntity.Mapping;

namespace Centro.OpenEntity.Specs.Mock.Northwind
{
    public class ShipperMap : ClassMapping<Shipper>
    {
        public ShipperMap()
        {
            this.ForTable("Shippers");
            this.Maps(x => x.Name).AsColumn("CompanyName");
        }
    }
}
