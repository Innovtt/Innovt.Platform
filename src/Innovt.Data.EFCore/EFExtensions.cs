// Innovt Company
// Author: Michel Borges
// Project: Innovt.Data.EFCore

using System;
using System.Collections.Generic;
using System.Linq;
using Innovt.Domain.Core.Repository;
using Innovt.Domain.Core.Specification;
using Microsoft.EntityFrameworkCore;

namespace Innovt.Data.EFCore;
#nullable enable
/// <summary>
///     Extension methods for Entity Framework IQueryable to facilitate query operations.
/// </summary>
public static class EfExtensions
{
    /// <summary>
    ///     Adds navigation properties to include in the query.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    /// <param name="query">The IQueryable to which includes are added.</param>
    /// <param name="includes">The navigation properties to include.</param>
    /// <returns>The IQueryable with added includes.</returns>
    public static IQueryable<T> AddInclude<T>(this IQueryable<T> query, Include? includes) where T : class
    {
        return includes == null || includes.IsEmpty()
            ? query
            : includes.Includes.Aggregate(query, (current, include) => current.Include(include));
    }

    /// <summary>
    ///     Adds navigation properties to include in the query using string-based includes.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    /// <param name="query">The IQueryable to which includes are added.</param>
    /// <param name="includes">Array of navigation properties to include.</param>
    /// <returns>The IQueryable with added includes.</returns>
    public static IQueryable<T> AddInclude<T>(this IQueryable<T> query, params string[] includes) where T : class
    {
        return includes is null ? query : includes.Aggregate(query, (current, include) => current.Include(include));
    }

    /// <summary>
    ///     Adds a navigation property to include in the query using a string.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    /// <param name="query">The IQueryable to which the include is added.</param>
    /// <param name="include">The navigation property to include.</param>
    /// <returns>The IQueryable with added include.</returns>
    public static IQueryable<T> AddInclude<T>(this IQueryable<T> query, string? include) where T : class
    {
        return include == null ? query : query.Include(include);
    }

    /// <summary>
    ///     Applies pagination to the query based on page number and page size.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    /// <param name="query">The IQueryable to which pagination is applied.</param>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The page size.</param>
    /// <returns>The IQueryable with applied pagination.</returns>
    public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, int? page, int? pageSize)
        where T : class
    {
        if (page.HasValue && pageSize.HasValue)
            query = query.Skip(page.Value * pageSize.Value).Take(pageSize.Value);

        return query;
    }

    /// <summary>
    ///     Applies pagination to the query based on an ISpecification.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    /// <param name="query">The IQueryable to which pagination is applied.</param>
    /// <param name="specification">The specification containing pagination details.</param>
    /// <returns>The IQueryable with applied pagination.</returns>
    public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, ISpecification<T> specification)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(specification);

        return query.ApplyPagination(specification.Page, specification.PageSize);
    }

    /// <summary>
    ///     Adds an entity type configuration to the ModelBuilder.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <param name="modelBuilder">The ModelBuilder to which the configuration is added.</param>
    /// <param name="configuration">The entity type configuration.</param>
    public static void AddConfiguration<TEntity>(this ModelBuilder modelBuilder,
        IEntityTypeConfiguration<TEntity> configuration)
        where TEntity : class
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        ArgumentNullException.ThrowIfNull(configuration);

        configuration.Configure(modelBuilder.Entity<TEntity>());
    }

    /// <summary>
    ///     Adds a list of entity type configurations to the ModelBuilder.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <param name="modelBuilder">The ModelBuilder to which the configurations are added.</param>
    /// <param name="configurationList">The list of entity type configurations.</param>
    public static void AddConfiguration<TEntity>(this ModelBuilder modelBuilder,
        IList<IEntityTypeConfiguration<TEntity>> configurationList)
        where TEntity : class
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        ArgumentNullException.ThrowIfNull(configurationList);

        foreach (var item in configurationList) modelBuilder.AddConfiguration(item);
    }
}