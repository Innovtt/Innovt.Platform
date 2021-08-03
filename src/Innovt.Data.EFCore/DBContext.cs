// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Data.EFCore
// Solution: Innovt.Platform
// Date: 2021-06-02
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
        private readonly IDataSource dataSource;
        private readonly ILoggerFactory loggerFactory;
        public int? MaxRetryCount { get; set; }
        public TimeSpan? MaxRetryDelay { get; set; }

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

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            return await SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public void Rollback()
        {
            base.ChangeTracker.Entries()
                .ToList()
                .ForEach(entry => entry.State = EntityState.Unchanged);
        }

        public new void Add<T>(T entity) where  T: class
        {
            base.Add(entity);
        }

        public void Add<T>(IEnumerable<T> entities) where T : class
        {            
            base.AddRange(entities);
        }

        public new async Task AddAsync<T>(T entity, CancellationToken cancellationToken = default) where T:class
        {
            await base.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        }
        
        public async Task AddAsync<T>(IEnumerable<T> entities,CancellationToken cancellationToken = default) where T : class
        {
            await base.AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
        }

        public new void Remove<T>(T entity) where T : class
        {
            base.Remove(entity);
        }

        public void Remove<T>(IEnumerable<T> entities) where T : class
        {
            base.RemoveRange(entities);
        }

        public new void Update<T>(T entity) where T : class
        {
            base.Update(entity);
        }

        public new void Attach<T>(T entity) where T : class
        {
            base.Attach(entity);
        }

        public void Detach<T>(T entity) where T : class
        {
            base.Entry(entity).State = EntityState.Detached;
        }

        public IQueryable<T> Queryable<T>() where T : class
        {
            return base.Set<T>();
        }

        public int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return base.Database.ExecuteSqlRaw(sql, parameters);
        }

        public async Task<int> ExecuteSqlCommandAsync(string sql, CancellationToken cancellationToken = default, params object[] parameters)
        {
            return await base.Database
                .ExecuteSqlRawAsync(sql, cancellationToken: cancellationToken, parameters: parameters)
                .ConfigureAwait(false);
        }


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
                    throw new ConnectionStringException(
                        $"Connection string for datasource {dataSource.Name} is empty.");

                switch (dataSource.Provider)
                {
                    case Provider.PostgreSqL:
                        optionsBuilder.UseNpgsql(connectionString, postOptions =>
                        {
                            if (MaxRetryCount != null && MaxRetryDelay != null)
                                postOptions.EnableRetryOnFailure(MaxRetryCount.GetValueOrDefault(),
                                    MaxRetryDelay.GetValueOrDefault(), null);
                        });
                        break;
                    case Provider.Oracle:
                        optionsBuilder.UseOracle(connectionString);
                        break;
                    default:
                        optionsBuilder.UseSqlServer(connectionString, sqlOptions =>
                        {
                            if (MaxRetryCount != null && MaxRetryDelay != null)
                                sqlOptions.EnableRetryOnFailure(MaxRetryCount.GetValueOrDefault(),
                                    MaxRetryDelay.GetValueOrDefault(), null
                                );
                        });
                        break;
                }
            }

            base.OnConfiguring(optionsBuilder);
        }
    }
}