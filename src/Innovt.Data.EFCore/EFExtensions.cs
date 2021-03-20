using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Collections.Generic;
using System;
using Innovt.Data.EFCore.Maps;
using Innovt.Domain.Core.Repository;
using Innovt.Domain.Core.Specification;
using Innovt.Domain.Security;

namespace Innovt.Data.EFCore
{
    public static class EfExtensions
    {
        public static IQueryable<T> AddInclude<T>(this IQueryable<T> query, Include includes) where T : class
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
            return query.ApplyPagination(specification.Page, specification.PageSize);
        }

        public static void AddConfiguration<TEntity>(this ModelBuilder modelBuilder,
            IEntityTypeConfiguration<TEntity> configuration)
            where TEntity : class
        {
            configuration.Configure(modelBuilder.Entity<TEntity>());
        }

        public static void AddConfiguration<TEntity>(this ModelBuilder modelBuilder,
            List<IEntityTypeConfiguration<TEntity>> configurationList)
            where TEntity : class
        {
            if (configurationList == null)
                throw new System.ArgumentNullException(nameof(configurationList));

            foreach (var item in configurationList)
            {
                modelBuilder.AddConfiguration(item);
            }
        }

        public static void AddSecurityMap(this ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<Policy>(new PolicyMap());
            modelBuilder.ApplyConfiguration<Permission>(new PermissionMap());
            modelBuilder.ApplyConfiguration<SecurityGroup>(new SecurityGroupMap());
            modelBuilder.ApplyConfiguration<PolicyPermission>(new PolicyPermissionMap());
            modelBuilder.ApplyConfiguration<SecurityGroupPolicy>(new SecurityGroupPolicyMap());
            modelBuilder.ApplyConfiguration<SecurityGroupUser>(new SecurityGroupUserMap());
        }
    }
}