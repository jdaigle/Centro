using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Diagnostics;

namespace Centro.Core.Extensions
{
    public static class Assemblies
    {

        /// <summary>
        /// Gets the path of the assembly on disk. The path will have a trailing slash.
        /// </summary>
        public static string GetPath(this Assembly assembly)
        {
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(assembly.ManifestModule.FullyQualifiedName);
            return Path.GetDirectoryName(Path.GetFullPath(versionInfo.FileName));
        }

        public static string GetVersion(this Assembly assembly)
        {
            if (assembly == null)
                assembly = typeof(Assemblies).Assembly;
            return assembly.GetName().Version.ToString();
        }

        public static string GetCompany(this Assembly assembly)
        {
            if (assembly == null)
                assembly = typeof(Assemblies).Assembly;
            object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
            if (attributes.Length == 0)
                return string.Empty;
            return ((AssemblyCompanyAttribute)attributes[0]).Company;
        }

        public static string GetTrademark(this Assembly assembly)
        {
            if (assembly == null)
                assembly = typeof(Assemblies).Assembly;
            object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyTrademarkAttribute), false);
            if (attributes.Length > 0)
                if (attributes[0] is AssemblyTrademarkAttribute)
                    return (attributes[0] as AssemblyTrademarkAttribute).Trademark;
            return string.Empty;
        }

        public static string GetDescription(this Assembly assembly)
        {
            if (assembly == null)
                assembly = typeof(Assemblies).Assembly;
            object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
            if (attributes.Length > 0)
                if (attributes[0] is AssemblyDescriptionAttribute)
                    return (attributes[0] as AssemblyDescriptionAttribute).Description;
            return string.Empty;
        }

        public static string GetProduct(this Assembly assembly)
        {
            if (assembly == null)
                assembly = typeof(Assemblies).Assembly;
            object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
            if (attributes.Length > 0)
                if (attributes[0] is AssemblyProductAttribute)
                    return (attributes[0] as AssemblyProductAttribute).Product;
            return string.Empty;
        }

        public static string GetCopyright(this Assembly assembly)
        {
            if (assembly == null)
                assembly = typeof(Assemblies).Assembly;
            object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
            if (attributes.Length > 0)
                if (attributes[0] is AssemblyCopyrightAttribute)
                    return (attributes[0] as AssemblyCopyrightAttribute).Copyright;
            return string.Empty;
        }
    }
}
