// Innovt Company
// Author: Michel Borges
// Project: Innovt.Data

using Innovt.Data.Model;
using Microsoft.Extensions.Configuration;

namespace Innovt.Data.DataSources;
/// <summary>
/// Represents a data source writer that inherits from <see cref="DataSourceBase"/> and implements <see cref="IDataSourceWriter"/>.
/// </summary>
public class DataSourceWriter : DataSourceBase, IDataSourceWriter
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DataSourceWriter"/> class with the specified parameters.
    /// </summary>
    /// <param name="name">The name of the data source.</param>
    /// <param name="connectionString">The connection string for the data source.</param>
    /// <param name="provider">The provider for the data source (default is MsSql).</param>
    public DataSourceWriter(string name, string connectionString, Provider provider = Provider.MsSql) : base(name,
        connectionString, provider)
    {
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="DataSourceWriter"/> class with configuration settings.
    /// </summary>
    /// <param name="configuration">The configuration containing connection string settings.</param>
    /// <param name="connectionStringName">The name of the connection string.</param>
    /// <param name="provider">The provider for the data source (default is MsSql).</param>
    public DataSourceWriter(IConfiguration configuration, string connectionStringName,
        Provider provider = Provider.MsSql) : base(configuration, connectionStringName, provider)
    {
        Name = nameof(DataSourceWriter);
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="DataSourceWriter"/> class with configuration settings and a custom name.
    /// </summary>
    /// <param name="configuration">The configuration containing connection string settings.</param>
    /// <param name="name">The custom name of the data source.</param>
    /// <param name="connectionStringName">The name of the connection string.</param>
    /// <param name="provider">The provider for the data source (default is MsSql).</param>
    public DataSourceWriter(IConfiguration configuration, string name, string connectionStringName,
        Provider provider = Provider.MsSql) : base(configuration, name, connectionStringName, provider)
    {
    }
}