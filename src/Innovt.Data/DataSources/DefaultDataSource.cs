using Innovt.Data.Model;
using Microsoft.Extensions.Configuration;

namespace Innovt.Data.DataSources
{
    public class DefaultDataSource : DataSourceBase
    {
        public DefaultDataSource(string connectionStringName, Provider provider = Provider.MsSql) : base(connectionStringName, provider)
        {
            Name = nameof(DefaultDataSource);
        }

        public DefaultDataSource(IConfiguration configuration, string connectionStringName, Provider provider = Provider.MsSql) : base(configuration, connectionStringName, provider)
        {
            
            Name = nameof(DefaultDataSource);
        }
    }
}