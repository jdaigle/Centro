using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Centro.VirtualFS
{
    public class FileSystemSecurityException : FileSystemException
    {
        public FileSystemSecurityException()
        {
        }
        public FileSystemSecurityException(String message)
            : base(message)
        {
        }
        public FileSystemSecurityException(String message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}