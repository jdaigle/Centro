using System.Collections.Generic;
using System.IO;
using System.Reflection;
using FluentNHibernate.Cfg.Db;
using StructureMap.Attributes;

namespace Centro.Data
{
    public static class SQLiteBuilder
    {
        public static NHibernateRegistry CreateRegistry(string databaseIdentifier, IEnumerable<Assembly> mappingAssemblies, InstanceScope sessionInstanceScope)
        {
            var dbPath = new FileInfo(Path.GetTempPath() + databaseIdentifier + ".sqlite");
            dbPath.Delete();

            var configurer = SQLiteConfiguration.Standard.ConnectionString(c => c.Is(string.Format("Data Source={0};Version=3;New=True;BinaryGuid=False", dbPath.FullName)));
            return new NHibernateRegistry(configurer, mappingAssemblies, sessionInstanceScope);
        }

        public static NHibernateRegistry CreateRegistry(IEnumerable<Assembly> mappingAssemblies, InstanceScope sessionInstanceScope)
        {
            return new NHibernateRegistry(SQLiteConfiguration.Standard.InMemory(), mappingAssemblies, sessionInstanceScope);
        }
    }
}
