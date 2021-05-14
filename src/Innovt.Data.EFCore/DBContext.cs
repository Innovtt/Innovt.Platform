// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Data.EFCore
// Solution: Innovt.Platform
// Date: 2021-05-03
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Core.Utilities;
using Innovt.Data.DataSources;
using Innovt.Data.Exceptions;
using Innovt.Data.Model;
using Innovt.Domain.Core.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Innovt.Data.EFCore
{
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext, IExtendedUnitOfWork
    {
        public int? MaxRetryCount { get; set; }
        public TimeSpan? MaxRetryDelay { get; set; }

        private readonly IDataSource dataSource;
        private readonly ILoggerFactory loggerFactory;
        
        public DbContext(IDataSource dataSource)
        {
            this.dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
            base.ChangeTracker.LazyLoadingEnabled = false;
        }

        public DbContext(IDataSource dataSource, ILoggerFactory loggerFactory) : this(dataSource)
        {
            this.loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public DbContext(DbContextOptions options) : base(options)
        {
            base.ChangeTracker.LazyLoadingEnabled = false;
        }

        public int Commit()
        {
            return SaveChanges();
        }

        public Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            return SaveChangesAsync(cancellationToken);
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
            await base.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        }

#pragma warning disable CS1066 // The default value specified will have no effect because it applies to a member that is used in contexts that do not allow optional arguments
        async Task IExtendedUnitOfWork.AddAsync<T>(IEnumerable<T> entities,
            CancellationToken cancellationToken = default)
#pragma warning restore CS1066 // The default value specified will have no effect because it applies to a member that is used in contexts that do not allow optional arguments
        {
            await base.AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
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

        void IExtendedUnitOfWork.Attach<T>(T entity)
        {
            base.Attach(entity);
        }

        void IExtendedUnitOfWork.Detach<T>(T entity)
        {
            base.Entry(entity).State = EntityState.Detached;
        }

        IQueryable<T> IExtendedUnitOfWork.Queryable<T>()
        {
            return base.Set<T>();
        }

        int IExtendedUnitOfWork.ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return base.Database.ExecuteSqlRaw(sql, parameters);
        }

        Task<int> IExtendedUnitOfWork.ExecuteSqlCommandAsync(string sql, CancellationToken cancellationToken,
            params object[] parameters)
        {
            return base.Database.ExecuteSqlRawAsync(sql, cancellationToken: cancellationToken,
                parameters: parameters);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging(false);
            optionsBuilder.EnableDetailedErrors();

            if (loggerFactory != null) optionsBuilder.UseLoggerFactory(loggerFactory);

            if (dataSource != null)
            {
                var connectionString = dataSource.GetConnectionString();

                if (connectionString.IsNullOrEmpty())
                    throw new ConnectionStringException(
                        $"Connection string for datasource {dataSource.Name} is empty.");
                
                switch (dataSource.Provider)
                {
                    case Provider.PostgreSqL:
                        optionsBuilder.UseNpgsql(connectionString, postOptions =>
                        {
                            if (MaxRetryCount != null && MaxRetryDelay != null)
                            {
                                postOptions.EnableRetryOnFailure(MaxRetryCount.GetValueOrDefault(),
                                    MaxRetryDelay.GetValueOrDefault(), null);
                            }
                        });
                        break;
                    case Provider.Oracle:
                        optionsBuilder.UseOracle(connectionString);
                        break;
                    default:
                        optionsBuilder.UseSqlServer(connectionString, sqlOptions =>
                        {
                            if (MaxRetryCount != null && MaxRetryDelay != null)
                            {
                                sqlOptions.EnableRetryOnFailure(MaxRetryCount.GetValueOrDefault(),
                                    MaxRetryDelay.GetValueOrDefault(), null
                                );
                            }
                        });
                break;
                }
            }

            base.OnConfiguring(optionsBuilder);
        }
    }
}