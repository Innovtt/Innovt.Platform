using System.Threading;
using System.Threading.Tasks;
using Innovt.Data.DataSources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Innovt.Data.EFCore.MsSql;

public class DBContext : DBContextBase
{
    public DBContext(IDataSource dataSource) : base(dataSource)
    {
    }

    protected DBContext(IDataSource dataSource, ILoggerFactory loggerFactory) : base(dataSource, loggerFactory)
    {
        
    }

    protected DBContext(DbContextOptions options) : base(options)
    {
    }

    public override int ExecuteSqlCommand(string sql, params object[] parameters)
    {
        return base.Database.ExecuteSqlRaw(sql, parameters);
    }

    public override Task<int> ExecuteSqlCommandAsync(string sql, CancellationToken cancellationToken = default,
        params object[] parameters)
    {
        return base.Database
            .ExecuteSqlRawAsync(sql, cancellationToken: cancellationToken, parameters: parameters);
    }

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