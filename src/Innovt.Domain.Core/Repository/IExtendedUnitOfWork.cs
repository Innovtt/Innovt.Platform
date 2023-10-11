// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain.Core

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Innovt.Domain.Core.Repository;

/// <summary>
/// Extended Unit of Work Interface To Provide More Features To Unit Of Work When Using Entity Framework
/// </summary>
public interface IExtendedUnitOfWork : IUnitOfWork
{
    /// <summary>
    /// Add Entity To Unit Of Work Context
    /// </summary>
    /// <param name="entity"></param>
    /// <typeparam name="T"></typeparam>
    void Add<T>(T entity) where T : class;

    /// <summary>
    /// Add Entities To Unit Of Work Context
    /// </summary>
    /// <param name="entities"></param>
    /// <typeparam name="T"></typeparam>
    void Add<T>(IEnumerable<T> entities) where T : class;

    /// <summary>
    /// Asynchronously adds an entity or a collection of entities.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task AddAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Asynchronously adds multiple entities of type T to the repository.
    /// </summary>
    /// <typeparam name="T">The type of the entities.</typeparam>
    /// <param name="entities">The entities to add.</param>
    /// <param name="cancellationToken">The optional cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>e asynchronous operation.</returns>
    Task AddAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Removes an entity of type T from the repository.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="entity">The entity to remove.</param>
    /// <returns></returns>
    void Remove<T>(T entity) where T : class;

    /// <summary>
    /// Removes multiple entities of type T from the repository.
    /// </summary>
    /// <typeparam name="T">The type of the entities.</typeparam>
    /// <param name="entities">The entities to remove.</param>
    void Remove<T>(IEnumerable<T> entities) where T : class;

    /// <summary>
    /// Updates an entity of type T in the repository.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="entity">The entity to update.</param>
    void Update<T>(T entity) where T : class;

    /// <summary>
    /// Attaches an entity of type T to the repository.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="entity">The entity to attach.</param>
    void Attach<T>(T entity) where T : class;

    /// <summary>
    /// Detaches an entity of type T from the repository.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="entity">The entity to detach.</param>
    void Detach<T>(T entity) where T : class;

    /// <summary>
    /// Executes a SQL command.
    /// </summary>
    /// <param name="sql">The SQL command to execute.</param>
    /// <param name="parameters">The parameters for the SQL command.</param>
    /// <returns>The number of affected rows.</returns>
    int ExecuteSqlCommand(string sql, params object[] parameters);

    /// <summary>
    /// Asynchronously executes a SQL command.
    /// </summary>
    /// <param name="sql">The SQL command to execute.</param>
    /// <param name="cancellationToken">The optional cancellation token.</param>
    /// <param name="parameters">The parameters for the SQL command.</param>
    /// <returns>A task representing the asynchronous operation, with the number of affected rows.</returns>
    Task<int> ExecuteSqlCommandAsync(string sql, CancellationToken cancellationToken = default,
        params object[] parameters);

    /// <summary>
    /// Returns a queryable interface for entities of type T.
    /// </summary>
    /// <typeparam name="T">The type of the entities.</typeparam>
    /// <returns>A queryable interface for the entities.</returns>
    IQueryable<T> Queryable<T>() where T : class;
}