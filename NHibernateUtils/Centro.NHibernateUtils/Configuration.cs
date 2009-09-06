using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System.Reflection;
using System.IO;
using StructureMap;
using StructureMap.Attributes;

namespace Centro.NHibernateUtils
{
    public class Configuration
    {
        public static NHibernate.Cfg.Configuration SetupSQLiteDataAccess(IInitializationExpression i, 
                                                                         InstanceScope sessionInstanceScope,
                                                                         IEnumerable<Assembly> mappingAssemblies,
                                                                         string databaseIdentifier)
        {
            var dbPath = new FileInfo(Path.GetTempPath() + databaseIdentifier + ".sqlite");
            dbPath.Delete();

            return ConfigureDataAccess(SQLiteConfiguration.Standard.ConnectionString(string.Format("Data Source={0};Version=3;New=True;BinaryGuid=False", dbPath.FullName)), mappingAssemblies, i, sessionInstanceScope);
        }

        public static NHibernate.Cfg.Configuration ConfigureMsSqlDataAccess(IInitializationExpression i, 
                                                                            InstanceScope sessionInstanceScope,
                                                                            IEnumerable<Assembly> mappingAssemblies,
                                                                            string connectionStringKey)
        {
            return ConfigureDataAccess(MsSqlConfiguration.MsSql2005.ConnectionString(c => c.FromConnectionStringWithKey(connectionStringKey)),
                                                                                    mappingAssemblies, i, sessionInstanceScope);

        }

        public static NHibernate.Cfg.Configuration ConfigureDataAccess(IPersistenceConfigurer databaseConfigurer,
                                                                       IEnumerable<Assembly> mappingAssemblies,
                                                                       IInitializationExpression i,
                                                                       InstanceScope sessionInstanceScope)
        {
            var cfg = new NHibernate.Cfg.Configuration();
            i.ForRequestedType<ISessionFactory>()
                .CacheBy(InstanceScope.Singleton)
                .TheDefault.IsThis(Configuration.ConfigureNHibernate(cfg, databaseConfigurer, mappingAssemblies));

            i.ForRequestedType<ISession>()
                .CacheBy(sessionInstanceScope)
                .TheDefault.Is.
                ConstructedBy(() => ObjectFactory.GetInstance<ISessionFactory>().OpenSession());

            return cfg;
        }

        private static ISessionFactory ConfigureNHibernate(NHibernate.Cfg.Configuration cfg, 
                                                          IPersistenceConfigurer databaseConfigurer, 
                                                          IEnumerable<Assembly> mappingAssemblies)
        {
            var factory = Fluently.Configure(cfg)
                .Database(databaseConfigurer)
                .Mappings(m =>
                    {
                        foreach (var assembly in mappingAssemblies)
                        {
                            m.FluentMappings.AddFromAssembly(assembly).Conventions.AddAssembly(assembly);
                        }
                    })
                //.ExposeConfiguration(c =>
                //{
                //    c.SetProperty("adonet.batch_size", "5");
                //    c.SetProperty("generate_statistics", "true");
                //    c.SetProperty("proxyfactory.factory_class", typeof(NHibernate.ByteCode.Castle.ProxyFactoryFactory).AssemblyQualifiedName);
                //})
                .BuildSessionFactory();

            return factory;
        }
    }
}
