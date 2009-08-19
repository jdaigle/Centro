using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cridion.Framework.FileSystem.Data;

namespace Cridion.Framework.FileSystem
{
    public class SqlFileVersionDefinition:FileVersionDefinition
    {


        public SqlFileVersionDefinition(long VersionID, SQLFileDefinition SQLFileDef)
            : base(VersionID)
        {
            this.SQLFileDef = SQLFileDef;
        }

        public SQLFileDefinition SQLFileDef { get; private set; }

        public long SqlVersionID
        {
            get { return (long)VersionID; }
        }

        public override DateTime ModifiedDate
        {
            get 
            { 
                FileExplorerDBContext db = SQLFileDef.FileExplorerDBContext;
                return (from f in db.FileVersions
                        where f.Fileversionid == SqlVersionID
                        select f.Filemodifieddate).First();            
            }
        }

        public override bool IsLatestVersion
        {
            get
            {
                FileExplorerDBContext db = SQLFileDef.FileExplorerDBContext;
                return (from f in db.FileVersions
                        where f.Fileversionid == SqlVersionID
                        select f.Latestversion).First();
            }
        }

        public override long Size
        {
            get
            {
                FileExplorerDBContext db = SQLFileDef.FileExplorerDBContext;
                return (from f in db.FileVersions
                        where f.Fileversionid == SqlVersionID
                        select f.FileBinary.Filesize).First();
            }
        }

        public override FileVersionDefinition GetLatestVersion()
        {
            return SQLFileDef.GetLatestVersion();
        }

        public override FileVersionDefinition GetVersion(object VersionID)
        {
            return SQLFileDef.GetVersion(VersionID);
        }

        public override FileVersionDefinition CreateNewVersion(System.Web.UI.WebControls.FileUpload UploadedFileControl)
        {
            ((SQLFileSystemProvider)SQLFileDef.FileSystemProvider).CreateNewFileVersion(UploadedFileControl, SQLFileDef.FileID);
            return SQLFileDef.GetLatestVersion();
        }

        public override int VersionNumber
        {
            get
            {
                FileExplorerDBContext db = SQLFileDef.FileExplorerDBContext;
                return (from f in db.FileVersions
                        where f.Fileversionid == SqlVersionID
                        select f.Versionnumber).First();
            }
        }

        public byte[] GetFileBytes()
        {
            FileExplorerDBContext db = SQLFileDef.FileExplorerDBContext;
            return (from f in db.FileVersions
                    where f.Fileversionid == SqlVersionID
                    select f.FileBinary.Filebinarydata.ToArray()).Take(1).First();
        }
    }
}
