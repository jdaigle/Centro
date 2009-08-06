using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenEntity.Mapping;

namespace OpenEntity.Specs.Mock.Northwind
{
    public class SupplierMap : ClassConfiguration<Supplier>
    {
        public SupplierMap()
        {
            this.ForTable("Suppliers");
            this.Map(x => x.CompanyName);
            this.Map(x => x.ContactName);
            this.Map(x => x.Phone).CustomType(typeof(SimplePhoneNumberConverter));
        }
    }
}
