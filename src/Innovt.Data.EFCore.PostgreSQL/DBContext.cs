using Innovt.Data.DataSources;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Innovt.Data.EFCore.PostgreSQL
{
    public class DBContext : DbContext
    {
        public DBContext(IDataSource dataSource) : base(dataSource)
        {
        }

        public override int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return base.Database.ExecuteSqlRaw(sql, parameters);
        }

        public override Task<int> ExecuteSqlCommandAsync(string sql, CancellationToken cancellationToken = default, params object[] parameters)
        {
            return base.Database
                .ExecuteSqlRawAsync(sql, cancellationToken: cancellationToken, parameters: parameters);

        }

        protected override void ConfigureProvider(DbContextOptionsBuilder optionsBuilder, string connectionString)
        {
            optionsBuilder.UseNpgsql(connectionString, postOptions =>
            {
                if (MaxRetryCount != null && MaxRetryDelay != null)
                    postOptions.EnableRetryOnFailure(MaxRetryCount.GetValueOrDefault(),
                        MaxRetryDelay.GetValueOrDefault(), null);
            });

        }
    }
}