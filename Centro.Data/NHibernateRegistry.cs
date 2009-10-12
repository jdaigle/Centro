using System.Collections.Generic;
using System.Reflection;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using StructureMap;
using StructureMap.Attributes;
using StructureMap.Configuration.DSL;

namespace Centro.NHibernateUtils
{
    public class NHibernateRegistry : Registry
    {
        private FluentConfigurationBuilder fluentConfigurationBuilder;

        public NHibernateRegistry(IPersistenceConfigurer databaseConfigurer, IEnumerable<Assembly> mappingAssemblies, InstanceScope sessionScope)
        {
            fluentConfigurationBuilder = new FluentConfigurationBuilder(databaseConfigurer, mappingAssemblies);

            ForRequestedType<ISessionFactory>()
                .CacheBy(InstanceScope.Singleton)
                .TheDefault.IsThis(fluentConfigurationBuilder.SessionFactory);

            ForRequestedType<ISession>()
                .CacheBy(sessionScope)
                .TheDefault.Is.
                ConstructedBy(() =>
                    ObjectFactory.GetInstance<ISessionFactory>().OpenSession()
                    );
        }

        public NHibernate.Cfg.Configuration Configuration { get { return fluentConfigurationBuilder.Configuration; } }
        public ISessionFactory SessionFactory { get { return fluentConfigurationBuilder.SessionFactory; } }
    }
}
