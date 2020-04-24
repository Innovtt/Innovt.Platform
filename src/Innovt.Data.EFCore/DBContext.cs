using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Innovt.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Innovt.Core.Utilities;
using System.Threading;

namespace Innovt.Data.EFCore
{
    public class DbContext: Microsoft.EntityFrameworkCore.DbContext, IExtendedUnitOfWork
    {
        protected readonly string ConnectionString;
        
        public DbContext(IDataSource dataSource)
        {
            ConnectionString = dataSource?.GetConnectionString() ?? throw new ArgumentNullException(nameof(dataSource));
        }

        public DbContext(string connectionString)
        {
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public DbContext()
        {
        
        }

        public DbContext(DbContextOptions options):base(options)
        {
         
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!ConnectionString.IsNullOrEmpty())
                optionsBuilder.UseSqlServer(ConnectionString);
            
            base.OnConfiguring(optionsBuilder);
        }

        public int Commit()
        {
           return SaveChanges();
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            return await SaveChangesAsync(cancellationToken);
        }
        public void Rollback()
        {
            base.ChangeTracker.Entries()
                            .ToList()
                            .ForEach(entry => entry.State = EntityState.Unchanged);
            
        }

        void IExtendedUnitOfWork.Add<T>(T entity)
        {
            base.Add(entity);
        }
        void IExtendedUnitOfWork.Add<T>(IEnumerable<T> entities)
        {
            base.AddRange(entities);
        }

#pragma warning disable CS1066 // The default value specified will have no effect because it applies to a member that is used in contexts that do not allow optional arguments
        async Task IExtendedUnitOfWork.AddAsync<T>(T entity, CancellationToken cancellationToken = default)
#pragma warning restore CS1066 // The default value specified will have no effect because it applies to a member that is used in contexts that do not allow optional arguments
        {
            await base.AddAsync(entity, cancellationToken);
        }

#pragma warning disable CS1066 // The default value specified will have no effect because it applies to a member that is used in contexts that do not allow optional arguments
        async Task IExtendedUnitOfWork.AddAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default)
#pragma warning restore CS1066 // The default value specified will have no effect because it applies to a member that is used in contexts that do not allow optional arguments
        {
            await base.AddRangeAsync(entities, cancellationToken);
        }

        void IExtendedUnitOfWork.Remove<T>(T entity)
        {
            base.Remove(entity);
        }

        void IExtendedUnitOfWork.Remove<T>(IEnumerable<T> entities)
        {
            base.RemoveRange(entities);
        }

        void IExtendedUnitOfWork.Update<T>(T entity)
        {
            base.Update(entity);
        }

        void IExtendedUnitOfWork.Attach<T>(T item)
        {
            base.Attach(item);
        }

        void IExtendedUnitOfWork.Detach<T>(T item)
        {
            base.Entry(item).State = EntityState.Detached;
        }

        IQueryable<T> IExtendedUnitOfWork.Queryable<T>()
        {
            return base.Set<T>();
        }

        int IExtendedUnitOfWork.ExecuteSqlCommand(string sql, params object[] parameters)
        {
			return base.Database.ExecuteSqlRaw(sql, parameters);
        }

        async Task<int> IExtendedUnitOfWork.ExecuteSqlCommandAsync(string sql, CancellationToken cancellationToken,
        params object[] parameters)
        {
           return await base.Database.ExecuteSqlRawAsync(sql,cancellationToken: cancellationToken,parameters: parameters);
        }

    }
}
