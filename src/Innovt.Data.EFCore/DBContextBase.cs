// Innovt Company
// Author: Michel Borges
// Project: Innovt.Data.EFCore

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Core.Utilities;
using Innovt.Data.DataSources;
using Innovt.Data.Exceptions;
using Innovt.Domain.Core.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Innovt.Data.EFCore;

/// <summary>
///     Abstract base class for Entity Framework DbContext implementing the extended unit of work interface.
/// </summary>
public abstract class DBContextBase : DbContext, IExtendedUnitOfWork
{
    private readonly IDataSource dataSource;
    private readonly ILoggerFactory loggerFactory;

    /// <summary>
    ///     Initializes a new instance of the <see cref="DBContextBase" /> class using a data source.
    /// </summary>
    /// <param name="dataSource">The data source to use.</param>
    /// <exception cref="ArgumentNullException">Thrown when the data source is null.</exception>
    protected DBContextBase(IDataSource dataSource)
    {
        this.dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
        base.ChangeTracker.LazyLoadingEnabled = false;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="DBContextBase" /> class using a data source and logger factory.
    /// </summary>
    /// <param name="dataSource">The data source to use.</param>
    /// <param name="loggerFactory">The logger factory to use.</param>
    /// <exception cref="ArgumentNullException">Thrown when the data source or logger factory is null.</exception>
    protected DBContextBase(IDataSource dataSource, ILoggerFactory loggerFactory) : this(dataSource)
    {
        this.loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="DBContextBase" /> class using DbContext options.
    /// </summary>
    /// <param name="options">The DbContext options.</param>
    protected DBContextBase(DbContextOptions options) : base(options)
    {
        base.ChangeTracker.LazyLoadingEnabled = false;
    }

    /// <summary>
    ///     Gets or sets the maximum number of retries for a transaction.
    /// </summary>
    public int? MaxRetryCount { get; set; }

    /// <summary>
    ///     Gets or sets the maximum delay between retries for a transaction.
    /// </summary>
    public TimeSpan? MaxRetryDelay { get; set; }

    /// <summary>
    ///     Commits the changes made in the unit of work to the database.
    /// </summary>
    /// <returns>The number of entities written to the database.</returns>
    public int Commit()
    {
        return SaveChanges();
    }

    /// <summary>
    ///     Asynchronously commits the changes made in the unit of work to the database.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token (optional).</param>
    /// <returns>The number of entities written to the database.</returns>
    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        return await SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    ///     Rolls back the changes made in the unit of work by setting all entries to an unchanged state.
    /// </summary>
    public void Rollback()
    {
        base.ChangeTracker.Entries()
            .ToList()
            .ForEach(entry => entry.State = EntityState.Unchanged);
    }

    /// <summary>
    ///     Adds an entity to the DbContext.
    /// </summary>
    /// <typeparam name="T">The type of entity to add.</typeparam>
    /// <param name="entity">The entity to add.</param>
    public new void Add<T>(T entity) where T : class
    {
        base.Add(entity);
    }

    /// <summary>
    ///     Adds a collection of entities to the DbContext.
    /// </summary>
    /// <typeparam name="T">The type of entities to add.</typeparam>
    /// <param name="entities">The collection of entities to add.</param>
    public void Add<T>(IEnumerable<T> entities) where T : class
    {
        base.AddRange(entities);
    }

    /// <summary>
    ///     Asynchronously adds an entity to the DbContext.
    /// </summary>
    /// <typeparam name="T">The type of entity to add.</typeparam>
    /// <param name="entity">The entity to add.</param>
    /// <param name="cancellationToken">Cancellation token (optional).</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public new async Task AddAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class
    {
        await base.AddAsync(entity, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    ///     Asynchronously adds a collection of entities to the DbContext.
    /// </summary>
    /// <typeparam name="T">The type of entities to add.</typeparam>
    /// <param name="entities">The collection of entities to add.</param>
    /// <param name="cancellationToken">Cancellation token (optional).</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task AddAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        where T : class
    {
        await base.AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    ///     Removes an entity from the DbContext.
    /// </summary>
    /// <typeparam name="T">The type of entity to remove.</typeparam>
    /// <param name="entity">The entity to remove.</param
    public new void Remove<T>(T entity) where T : class
    {
        base.Remove(entity);
    }

    /// <summary>
    ///     Removes a collection of entities from the DbContext.
    /// </summary>
    /// <typeparam name="T">The type of entities to remove.</typeparam>
    /// <param name="entities">The collection of entities to remove.</param>
    public void Remove<T>(IEnumerable<T> entities) where T : class
    {
        base.RemoveRange(entities);
    }

    /// <summary>
    ///     Updates the specified entity in the DbContext.
    /// </summary>
    /// <typeparam name="T">The type of entity to update.</typeparam>
    /// <param name="entity">The entity to update.</param>
    public new void Update<T>(T entity) where T : class
    {
        base.Update(entity);
    }

    /// <summary>
    ///     Attaches the specified entity to the DbContext.
    /// </summary>
    /// <typeparam name="T">The type of entity to attach.</typeparam>
    /// <param name="entity">The entity to attach.</param>
    public new void Attach<T>(T entity) where T : class
    {
        base.Attach(entity);
    }

    /// <summary>
    ///     Detaches an entity from the DbContext.
    /// </summary>
    /// <typeparam name="T">The type of entity to detach.</typeparam>
    /// <param name="entity">The entity to detach.</param>
    public void Detach<T>(T entity) where T : class
    {
        base.Entry(entity).State = EntityState.Detached;
    }

    /// <summary>
    ///     Gets a queryable representation of a specific entity type.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    /// <returns>An <see cref="IQueryable{T}" /> representing the entity type.</returns>
    public IQueryable<T> Queryable<T>() where T : class
    {
        return base.Set<T>();
    }

    /// <summary>
    ///     Executes a SQL command against the database.
    /// </summary>
    /// <param name="sql">The SQL command to execute.</param>
    /// <param name="parameters">The parameters for the SQL command.</param>
    /// <returns>The number of entities affected by the SQL command.</returns>
    public abstract int ExecuteSqlCommand(string sql, params object[] parameters);

    /// <summary>
    ///     Asynchronously executes a SQL command against the database.
    /// </summary>
    /// <param name="sql">The SQL command to execute.</param>
    /// <param name="cancellationToken">Cancellation token (optional).</param>
    /// <param name="parameters">The parameters for the SQL command.</param>
    /// <returns>
    ///     A task representing the asynchronous operation and yielding the number of entities affected by the SQL
    ///     command.
    /// </returns>
    public abstract Task<int> ExecuteSqlCommandAsync(string sql, CancellationToken cancellationToken = default,
        params object[] parameters);

    /// <summary>
    ///     Overrides the configuration of DbContext options.
    /// </summary>
    /// <param name="optionsBuilder">The options builder for configuring DbContext options.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder == null) throw new ArgumentNullException(nameof(optionsBuilder));

        optionsBuilder.EnableSensitiveDataLogging(false);
        optionsBuilder.EnableDetailedErrors();

        if (loggerFactory != null) optionsBuilder.UseLoggerFactory(loggerFactory);

        if (dataSource != null)
        {
            var connectionString = dataSource.GetConnectionString();

            if (connectionString.IsNullOrEmpty())
                throw new ConnectionStringException($"Connection string for datasource {dataSource.Name} is empty.");


            ConfigureProvider(optionsBuilder, connectionString);
        }

        base.OnConfiguring(optionsBuilder);
    }

    /// <summary>
    ///     Configures the provider-specific options for the DbContext.
    /// </summary>
    /// <param name="optionsBuilder">The options builder for configuring DbContext options.</param>
    /// <param name="connectionString">The connection string for the data source.</param>
    protected abstract void ConfigureProvider(DbContextOptionsBuilder optionsBuilder, string connectionString);
}