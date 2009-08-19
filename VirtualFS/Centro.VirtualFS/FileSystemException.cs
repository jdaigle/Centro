using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Centro.VirtualFS
{
    public class FileSystemException : Exception
    {
        public FileSystemException()
        {
        }
        public FileSystemException(String message)
            : base(message)
        {
        }
        public FileSystemException(String message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
