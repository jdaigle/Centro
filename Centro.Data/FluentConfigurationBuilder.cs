using System.Collections.Generic;
using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;

namespace Centro.Data
{
    public class FluentConfigurationBuilder
    {
        public FluentConfigurationBuilder(IPersistenceConfigurer databaseConfigurer, IEnumerable<Assembly> mappingAssemblies)
        {
            SessionFactory = CreateSessionFactory(databaseConfigurer, mappingAssemblies);
        }

        public NHibernate.Cfg.Configuration Configuration { get; private set; }
        public ISessionFactory SessionFactory { get; private set; }

        private ISessionFactory CreateSessionFactory(IPersistenceConfigurer databaseConfigurer, IEnumerable<Assembly> mappingAssemblies)
        {
            Configuration = new NHibernate.Cfg.Configuration();

            var factory = Fluently.Configure(Configuration)
                .Database(databaseConfigurer)
                .Mappings(m =>
                    {
                        foreach (var assembly in mappingAssemblies)
                            m.FluentMappings.AddFromAssembly(assembly);
                    })
                .BuildSessionFactory();

            return factory;
        }

    }
}
