using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cridion.Framework.FileSystem.Data;
using Cridion.Framework.Configuration;

namespace Cridion.Framework.FileSystem
{
    public class SQLFileSystemProvider : FileSystemProvider
    {

        protected FileExplorerDBContext db;

        public FileExplorerDBContext FileExplorerDBContext { get { return db; } }

        public SQLFileSystemProvider(String HeadPath, String CurrentPath)
            : base(HeadPath, CurrentPath)
        {
            InitDB();
        }

        public SQLFileSystemProvider(String Path)
            : this(Path, Path)
        {

        }

        /// <summary>
        /// Initializes the database provider
        /// </summary>
        protected void InitDB()
        {
            if (db == null)
                db = new FileExplorerDBContext(ConnectionStrings.GetConnectionString("FileExplorer"), System.Data.Linq.Mapping.XmlMappingSource.FromUrl(Cridion.Framework.Web.Utility.GetPhysicalAppPath() + LinqMappingConfiguration.Default["FileExplorer"]));
        }


        /// <summary>
        /// Parses the SQL based path and returns a String[] of the directory names
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        protected String[] ParsePath(String Path)
        {
            if (!Path.StartsWith("SQL:\\\\", StringComparison.CurrentCultureIgnoreCase))
                throw new Exception("Path is not a Valid SQL File System Path: " + Path);
            Path = Path.Substring(6);
            return Path.Trim().Split(new String[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// The SQL ID of the current Directory
        /// </summary>
        protected long CurrentDirectoyID
        {
            get { return ((SQLDirectoryDefinition)CurrentDirectory).DirectoryID; }
        }

        /*
         * This method takes is a FullPath in the form of SQL:\\Root\Directory\SubDirectory
         * It will parse this, in recursively obtain directory IDs, drilling down to the "current" directory.
         * It will then pull out all data as a Directory object, and pass this to the SQLDirectoyDef constructor.
         * 
         */
        public override DirectoryDefinition GetDirectory(string FullPath)
        {
            InitDB();
            String[] _ParsedPath = ParsePath(FullPath);
            if (_ParsedPath == null || _ParsedPath.Length == 0)
                throw new Exception("Path is not valid: " + FullPath);

            long directoryid = (from rd in db.Directories
                                where rd.Directoryname.Equals(_ParsedPath[0]) && !rd.Directorydeleted && rd.Rootdirectory
                                select rd.Directoryid).Take(1).First();

            for (int i = 1; i < _ParsedPath.Length; i++)
            {
                directoryid = (from d in db.Directories
                               where d.Directoryname.Equals(_ParsedPath[i]) && d.Parentdirectoryid == directoryid && !d.Directorydeleted
                               select d.Directoryid).Take(1).First();
            }

            var directories = (from d in db.Directories
                               where d.Directoryid == directoryid && !d.Directorydeleted
                               select d).Take(1);

            SQLDirectoryDefinition SQLDirectoryDef = new SQLDirectoryDefinition(directories.First(), FullPath, this);
            if (!SQLDirectoryDef.HasReadAccess(GetWebContextUserName()))
                throw new FileSystemSecurityException("User does not have read access on this directory");

            return SQLDirectoryDef;
        }


        /* returns all sub directories */
        public override DirectoryDefinition[] GetSubDirectories(DirectoryDefinition ParentDirectory)
        {
            if (!(ParentDirectory is SQLDirectoryDefinition))
                ParentDirectory = GetDirectory(ParentDirectory.FullPath);
            long directoryid = ((SQLDirectoryDefinition)ParentDirectory).DirectoryID;

            Directory[] subdirectories = (from d in db.Directories
                                          where d.Parentdirectoryid == directoryid && !d.Directorydeleted
                                          select d).ToArray();

            SQLDirectoryDefinition[] subdirdefs = new SQLDirectoryDefinition[subdirectories.Length];
            for (int i = 0; i < subdirdefs.Length; i++)
                subdirdefs[i] = new SQLDirectoryDefinition(subdirectories[i], ParentDirectory.FullPath + "\\" + subdirectories[i].Directoryname, this);

            return subdirdefs;
        }


        /* returns a specific file */
        public override FileDefinition GetFile(string FullFilePath)
        {
            String[] parsedpath = ParsePath(FullFilePath);
            String filename = parsedpath[parsedpath.Length - 1];

            File file = (from f in db.Files
                         where f.Filename.Equals(filename) && f.Directoryid == ((SQLDirectoryDefinition)CurrentDirectory).DirectoryID && !f.Filedeleted
                         select f).Take(1).First();

            return new SQLFileDefinition(file, FullFilePath, this);
        }


        /* returns all files in a given directory */
        public override FileDefinition[] GetFiles(DirectoryDefinition ParentDirectory)
        {
            if (!(ParentDirectory is SQLDirectoryDefinition))
                ParentDirectory = GetDirectory(ParentDirectory.FullPath);
            long directoryid = ((SQLDirectoryDefinition)ParentDirectory).DirectoryID;

            File[] files = (from f in db.Files
                            where f.Directoryid == directoryid && !f.Filedeleted
                            select f).ToArray();

            SQLFileDefinition[] filedefs = new SQLFileDefinition[files.Length];
            for (int i = 0; i < filedefs.Length; i++)
            {
                filedefs[i] = new SQLFileDefinition(files[i], ParentDirectory.FullPath + "\\" + files[i].Filename, this);
            }

            return filedefs;
        }

        /* checks to see if a directory already exists in the current context */
        public Boolean CheckDirectoryExists(string DirectoryName)
        {
            long directoryID = ((SQLDirectoryDefinition)CurrentDirectory).DirectoryID;

            return ((from d in db.Directories
                     where d.Directoryname.Equals(DirectoryName) && d.Parentdirectoryid == directoryID && !d.Directorydeleted
                     select d).Count() > 0);
        }

        /* creates a new directory in the current context */
        public override void CreateNewDirectory(string DirectoryName)
        {
            if (!((SQLDirectoryDefinition)CurrentDirectory).HasChangeAccess(GetWebContextUserName()))
                throw new FileSystemSecurityException("User does not have change access on this directory");

            if (CheckDirectoryExists(DirectoryName))
                return;
            long directoryID = ((SQLDirectoryDefinition)CurrentDirectory).DirectoryID;
            Directory newdirectory = new Directory();
            newdirectory.Directoryname = DirectoryName;
            newdirectory.Parentdirectoryid = directoryID;
            db.Directories.InsertOnSubmit(newdirectory);
            db.SubmitChanges();

        }

        /* deletes a directory from the current context */
        public override void DeleteDirectory(string DirectoryName)
        {
            if (!((SQLDirectoryDefinition)CurrentDirectory).HasChangeAccess(GetWebContextUserName()))
                throw new FileSystemSecurityException("User does not have change access on this directory");

            long parentdirectoryID = ((SQLDirectoryDefinition)CurrentDirectory).DirectoryID;
            Directory directorytodelete = (from d in db.Directories
                                           where d.Parentdirectoryid == parentdirectoryID && d.Directoryname.Equals(DirectoryName) && !d.Directorydeleted
                                           select d).Take(1).First();
            directorytodelete.Directorydeleted = true;
            db.SubmitChanges();
        }

        /* deletes a file from the current context */
        public override void DeleteFile(string FileName)
        {
            if (!((SQLDirectoryDefinition)CurrentDirectory).HasChangeAccess(GetWebContextUserName()))
                throw new FileSystemSecurityException("User does not have change access on this directory");


            long parentdirectoryID = ((SQLDirectoryDefinition)CurrentDirectory).DirectoryID;
            File filetodelete = (from f in db.Files
                                 where f.Directoryid == parentdirectoryID && f.Filename.Equals(FileName) && !f.Filedeleted
                                 select f).Take(1).First();
            filetodelete.Filedeleted = true;
            db.SubmitChanges();
        }


        /// <summary>
        /// The file with the given name exists
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        private Boolean FileExists(String FileName)
        {
            long parentdirectoryID = ((SQLDirectoryDefinition)CurrentDirectory).DirectoryID;
            int filescount = (from f in db.Files
                         where f.Filename == FileName && f.Filedeleted == false && f.Directoryid == parentdirectoryID
                         select f).Count();
            return (filescount > 0);
        }

        public void CreateNewFileVersion(System.Web.UI.WebControls.FileUpload UploadedFileControl, long FileID)
        {
            if (!((SQLDirectoryDefinition)CurrentDirectory).HasChangeAccess(GetWebContextUserName()))
                throw new FileSystemSecurityException("User does not have change access on this directory");

            FileVersion oldVersion = (from f in db.FileVersions
                                      where f.Fileid == FileID && f.Latestversion
                                      select f).First();
            oldVersion.Latestversion = false;

            long parentdirectoryID = ((SQLDirectoryDefinition)CurrentDirectory).DirectoryID;

            FileBinary newFileBinary = new FileBinary();
            newFileBinary.Filebinarydata = new System.Data.Linq.Binary(UploadedFileControl.FileBytes);
            newFileBinary.Filesize = (long)UploadedFileControl.PostedFile.ContentLength;

            FileVersion newFileVersion = new FileVersion();
            newFileVersion.FileBinary = newFileBinary;
            newFileVersion.Versionnumber = oldVersion.Versionnumber + 1;
            newFileVersion.Filemodifieddate = DateTime.Now;
            newFileVersion.Latestversion = true;
            newFileVersion.Fileid = FileID;

            db.FileVersions.InsertOnSubmit(newFileVersion);

            db.SubmitChanges();
        }


        /// <summary>
        /// Implemeneted: Creates a new file given a filled FileUpload web control
        /// </summary>
        /// <param name="UploadedFileControl"></param>
        public override void CreateNewFile(System.Web.UI.WebControls.FileUpload UploadedFileControl)
        {
            CreateNewFile(UploadedFileControl.FileName, (long)UploadedFileControl.PostedFile.ContentLength, UploadedFileControl.FileBytes);
        }

        /// <summary>
        /// Implemeneted: Creates a new file given the name and file information
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileSize"></param>
        /// <param name="fileData"></param>
        public override void CreateNewFile(string fileName, long fileSize, byte[] fileData)
        {
            if (!((SQLDirectoryDefinition)CurrentDirectory).HasChangeAccess(GetWebContextUserName()))
                throw new FileSystemSecurityException("User does not have change access on this directory");

            if (FileExists(fileName))
                throw new FileSystemSecurityException("File: " + fileName + " already exists");

            long parentdirectoryID = ((SQLDirectoryDefinition)CurrentDirectory).DirectoryID;

            FileBinary newFileBinary = new FileBinary();
            newFileBinary.Filebinarydata = new System.Data.Linq.Binary(fileData);
            newFileBinary.Filesize = fileSize;

            File newFile = new File
            {
                Filename = fileName,
                Filecreationdate = DateTime.Now,
                Directoryid = parentdirectoryID
            };

            FileVersion newFileVersion = new FileVersion();
            newFileVersion.FileBinary = newFileBinary;
            newFileVersion.Versionnumber = 1;
            newFileVersion.Filemodifieddate = DateTime.Now;
            newFileVersion.Latestversion = true;
            newFileVersion.File = newFile;

            db.FileVersions.InsertOnSubmit(newFileVersion);

            db.SubmitChanges();
        }

        /// <summary>
        /// Implemeneted: Renames a file to a given name
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="newFileName"></param>
        public override void RenameFile(string fileName, string newFileName)
        {
            if (!((SQLDirectoryDefinition)CurrentDirectory).HasChangeAccess(GetWebContextUserName()))
                throw new FileSystemSecurityException("User does not have change access on this directory");

            if (!FileExists(fileName))
                throw new FileSystemSecurityException("File: " + fileName + " does not exist");

            long parentdirectoryID = ((SQLDirectoryDefinition)CurrentDirectory).DirectoryID;

            File file = (from f in db.Files
                         where f.Filename == fileName && f.Directoryid == parentdirectoryID && !f.Filedeleted
                         select f).Take(1).First();
            file.Filename = newFileName;

            db.SubmitChanges();
        }

        /// <summary>
        /// Implemeneted: Returns a native file property object given the file and property type
        /// </summary>
        /// <param name="File"></param>
        /// <param name="Property"></param>
        /// <returns></returns>
        public override object GetNativeFileProperty(FileDefinition File, FileProperties Property)
        {
            if (Property == FileProperties.Size)
            {
                return (from f in db.FileVersions
                        where f.Latestversion == true && f.File.Fileid == ((SQLFileDefinition)File).FileID
                        select f.FileBinary.Filesize).Take(1).First();
            }
            if (Property == FileProperties.LastWriteTime)
            {
                return (from f in db.FileVersions
                        where f.Latestversion == true && f.File.Fileid == ((SQLFileDefinition)File).FileID
                        select f.Filemodifieddate).Take(1).First();
            }
            if (Property == FileProperties.CreateTime)
            {
                return (from f in db.Files
                        where f.Fileid == ((SQLFileDefinition)File).FileID
                        select f.Filecreationdate).Take(1).First();
            }
            return null;
        }


        /// <summary>
        /// This will Expunge deleted files/folders from the current directory context
        /// </summary>
        public void Expunge()
        {
            Expunge(CurrentDirectoyID);
        }

        /// <summary>
        /// This is a recursive helper function to expunge deleted files/folders from the given directory context
        /// </summary>
        /// <param name="directoryID">Directory ID to delete from</param>
        /// <param name="recursive">Expunge sub directories also</param>
        private void Expunge(long directoryID)
        {
            //first thing we need to do, if this directory is marked to be deleted, mark ALL of it's children as deleted
            var directory = db.Directories.Where(d => d.Directoryid == directoryID).Select(d => d).First();
            if (directory.Directorydeleted)
            {
                foreach (var file in directory.Files)
                    file.Filedeleted = true;
                foreach (var subdir in db.Directories.Where(d => d.Parentdirectoryid == directoryID).Select(d => d))
                    subdir.Directorydeleted = true;
                db.SubmitChanges();
            }


            //first expunge sub directories
            var subdirectoryids = from d in db.Directories
                                  where d.Parentdirectoryid == directoryID
                                  select d.Directoryid;
            foreach (long subdirectoryID in subdirectoryids)
                Expunge(subdirectoryID);
            

            //now that everything under those directories are gone, we can delete
            //the directories marked as deleted
            //BUT FIRST we need to delete ACLMembers and AccessControlLists for the directory we're deleting
            var aclMembers = from a in db.ACLMembers
                             where a.AccessControlList.Directory.Parentdirectoryid == directoryID &&
                                   a.AccessControlList.Directory.Directorydeleted
                             select a;
            db.ACLMembers.DeleteAllOnSubmit(aclMembers);
            db.SubmitChanges();
            var aclLists = from a in db.AccessControlLists
                           where a.Directory.Parentdirectoryid == directoryID &&
                                 a.Directory.Directorydeleted
                           select a;
            db.AccessControlLists.DeleteAllOnSubmit(aclLists);
            db.SubmitChanges();
            var deletedsubdirecories = from d in db.Directories
                                       where d.Parentdirectoryid == directoryID && d.Directorydeleted
                                       select d;
            db.Directories.DeleteAllOnSubmit(deletedsubdirecories);
            db.SubmitChanges();           

            //the easy thing to do is delete marked files
            //delete versions first, then the file, then orphaned binaries
            var fileversions = from f in db.FileVersions
                               where f.File.Filedeleted
                               select f;
            db.FileVersions.DeleteAllOnSubmit(fileversions);
            db.SubmitChanges();            
            var files = from f in db.Files
                        where f.Filedeleted
                        select f;
            db.Files.DeleteAllOnSubmit(files);
            db.SubmitChanges();
            var filebinaries = from f in db.FileBinaries
                               where f.FileVersions.Count == 0
                               select f;
            db.FileBinaries.DeleteAllOnSubmit(filebinaries);
            db.SubmitChanges();
        }        

        /// <summary>
        /// Implemeneted: This FileSystemProvider supports Privileges
        /// </summary>
        public override bool SupportsPrivileges
        {
            get { return true; }
        }


        /// <summary>
        /// Implemeneted: This FileSystemProvider supports versioning
        /// </summary>
        public override bool SupportVersioning
        {
            get { return true; }
        }

        
    }
}
