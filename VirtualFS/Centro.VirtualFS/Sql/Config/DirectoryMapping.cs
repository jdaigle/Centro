using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Centro.OpenEntity.Mapping;

namespace Centro.VirtualFS.Sql.Config
{
    public class DirectoryMapping: ClassMapping<SqlDirectory>
    {
        public DirectoryMapping()
        {
            ForTable("directories");
            Maps(x => x.Id);
            Maps(x => x.Name);
            Maps(x => x.IsRoot);
            Maps(x => x.IsDeleted);
            References(x => x.ParentDirectory).WithColumn("parent_id");
        }
    }
}
