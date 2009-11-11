using System.Collections.Generic;
using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Validator.Engine;

namespace Centro.Data
{
    public class FluentConfigurationBuilder
    {
        public static FluentConfiguration CreateFluentConfiguration(IPersistenceConfigurer databaseConfigurer, IEnumerable<Assembly> mappingAssemblies, ref NHibernate.Cfg.Configuration configuration)
        {
            if (configuration == null)
                configuration = new NHibernate.Cfg.Configuration();
            return Fluently.Configure(configuration)
                .Database(databaseConfigurer)
                .Mappings(m =>
                    {
                        foreach (var assembly in mappingAssemblies)
                            m.FluentMappings.AddFromAssembly(assembly);
                        foreach (var assembly in mappingAssemblies)
                            m.HbmMappings.AddFromAssembly(assembly);
                    });
        }

        public static ValidatorEngine CreateValidatorEngine()
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
