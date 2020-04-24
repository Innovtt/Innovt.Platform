using System;
using System.Configuration;
using Microsoft.Extensions.Configuration;


namespace Innovt.Data
{
    /// <summary>
    /// The Default DataSource is using ConfigurationManager
    /// </summary>
    public class DefaultDataSource : IDataSource
    {
        private  string connectionString = null;

        public DefaultDataSource(string connectionStringName)
        {
            SetConnectionString(connectionStringName);
        }

        public DefaultDataSource(IConfiguration configuration, string connectionStringName)
        {
            SetConnectionString(configuration, connectionStringName);
        }

        private void SetConnectionString(string connectionStringName)
        {
        #if (!NETCOREAPP2_0 && !NETCOREAPP2_1)
            if (connectionStringName == null) throw new ArgumentNullException(nameof(connectionStringName));

            var localConnectionString = ConfigurationManager.ConnectionStrings[connectionStringName]?.ConnectionString;

            if (string.IsNullOrEmpty(localConnectionString))
                throw new Exception($"Connection string {connectionStringName} not found or null.");

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
                throw new Exception($"Connection string {name} not found or null.");

            this.connectionString = localConnectionString;
        }

        public string GetConnectionString()
        {
            return connectionString;
        }
    }
}