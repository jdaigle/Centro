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
        }
    }
}
