using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;

namespace OpenEntity.Extensions
{
    public static class Attributes
    {
        /// <summary>
        /// Gets the ProgId using reflection on the provided type.
        /// </summary>
        public static string GetProgId(this MemberInfo progIdType)
        {
            object[] attributes = progIdType.GetCustomAttributes(typeof(ProgIdAttribute), false);
            if (attributes.Length == 0)
                return string.Empty;
            if (attributes[0] is ProgIdAttribute)
                return ((ProgIdAttribute)attributes[0]).Value;
            else
                return string.Empty;
        }
    }
}
