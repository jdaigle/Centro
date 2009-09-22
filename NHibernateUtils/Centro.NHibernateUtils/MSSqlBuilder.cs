using System.Collections.Generic;
using System.Reflection;
using FluentNHibernate.Cfg.Db;
using StructureMap.Attributes;

namespace Centro.NHibernateUtils
{
    public class MSSqlBuilder
    {
        public static NHibernateRegistry CreateRegistry(string connectionStringKey,
                                                        IEnumerable<Assembly> mappingAssemblies,
                                                        InstanceScope sessionInstanceScope)
        {
            var configurer = MsSqlConfiguration.MsSql2005.ConnectionString(c => c.FromConnectionStringWithKey(connectionStringKey));
            return new NHibernateRegistry(configurer, mappingAssemblies, sessionInstanceScope);

        }

        public static NHibernateRegistry CreateRegistry(string sqlServerAddress, string username, string password, string database, IEnumerable<Assembly> mappingAssemblies, InstanceScope sessionInstanceScope)
        {
            var configurer = MsSqlConfiguration.MsSql2005.ConnectionString(c => c.Server(sqlServerAddress)
                                                                                     .Username(username)
                                                                                     .Password(password)
                                                                                     .Database(database));
            return new NHibernateRegistry(configurer, mappingAssemblies, sessionInstanceScope);

        }
    }
}
