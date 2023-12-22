// Innovt Company
// Author: Michel Borges
// Project: Innovt.Data.Ado

using System;
using System.Data;
using System.Data.SqlClient;
using Innovt.Core.Utilities;
using Innovt.Data.DataSources;
using Innovt.Data.Exceptions;
using Innovt.Data.Model;
using Npgsql;
using Oracle.ManagedDataAccess.Client;

namespace Innovt.Data.Ado;

/// <summary>
///     Represents a factory for creating database connections based on a data source.
/// </summary>
public class ConnectionFactory : IConnectionFactory
{
    /// <summary>
    ///     Creates a new instance of <see cref="IDbConnection" /> based on the provided data source.
    /// </summary>
    /// <param name="dataSource">The data source for which a connection will be created.</param>
    /// <returns>A new instance of <see cref="IDbConnection" /> based on the data source.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataSource" /> is null.</exception>
    /// <exception cref="ConnectionStringException">Thrown when the connection string is null or empty.</exception>
    public IDbConnection Create(IDataSource dataSource)
    {
        if (dataSource == null) throw new ArgumentNullException(nameof(dataSource));

        var connectionString = dataSource.GetConnectionString();

        if (connectionString.IsNullOrEmpty())
            throw new ConnectionStringException($"Data source {dataSource.Name}");

        return dataSource.Provider switch
        {
            Provider.PostgreSqL => new NpgsqlConnection(connectionString),
            Provider.Oracle => new OracleConnection(connectionString),
            _ => new SqlConnection(connectionString)
        };
    }
}