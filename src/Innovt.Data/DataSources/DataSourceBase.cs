// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Data
// Solution: Innovt.Platform
// Date: 2021-04-08
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using Innovt.Data.Exceptions;
using Innovt.Data.Model;
using Microsoft.Extensions.Configuration;

namespace Innovt.Data.DataSources
{
    /// <summary>
    ///     The Default DataSource is using ConfigurationManager
    /// </summary>
    public abstract class DataSourceBase : IDataSource
    {
        private string connectionString;

        protected DataSourceBase(string name, string connectionString, Provider provider = Provider.MsSql)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Provider = provider;
            this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }


        protected DataSourceBase(IConfiguration configuration, string connectionStringName,
            Provider provider = Provider.MsSql)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (connectionStringName == null) throw new ArgumentNullException(nameof(connectionStringName));


            Provider = provider;
            SetConnectionString(configuration, connectionStringName);
        }

        protected DataSourceBase(IConfiguration configuration, string name, string connectionStringName,
            Provider provider = Provider.MsSql)
        {
            Provider = provider;
            Name = name;

            SetConnectionString(configuration, connectionStringName);
        }

        public string Name { get; set; }

        public Provider Provider { get; }

        public string GetConnectionString()
        {
            return connectionString;
        }

        private void SetConnectionString(IConfiguration configuration, string name)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (name == null) throw new ArgumentNullException(nameof(name));

            var localConnectionString = configuration.GetConnectionString(name);

            if (string.IsNullOrEmpty(localConnectionString))
                throw new ConnectionStringException($"Connection string {name} not found or null.");

            Name = name;
            connectionString = localConnectionString;
        }
    }
}