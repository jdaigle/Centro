using System.Collections.Generic;
using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Cfg;
using Centro.Data.Validation;

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
                        foreach (var assembly in mappingAssemblies)
                            m.HbmMappings.AddFromAssembly(assembly);
                    })
                .BuildSessionFactory();
            return factory;
        }

        public ValidatorEngine CreateValidatorEngine()
        {
            //Environment.SharedEngineProvider = new SharedValidatorProvider();
            var nhvc = new NHibernate.Validator.Cfg.Loquacious.FluentConfiguration();            
            nhvc.SetDefaultValidatorMode(ValidatorMode.UseAttribute);
            //nhvc.IntegrateWithNHibernate.ApplyingDDLConstraints().And.RegisteringListeners();

            var validatorEngine = new ValidatorEngine();
            validatorEngine.Configure(nhvc);
            //ValidatorInitializer.Initialize(Configuration, validatorEngine);
            return validatorEngine;
        }

    }
}
