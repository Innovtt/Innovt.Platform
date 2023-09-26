// Innovt Company
// Author: Michel Borges
// Project: Innovt.Data.EFCore.MsSql

using System.Threading;
using System.Threading.Tasks;
using Innovt.Data.DataSources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Innovt.Data.EFCore.MsSql;
/// <summary>
/// Represents the Entity Framework DbContext for the application.
/// </summary>
public class DBContext : DBContextBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DBContext"/> class using a data source.
    /// </summary>
    /// <param name="dataSource">The data source to use.</param>
    public DBContext(IDataSource dataSource) : base(dataSource)
    {
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="DBContext"/> class using a data source and logger factory.
    /// </summary>
    /// <param name="dataSource">The data source to use.</param>
    /// <param name="loggerFactory">The logger factory to use.</param>
    protected DBContext(IDataSource dataSource, ILoggerFactory loggerFactory) : base(dataSource, loggerFactory)
    {
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="DBContext"/> class using DbContext options.
    /// </summary>
    /// <param name="options">The DbContext options.</param>
    protected DBContext(DbContextOptions options) : base(options)
    {
    }
    /// <summary>
    /// Executes a raw SQL command against the database and returns the number of affected entities.
    /// </summary>
    /// <param name="sql">The SQL command to execute.</param>
    /// <param name="parameters">The parameters for the SQL command.</param>
    /// <returns>The number of affected entities.</returns>
    public override int ExecuteSqlCommand(string sql, params object[] parameters)
    {
        return base.Database.ExecuteSqlRaw(sql, parameters);
    }
    /// <summary>
    /// Asynchronously executes a raw SQL command against the database and returns the number of affected entities.
    /// </summary>
    /// <param name="sql">The SQL command to execute.</param>
    /// <param name="cancellationToken">Cancellation token (optional).</param>
    /// <param name="parameters">The parameters for the SQL command.</param>
    /// <returns>A task representing the asynchronous operation and yielding the number of affected entities.</returns>
    public override Task<int> ExecuteSqlCommandAsync(string sql, CancellationToken cancellationToken = default,
        params object[] parameters)
    {
        return base.Database
            .ExecuteSqlRawAsync(sql, cancellationToken: cancellationToken, parameters: parameters);
    }
    /// <summary>
    /// Configures the provider-specific options for the DbContext.
    /// </summary>
    /// <param name="optionsBuilder">The options builder for configuring DbContext options.</param>
    /// <param name="connectionString">The connection string for the data source.</param>
    protected override void ConfigureProvider(DbContextOptionsBuilder optionsBuilder, string connectionString)
    {
        optionsBuilder.UseSqlServer(connectionString, sqlOptions =>
        {
            if (MaxRetryCount != null && MaxRetryDelay != null)
                sqlOptions.EnableRetryOnFailure(MaxRetryCount.GetValueOrDefault(),
                    MaxRetryDelay.GetValueOrDefault(), null
                );
        });
    }
}