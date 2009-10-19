using System.Collections.Generic;
using System.Reflection;
using Centro.Data.Validation;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Validator.Engine;
using StructureMap;
using StructureMap.Attributes;
using StructureMap.Configuration.DSL;

namespace Centro.Data
{
    public class NHibernateRegistry : Registry
    {
        private FluentConfigurationBuilder fluentConfigurationBuilder;

        public NHibernateRegistry(IPersistenceConfigurer databaseConfigurer, IEnumerable<Assembly> mappingAssemblies, InstanceScope sessionScope)
            :this(databaseConfigurer, mappingAssemblies, sessionScope, false)
        {
        }

        public NHibernateRegistry(IPersistenceConfigurer databaseConfigurer, IEnumerable<Assembly> mappingAssemblies, InstanceScope sessionScope, bool withValidation)
        {
            fluentConfigurationBuilder = new FluentConfigurationBuilder(databaseConfigurer, mappingAssemblies);
            ValidatorEngine validatorEngine = null;
            if (withValidation)
            {
                validatorEngine = fluentConfigurationBuilder.CreateValidatorEngine();
                NHibernate.Validator.Cfg.Environment.SharedEngineProvider = new StructureMapSharedEngineProvider(validatorEngine);
            }

            ForRequestedType<ISessionFactory>()
                .CacheBy(InstanceScope.Singleton)
                .TheDefault.IsThis(fluentConfigurationBuilder.SessionFactory);

            ForRequestedType<ISession>()
                .CacheBy(sessionScope)
                .TheDefault.Is
                .ConstructedBy(() => ObjectFactory.GetInstance<ISessionFactory>().OpenSession());

            if (withValidation)
            {
                ForRequestedType<ValidatorEngine>()
                    .CacheBy(InstanceScope.Singleton)
                    .TheDefault.IsThis(validatorEngine);

                ForRequestedType<ISharedEngineProvider>()
                    .TheDefault.IsThis(NHibernate.Validator.Cfg.Environment.SharedEngineProvider);

                ForRequestedType<Centro.Validation.IValidator>()
                    .CacheBy(InstanceScope.Singleton)
                    .TheDefault.Is
                    .ConstructedBy(() => new Validator(ObjectFactory.GetInstance<ValidatorEngine>()));
            }
        }

        public NHibernate.Cfg.Configuration Configuration { get { return fluentConfigurationBuilder.Configuration; } }
        public ISessionFactory SessionFactory { get { return fluentConfigurationBuilder.SessionFactory; } }
    }
}
