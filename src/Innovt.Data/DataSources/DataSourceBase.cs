// Innovt Company
// Author: Michel Borges
// Project: Innovt.Data

using System;
using Innovt.Data.Exceptions;
using Innovt.Data.Model;
using Microsoft.Extensions.Configuration;

namespace Innovt.Data.DataSources;

/// <summary>
///     The Default DataSource is using ConfigurationManager
/// </summary>
public abstract class DataSourceBase : IDataSource
{
    private string connectionString;

    /// <summary>
    ///     Initializes a new instance of the <see cref="DataSourceBase" /> class with the specified parameters.
    /// </summary>
    /// <param name="name">The name of the data source.</param>
    /// <param name="connectionString">The connection string for the data source.</param>
    /// <param name="provider">The provider for the data source (default is MsSql).</param>
    protected DataSourceBase(string name, string connectionString, Provider provider = Provider.MsSql)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Provider = provider;
        this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="DataSourceBase" /> class with configuration settings.
    /// </summary>
    /// <param name="configuration">The configuration containing connection string settings.</param>
    /// <param name="connectionStringName">The name of the connection string.</param>
    /// <param name="provider">The provider for the data source (default is MsSql).</param>
    protected DataSourceBase(IConfiguration configuration, string connectionStringName,
        Provider provider = Provider.MsSql)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));
        if (connectionStringName == null) throw new ArgumentNullException(nameof(connectionStringName));
        
        Provider = provider;
        SetConnectionString(configuration, connectionStringName);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="DataSourceBase" /> class with configuration settings and a custom
    ///     name.
    /// </summary>
    /// <param name="configuration">The configuration containing connection string settings.</param>
    /// <param name="name">The custom name of the data source.</param>
    /// <param name="connectionStringName">The name of the connection string.</param>
    /// <param name="provider">The provider for the data source (default is MsSql).</param>
    protected DataSourceBase(IConfiguration configuration, string name, string connectionStringName,
        Provider provider = Provider.MsSql)
    {
        Provider = provider;
        Name = name;

        SetConnectionString(configuration, connectionStringName);
    }

    /// <summary>
    ///     Gets or sets the name of the data source.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Gets the provider for the data source.
    /// </summary>
    public Provider Provider { get; }

    /// <summary>
    ///     Retrieves the connection string for the data source.
    /// </summary>
    /// <returns>The connection string for the data source.</returns>
    public string GetConnectionString()
    {
        return connectionString;
    }

    /// <summary>
    ///     Sets the connection string for the data source based on the provided configuration and connection string name.
    /// </summary>
    /// <param name="configuration">The configuration containing connection string settings.</param>
    /// <param name="name">The name of the connection string.</param>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="configuration" /> or <paramref name="name" /> is
    ///     null.
    /// </exception>
    /// <exception cref="ConnectionStringException">Thrown when the connection string is not found or null.</exception>
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