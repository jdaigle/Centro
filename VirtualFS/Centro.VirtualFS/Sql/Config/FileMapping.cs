using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Centro.OpenEntity.Mapping;

namespace Centro.VirtualFS.Sql.Config
{
    public class FileMapping : ClassMapping<SqlFile>
    {
        public FileMapping()
        {
            ForTable("files");
            Maps(x => x.Id);
            Maps(x => x.Name);
            Maps(x => x.IsDeleted);
            Maps(x => x.CreationTime);
            References(x => x.ParentDirectory).AsColumn("directory_id");
        }
    }
}
