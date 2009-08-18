using System;
using System.Configuration;
using System.Globalization;
using Centro.OpenEntity.Extensions;

namespace Centro.OpenEntity.DataProviders
{
    public static class DataProviderFactory
    {
        internal static IDataProvider CreateNewProvider(ConnectionStringSettings connectionStringSettings)
        {
            return DataProviderFactory.CreateNewProvider(connectionStringSettings.ConnectionString, connectionStringSettings.ProviderName, string.Empty);
        }

        internal static IDataProvider CreateNewProvider(ConnectionStringSettings connectionStringSettings, string defaultSchemaName)
        {
            return DataProviderFactory.CreateNewProvider(connectionStringSettings.ConnectionString, connectionStringSettings.ProviderName, defaultSchemaName);
        }

        public static IDataProvider CreateNewProvider(string connectionStringName)
        {
            var connectionStringSettings = ConfigurationManager.ConnectionStrings[connectionStringName];
            if (connectionStringSettings == null)
                throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "Missing connection string named [{0}]", connectionStringName));
            return DataProviderFactory.CreateNewProvider(connectionStringSettings);
        }

        public static IDataProvider CreateNewProvider(string connectionStringName, string defaultSchemaName)
        {
            var connectionStringSettings = ConfigurationManager.ConnectionStrings[connectionStringName];
            if (connectionStringSettings == null)
                throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "Missing connection string named [{0}]", connectionStringName));
            return DataProviderFactory.CreateNewProvider(connectionStringSettings, defaultSchemaName);
        }

        public static IDataProvider CreateNewProvider(string connectionString, string providerName, string defaultSchemaName)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("connectionString");
            if (string.IsNullOrEmpty(providerName))
                throw new ArgumentNullException("providerName");
            if (providerName.Matches(SqlDbClientTypeName.MSSQL))
            {
                return new SqlServerDataProvider(connectionString, defaultSchemaName);
            }
            else if (providerName.Matches(SqlDbClientTypeName.MSORACLE))
            {
                throw new NotSupportedException();
               // return new MSOracleDataProvider(connectionString, schema);
            }
            else
            {
                throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "The provider [{0}] is not supported.", providerName));
            }
        }
    }
}
