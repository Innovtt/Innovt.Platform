// Innovt Company
// Author: Michel Borges
// Project: Innovt.Data

using System.Diagnostics.CodeAnalysis;
using Innovt.Data.Model;
using Microsoft.Extensions.Configuration;

namespace Innovt.Data.DataSources;

/// <summary>
/// Represents a data source reader that inherits from <see cref="DataSourceBase"/> and implements <see cref="IDataSourceReader"/>.
/// </summary>
public class DataSourceReader : DataSourceBase, IDataSourceReader
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DataSourceReader"/> class with the specified parameters.
    /// </summary>
    /// <param name="name">The name of the data source.</param>
    /// <param name="connectionString">The connection string for the data source.</param>
    /// <param name="provider">The provider for the data source (default is MsSql).</param>
    public DataSourceReader([NotNull] string name, [NotNull] string connectionString,
        Provider provider = Provider.MsSql) : base(name, connectionString, provider)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataSourceReader"/> class with configuration settings.
    /// </summary>
    /// <param name="configuration">The configuration containing connection string settings.</param>
    /// <param name="connectionStringName">The name of the connection string.</param>
    /// <param name="provider">The provider for the data source (default is MsSql).</param>
    public DataSourceReader([NotNull] IConfiguration configuration, [NotNull] string connectionStringName,
        Provider provider = Provider.MsSql) : base(configuration, connectionStringName, provider)
    {
        Name = nameof(DataSourceReader);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataSourceReader"/> class with configuration settings and a custom name.
    /// </summary>
    /// <param name="configuration">The configuration containing connection string settings.</param>
    /// <param name="name">The custom name of the data source.</param>
    /// <param name="connectionStringName">The name of the connection string.</param>
    /// <param name="provider">The provider for the data source (default is MsSql).</param>
    public DataSourceReader(IConfiguration configuration, string name, string connectionStringName,
        Provider provider = Provider.MsSql) : base(configuration, name, connectionStringName, provider)
    {
    }
}