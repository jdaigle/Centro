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
            this.Maps(x => x.CompanyName);
            this.Maps(x => x.ContactName);
            this.Maps(x => x.Phone).AsCustomType(typeof(SimplePhoneNumberConverter));
        }
    }
}
