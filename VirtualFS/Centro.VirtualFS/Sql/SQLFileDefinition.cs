using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cridion.Framework.FileSystem.Data;

namespace Cridion.Framework.FileSystem
{
    public class SQLFileDefinition:FileDefinition, IFileVersion
    {
        public long FileID { get; private set; }

        public SQLFileDefinition(Cridion.Framework.FileSystem.Data.File File, String FullPath, SQLFileSystemProvider FileSystemProvider)
            : base(File.Filename, FullPath, FileSystemProvider)
        {
            this.FileID = File.Fileid;
        }

        public byte[] GetFileBytes()
        {
            return ((SqlFileVersionDefinition)GetLatestVersion()).GetFileBytes();
        }

        public FileExplorerDBContext FileExplorerDBContext { get { return (FileSystemProvider as SQLFileSystemProvider).FileExplorerDBContext; } }

        #region IFileVersion Members

        public FileVersionDefinition GetLatestVersion()
        {
            FileExplorerDBContext db = FileExplorerDBContext;

            long VersionID = (from f in db.FileVersions
                              where f.Fileid == FileID && f.Latestversion
                              select f.Fileversionid).First();

            return new SqlFileVersionDefinition(VersionID, this);
        }

        public FileVersionDefinition GetVersion(object VersionID)
        {
            return GetVersion((int)VersionID);
        }

        public SqlFileVersionDefinition GetVersion(long VersionID)
        {
            return new SqlFileVersionDefinition(VersionID, this);
        }

        public FileVersionDefinition[] GetVersions()
        {
            FileExplorerDBContext db = FileExplorerDBContext;

            long[] versionids = (from f in db.FileVersions
                                 where f.Fileid == FileID
                                 select f.Fileversionid).ToArray();

            SqlFileVersionDefinition[] fileversions = new SqlFileVersionDefinition[versionids.Length];
            for (int i = 0; i < fileversions.Length; i++)
                fileversions[i] = new SqlFileVersionDefinition(versionids[i], this);
            return fileversions;
        }

        #endregion
    }
}
