using System.Collections.Generic;
using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;

namespace Centro.Data
{
    public class MSSqlBuilder
    {
        public static FluentConfiguration CreateConfiguration(string connectionStringKey,
                                                              IEnumerable<Assembly> mappingAssemblies,
                                                              ref NHibernate.Cfg.Configuration configuration)
        {
            return CreateConfiguration(connectionStringKey, mappingAssemblies, ref configuration, false);
        }

        public static FluentConfiguration CreateConfiguration(string connectionStringKey,
                                                              IEnumerable<Assembly> mappingAssemblies,
                                                              ref NHibernate.Cfg.Configuration configuration, bool withValidation)
        {
            var configurer = MsSqlConfiguration.MsSql2005.ConnectionString(c => c.FromConnectionStringWithKey(connectionStringKey));
            var config = FluentConfigurationBuilder.CreateFluentConfiguration(configurer, mappingAssemblies, ref configuration);
            if (withValidation)
                FluentConfigurationBuilder.CreateValidatorEngine();
            return config;
        }

        public static FluentConfiguration CreateConfiguration(string sqlServerAddress, string username, string password, string database, IEnumerable<Assembly> mappingAssemblies, ref NHibernate.Cfg.Configuration configuration)
        {
            return CreateConfiguration(sqlServerAddress, username, password, database, mappingAssemblies, ref configuration, false);
        }

        public static FluentConfiguration CreateConfiguration(string sqlServerAddress, string username, string password, string database, IEnumerable<Assembly> mappingAssemblies, ref NHibernate.Cfg.Configuration configuration, bool withValidation)
        {
            var configurer = MsSqlConfiguration.MsSql2005.ConnectionString(c => c.Server(sqlServerAddress)
                                                                                     .Username(username)
                                                                                     .Password(password)
                                                                                     .Database(database));
            var config = FluentConfigurationBuilder.CreateFluentConfiguration(configurer, mappingAssemblies, ref configuration);
            if (withValidation)
                FluentConfigurationBuilder.CreateValidatorEngine();
            return config;
        }
    }
}
