using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Centro.VirtualFS.NTFS
{
    public class NTFSFileSystemProvider : IFileSystemProvider
    {
        private string rootDirectoryPath;

        public NTFSFileSystemProvider(String rootDirectoryPath)
        {
            if (!Directory.Exists(rootDirectoryPath))
                throw new ArgumentException("Directory does not exist.", rootDirectoryPath);
            this.rootDirectoryPath = rootDirectoryPath;
        }

        //public override DirectoryDefinition GetDirectory(String FullPath)
        //{
        //    try
        //    {
        //        DirectoryInfo directory = new DirectoryInfo(FullPath);
        //        return new DirectoryDefinition(directory.Name, directory.FullName, this);
        //    }
        //    catch (System.UnauthorizedAccessException e)
        //    {
        //        throw new FileSystemSecurityException("File System Access Error", e);
        //    }
        //    catch (System.IO.IOException ioe)
        //    {
        //        throw new FileSystemSecurityException("File System Access Error", ioe);
        //    }
        //    throw new FileSystemSecurityException("Failed to Get Directory: " + FullPath);
        //}

        //public override DirectoryDefinition[] GetSubDirectories(DirectoryDefinition ParentDirectory)
        //{
        //    try
        //    {
        //        DirectoryInfo directory = new DirectoryInfo(ParentDirectory.FullPath);
        //        DirectoryInfo[] subdirs = directory.GetDirectories();
        //        List<DirectoryDefinition> list = new List<DirectoryDefinition>();
        //        for (int i = 0; i < subdirs.Length; i++)
        //        {
        //            if ((subdirs[i].Attributes & FileAttributes.System) == 0 && (subdirs[i].Attributes & FileAttributes.Hidden) == 0)
        //                list.Add(new DirectoryDefinition(subdirs[i].Name, subdirs[i].FullName, this));
        //        }
        //        return list.ToArray();
        //    }
        //    catch (System.UnauthorizedAccessException e)
        //    {
        //        throw new FileSystemSecurityException("File System Access Error", e);
        //    }
        //    catch (System.IO.IOException ioe)
        //    {
        //        throw new FileSystemSecurityException("File System Access Error", ioe);
        //    }
        //    throw new FileSystemSecurityException("Failed to Get Sub Directories: " + ParentDirectory.FullPath);
        //}

        //public override FileDefinition GetFile(String FullFilePath)
        //{
        //    try
        //    {
        //        FileInfo file = new FileInfo(FullFilePath);
        //        return new FileDefinition(file.Name, file.FullName, this);
        //    }
        //    catch (System.UnauthorizedAccessException e)
        //    {
        //        throw new FileSystemSecurityException("File System Access Error", e);
        //    }
        //    catch (System.IO.IOException ioe)
        //    {
        //        throw new FileSystemSecurityException("File System Access Error", ioe);
        //    }
        //    throw new FileSystemSecurityException("Failed to Get File: " + FullFilePath);
        //}

        //public override FileDefinition[] GetFiles(DirectoryDefinition ParentDirectory)
        //{
        //    try
        //    {
        //        FileInfo[] files = new DirectoryInfo(ParentDirectory.FullPath).GetFiles();
        //        List<FileDefinition> filedefs = new List<FileDefinition>();
        //        for (int i = 0; i < files.Length; i++)
        //            if ((files[i].Attributes & FileAttributes.System) == 0 && (files[i].Attributes & FileAttributes.Hidden) == 0)
        //                filedefs.Add(new FileDefinition(files[i].Name, files[i].FullName, this));
        //        return filedefs.ToArray();
        //    }
        //    catch (System.UnauthorizedAccessException e)
        //    {
        //        throw new FileSystemSecurityException("File System Access Error", e);
        //    }
        //    catch (System.IO.IOException ioe)
        //    {
        //        throw new FileSystemSecurityException("File System Access Error", ioe);
        //    }
        //    throw new FileSystemSecurityException("Failed to Get Sub Files: " + ParentDirectory.FullPath);
        //}

        //public override void CreateNewDirectory(String DirectoryName)
        //{
        //    try
        //    {
        //        DirectoryInfo directory = new DirectoryInfo(TailPathDirectory.FullPath + "\\" + DirectoryName);
        //        directory.Create();
        //    }
        //    catch (System.UnauthorizedAccessException e)
        //    {
        //        throw new FileSystemSecurityException("File System Access Error", e);
        //    }
        //    catch (System.IO.IOException ioe)
        //    {
        //        throw new FileSystemSecurityException("File System Access Error", ioe);
        //    }
        //}

        //public override void DeleteDirectory(String DirectoryName)
        //{
        //    try
        //    {
        //        DirectoryInfo directory = new DirectoryInfo(TailPathDirectory.FullPath + "\\" + DirectoryName);
        //        directory.Delete(true);
        //    }
        //    catch (System.UnauthorizedAccessException e)
        //    {
        //        throw new FileSystemSecurityException("File System Access Error", e);
        //    }
        //    catch (System.IO.IOException ioe)
        //    {
        //        throw new FileSystemSecurityException("File System Access Error", ioe);
        //    }
        //}

        //public override void DeleteFile(String FileName)
        //{
        //    try
        //    {
        //        FileInfo file = new FileInfo(TailPathDirectory.FullPath + "\\" + FileName);
        //        file.Delete();
        //    }
        //    catch (System.UnauthorizedAccessException e)
        //    {
        //        throw new FileSystemSecurityException("File System Access Error", e);
        //    }
        //    catch (System.IO.IOException ioe)
        //    {
        //        throw new FileSystemSecurityException("File System Access Error", ioe);
        //    }
        //}

        //private Boolean FileExists(String FileName)
        //{
        //    try
        //    {
        //        FileInfo file = new FileInfo(TailPathDirectory.FullPath + "\\" + FileName);
        //        return file.Exists;
        //    }
        //    catch (System.UnauthorizedAccessException e)
        //    {
        //        throw new FileSystemSecurityException("File System Access Error", e);
        //    }
        //    catch (System.IO.IOException ioe)
        //    {
        //        throw new FileSystemSecurityException("File System Access Error", ioe);
        //    }
        //}


        //public override void CreateNewFile(string fileName, long fileSize, byte[] fileData)
        //{
        //    try
        //    {
        //        if (FileExists(fileName))
        //            throw new FileSystemSecurityException("File: " + fileName + " already exists");
        //        System.IO.File.WriteAllBytes(TailPathDirectory.FullPath + "\\" + fileName, fileData);
        //    }
        //    catch (System.UnauthorizedAccessException e)
        //    {
        //        throw new FileSystemSecurityException("File System Access Error", e);
        //    }
        //    catch (System.IO.IOException ioe)
        //    {
        //        throw new FileSystemSecurityException("File System Access Error", ioe);
        //    }
        //}

        //public override void RenameFile(string fileName, string newFileName)
        //{
        //    try
        //    {
        //        if (!FileExists(fileName))
        //            throw new FileSystemSecurityException("File: " + fileName + " does not exist");
        //        File.Move(TailPathDirectory.FullPath + "\\" + fileName, TailPathDirectory.FullPath + "\\" + newFileName);
        //    }
        //    catch (System.UnauthorizedAccessException e)
        //    {
        //        throw new FileSystemSecurityException("File System Access Error", e);
        //    }
        //    catch (System.IO.IOException ioe)
        //    {
        //        throw new FileSystemSecurityException("File System Access Error", ioe);
        //    }
        //}

        //public override object GetNativeFileProperty(FileDefinition File, FileProperties Property)
        //{
        //    try
        //    {
        //        if (Property == FileProperties.Size)
        //        {
        //            return new FileInfo(File.FullPath).Length;
        //        }
        //        if (Property == FileProperties.LastWriteTime)
        //        {
        //            return new FileInfo(File.FullPath).LastWriteTime;
        //        }
        //        if (Property == FileProperties.CreateTime)
        //        {
        //            return new FileInfo(File.FullPath).CreationTime;
        //        }
        //    }
        //    catch (System.UnauthorizedAccessException e)
        //    {
        //        throw new FileSystemSecurityException("File System Access Error", e);
        //    }
        //    catch (System.IO.IOException ioe)
        //    {
        //        throw new FileSystemSecurityException("File System Access Error", ioe);
        //    }
        //    throw new FileSystemSecurityException("Failed to Get File Property: " + File.FullPath);
        //}


        //public override bool SupportsPrivileges
        //{
        //    get { return false; }
        //}

        //public override bool SupportVersioning
        //{
        //    get { return false; }
        //}

        private string GetNTFSDirectoryPath(VirtualPath path)
        {
            return Path.Combine(rootDirectoryPath, "." + path.Directory);
        }

        #region IFileSystemProvider Members

        public IDirectory RootDirectory
        {
            get { return GetDirectory(VirtualPath.RootPath); }
        }

        public IDirectory GetDirectory(VirtualPath path)
        {
            if (path == null)
                throw new ArgumentNullException("path");
            string realPath = GetNTFSDirectoryPath(path);
            try
            {
                DirectoryInfo directory = new DirectoryInfo(realPath);
                return new NTFSDirectory(this, directory.Name, path.DirectoryPart);
            }
            catch (System.UnauthorizedAccessException e)
            {
                throw new FileSystemSecurityException("File System Access Control Error: "+e.Message, e);
            }
            catch (System.IO.IOException ioe)
            {
                throw new FileSystemException("File System Access Error: "+ioe.Message, ioe);
            }
        }

        public IFile GetFile(VirtualPath path)
        {
            
            throw new NotImplementedException();
        }

        public IList<IDirectory> GetDirectories(IDirectory directory)
        {
            throw new NotImplementedException();
        }

        public IList<IFile> GetFiles(IDirectory directory)
        {
            throw new NotImplementedException();
        }

        public IDirectory CreateDirectory(IDirectory parent, string name)
        {
            throw new NotImplementedException();
        }

        public void DeleteDirectory(IDirectory directory)
        {
            throw new NotImplementedException();
        }

        public void DeleteDirectory(VirtualPath path)
        {
            throw new NotImplementedException();
        }

        public IFile CreateFile(IDirectory directory, string name, byte[] data)
        {
            throw new NotImplementedException();
        }

        public IFile RenameFile(IFile file, string name)
        {
            throw new NotImplementedException();
        }

        public bool SupportsAccessControl
        {
            get { throw new NotImplementedException(); }
        }

        public bool SupportsFileVersioning
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}