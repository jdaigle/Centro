using System.Collections.Generic;
using System.IO;
using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;

namespace Centro.Data
{
    public static class SQLiteBuilder
    {
        public static FluentConfiguration CreateConfiguration(string databaseIdentifier, IEnumerable<Assembly> mappingAssemblies)
        {
            return CreateConfiguration(databaseIdentifier, mappingAssemblies, false);
        }

        public static FluentConfiguration CreateConfiguration(string databaseIdentifier, IEnumerable<Assembly> mappingAssemblies, bool withValidation)
        {
            var dbPath = new FileInfo(Path.GetTempPath() + databaseIdentifier + ".sqlite");
            dbPath.Delete();

            var configurer = SQLiteConfiguration.Standard.ConnectionString(c => c.Is(string.Format("Data Source={0};Version=3;New=True;BinaryGuid=False", dbPath.FullName)));
            var config = FluentConfigurationBuilder.CreateFluentConfiguration(configurer, mappingAssemblies);
            if (withValidation)
                FluentConfigurationBuilder.CreateValidatorEngine();
            return config;
        }

        public static FluentConfiguration CreateConfiguration(IEnumerable<Assembly> mappingAssemblies)
        {
            return CreateConfiguration(mappingAssemblies, false);
        }

        public static FluentConfiguration CreateConfiguration(IEnumerable<Assembly> mappingAssemblies, bool withValidation)
        {
            var config = FluentConfigurationBuilder.CreateFluentConfiguration(SQLiteConfiguration.Standard.InMemory(), mappingAssemblies);
            if (withValidation)
                FluentConfigurationBuilder.CreateValidatorEngine();
            return config;
        }
    }
}
