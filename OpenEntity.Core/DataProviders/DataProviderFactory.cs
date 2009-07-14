using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Globalization;
using OpenEntity.Extensions;

namespace OpenEntity.DataProviders
{
    /// <summary>
    /// An object which can create new instances of an IDataProvider.
    /// </summary>
    public static class DataProviderFactory
    {
        /// <summary>
        /// Creates a new provider for the specified connection string.
        /// </summary>
        /// <param name="connectionStringSettings">The connection string settings.</param>
        /// <returns>
        /// The create IDataProvider. It should not return null.
        /// </returns>
        internal static IDataProvider CreateNewProvider(ConnectionStringSettings connectionStringSettings)
        {
            return DataProviderFactory.CreateNewProvider(connectionStringSettings.ConnectionString, connectionStringSettings.ProviderName, string.Empty);
        }

        /// <summary>
        /// Creates a new provider for the specified connection string.
        /// </summary>
        /// <param name="connectionStringSettings">The connection string settings.</param>
        /// <param name="defaultSchemaName">The name of the default schema.</param>
        /// <returns>
        /// The create IDataProvider. It should not return null.
        /// </returns>
        internal static IDataProvider CreateNewProvider(ConnectionStringSettings connectionStringSettings, string defaultSchemaName)
        {
            return DataProviderFactory.CreateNewProvider(connectionStringSettings.ConnectionString, connectionStringSettings.ProviderName, defaultSchemaName);
        }

        /// <summary>
        /// Creates a new provider for the specified connection string.a.
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string.</param>
        /// <returns>
        /// The create IDataProvider. It should not return null.
        /// </returns>
        public static IDataProvider CreateNewProvider(string connectionStringName)
        {
            var connectionStringSettings = ConfigurationManager.ConnectionStrings[connectionStringName];
            if (connectionStringSettings == null)
                throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "Missing connection string named [{0}]", connectionStringName));
            return DataProviderFactory.CreateNewProvider(connectionStringSettings);
        }

        /// <summary>
        /// Creates a new provider for the specified connection string.
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string.</param>
        /// <param name="defaultSchemaName">The name of the default schema.</param>
        /// <returns>
        /// The create IDataProvider. It should not return null.
        /// </returns>
        public static IDataProvider CreateNewProvider(string connectionStringName, string defaultSchemaName)
        {
            var connectionStringSettings = ConfigurationManager.ConnectionStrings[connectionStringName];
            if (connectionStringSettings == null)
                throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "Missing connection string named [{0}]", connectionStringName));
            return DataProviderFactory.CreateNewProvider(connectionStringSettings, defaultSchemaName);
        }

        /// <summary>
        /// Creates a new provider for the specified connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="providerName">Name of the data provider client.</param>
        /// <param name="defaultSchemaName">The name of the default schema.</param>
        /// <returns>
        /// The create IDataProvider. It should not return null.
        /// </returns>
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
