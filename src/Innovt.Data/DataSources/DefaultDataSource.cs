using Innovt.Data.Model;
using Microsoft.Extensions.Configuration;

namespace Innovt.Data.DataSources
{
    public class DefaultDataSource : DataSourceBase
    {
        public DefaultDataSource( string name,string connectionString, Provider provider = Provider.MsSql) : base(name, connectionString, provider)
        {
        }

        public DefaultDataSource(IConfiguration configuration, string connectionStringName, Provider provider = Provider.MsSql) : base(configuration, connectionStringName, provider)
        {
            Name = nameof(DefaultDataSource);
        }

        public DefaultDataSource(IConfiguration configuration, string name, string connectionStringName, Provider provider = Provider.MsSql) : base(configuration, name, connectionStringName, provider)
        {
        }
    }
}