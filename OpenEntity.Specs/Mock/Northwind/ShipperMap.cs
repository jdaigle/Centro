using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenEntity.Mapping;

namespace OpenEntity.Specs.Mock.Northwind
{
    public class ShipperMap : ClassConfiguration<Shipper>
    {
        public ShipperMap()
        {
            this.ForTable("Shippers");
            this.Map(x => x.Name, "CompanyName");
        }
    }
}
