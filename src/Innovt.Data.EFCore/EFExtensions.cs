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

public static class EfExtensions
{
    public static IQueryable<T> AddInclude<T>(this IQueryable<T> query, Include? includes) where T : class
    {
        return includes == null || includes.IsEmpty()
            ? query
            : includes.Includes.Aggregate(query, (current, include) => current.Include(include));
    }

    public static IQueryable<T> AddInclude<T>(this IQueryable<T> query, params string[] includes) where T : class
    {
        return includes == null ? query : includes.Aggregate(query, (current, include) => current.Include(include));
    }

    public static IQueryable<T> AddInclude<T>(this IQueryable<T> query, string include) where T : class
    {
        if (include == null)
            return query;

        return query.Include(include);
    }

    public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, int? page, int? pageSize)
        where T : class
    {
        if (page.HasValue && pageSize.HasValue)
            query = query.Skip(page.Value * pageSize.Value).Take(pageSize.Value);

        return query;
    }

    public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, ISpecification<T> specification)
        where T : class
    {
        if (specification == null) throw new ArgumentNullException(nameof(specification));

        return query.ApplyPagination(specification.Page, specification.PageSize);
    }

    public static void AddConfiguration<TEntity>(this ModelBuilder modelBuilder,
        IEntityTypeConfiguration<TEntity> configuration)
        where TEntity : class
    {
        if (modelBuilder == null) throw new ArgumentNullException(nameof(modelBuilder));
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        configuration.Configure(modelBuilder.Entity<TEntity>());
    }

    public static void AddConfiguration<TEntity>(this ModelBuilder modelBuilder,
        IList<IEntityTypeConfiguration<TEntity>> configurationList)
        where TEntity : class
    {
        if (modelBuilder == null) throw new ArgumentNullException(nameof(modelBuilder));
        if (configurationList == null) throw new ArgumentNullException(nameof(configurationList));

        foreach (var item in configurationList) modelBuilder.AddConfiguration(item);
    }
}