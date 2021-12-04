using Innovt.Data.DataSources;
using Microsoft.EntityFrameworkCore;

namespace Innovt.Data.EFCore.Oracle
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
            optionsBuilder.UseOracle(connectionString);
        }
    }
}