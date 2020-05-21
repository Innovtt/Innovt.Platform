using System;
using System.Configuration;
using Innovt.Core.Exceptions;
using Innovt.Data.Exceptions;
using Innovt.Data.Model;
using Microsoft.Extensions.Configuration;

namespace Innovt.Data.DataSources
{
    /// <summary>
    /// The Default DataSource is using ConfigurationManager
    /// </summary>
    public abstract class DataSourceBase : IDataSource
    {
        private  string connectionString = null;

        public string Name { get; set; }

        public Provider Provider { get; private set; }

        protected DataSourceBase(string connectionStringName,Provider provider = Provider.MsSql)
        {
            this.Provider = provider;

            SetConnectionString(connectionStringName);
        }

        protected DataSourceBase(IConfiguration configuration, string connectionStringName,Provider provider = Provider.MsSql)
        {
            this.Provider = provider;

            SetConnectionString(configuration, connectionStringName);
        }

        private void SetConnectionString(string connectionStringName)
        {
            Name = connectionStringName;

        #if (!NETCOREAPP2_0 && !NETCOREAPP2_1)
            if (connectionStringName == null) throw new ArgumentNullException(nameof(connectionStringName));

            var localConnectionString = ConfigurationManager.ConnectionStrings[connectionStringName]?.ConnectionString;

            if (string.IsNullOrEmpty(localConnectionString))
                throw new CriticalException($"Connection string {connectionStringName} not found or null.");

            this.connectionString = localConnectionString;

            #else
                var builder = new Microsoft.Extensions.Configuration.ConfigurationBuilder();

                SetConnectionString(builder.Build(), connectionStringName);
            #endif
        }

        private void SetConnectionString(IConfiguration configuration, string name)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (name == null) throw new ArgumentNullException(nameof(name));

            var localConnectionString = configuration.GetConnectionString(name);

            if (string.IsNullOrEmpty(localConnectionString))
                throw new ConnectionStringException($"Connection string {name} not found or null.");

            this.Name = name;
            this.connectionString = localConnectionString;
        }


        public string GetConnectionString()
        {
            return connectionString;
        }
    }
}