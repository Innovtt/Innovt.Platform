using Innovt.Data.Model;
using Microsoft.Extensions.Configuration;

namespace Innovt.Data.DataSources
{
    public class ReadDataSource: DataSourceBase, IReadDataSource
    {
        public ReadDataSource(string connectionStringName, Provider provider = Provider.MsSql) : base(connectionStringName, provider)
        {
            Name = nameof(ReadDataSource);
        }

        public ReadDataSource(IConfiguration configuration, string connectionStringName, Provider provider = Provider.MsSql) : base(configuration, connectionStringName, provider)
        {
            Name = nameof(ReadDataSource);
        }
    }
}