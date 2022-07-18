// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Data.Ado
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Core.Utilities;
using Innovt.Data.DataSources;
using Innovt.Data.Exceptions;
using Innovt.Data.Model;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Innovt.Data.Ado;

public class ConnectionFactory : IConnectionFactory
{
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