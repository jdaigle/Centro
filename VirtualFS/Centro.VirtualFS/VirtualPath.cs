using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Centro.VirtualFS
{
    public class VirtualPath
    {
        public static readonly char DirectorySeparatorChar;
        public static readonly VirtualPath RootPath;

        static VirtualPath()
        {
            DirectorySeparatorChar = '/';
            RootPath = new VirtualPath("/");
        }

        //public static char[] GetInvalidFileNameChars() { return System.IO.Path.GetInvalidFileNameChars(); }
        //public static char[] GetInvalidPathChars() { return System.IO.Path.GetInvalidPathChars(); }

        public static VirtualPath Combine(VirtualPath directory, string file)
        {
            return new VirtualPath(directory.Directory + file);
        }

        private string path;

        public VirtualPath(string path)
        {

            if (!path.StartsWith("/"))
                throw new ArgumentException(string.Format("Path must start with root path indicator \"{0}\"", DirectorySeparatorChar), "path");
            this.path = path;
        }

        public string FullPath
        {
            get { return path; }
        }

        public VirtualPath DirectoryPart
        {
            get
            {
                return new VirtualPath(path.Substring(0, path.LastIndexOf(DirectorySeparatorChar) + 1));
            }
        }

        public string Directory
        {
            get { return DirectoryPart.FullPath; }
        }

        public bool HasFileName
        {
            get
            {
                return !path.EndsWith(DirectorySeparatorChar.ToString());
            }
        }

        public string FileName
        {
            get
            {
                if (!HasFileName)
                    return null;
                return path.Substring(path.LastIndexOf(DirectorySeparatorChar) + 1);
            }
        }

        public bool IsRoot { get { return this.Equals(VirtualPath.RootPath); } }

        public override string ToString()
        {
            return FullPath;
        }

        public override bool Equals(object obj)
        {
            var otherPath = obj as VirtualPath;
            if (otherPath == null)
                return false;
            return FullPath.Equals(otherPath.FullPath);
        }

        public override int GetHashCode()
        {
            return FullPath.GetHashCode();
        }
    }
}
