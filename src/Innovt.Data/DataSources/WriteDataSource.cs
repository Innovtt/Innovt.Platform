using Innovt.Data.Model;
using Microsoft.Extensions.Configuration;

namespace Innovt.Data.DataSources
{
    public class WriteDataSource:DataSourceBase, IWriteDataSource
    {
        //const  string WriteDataSourceName = ""
        public WriteDataSource(string connectionStringName, Provider provider = Provider.MsSql) : base(connectionStringName, provider)
        {
            Name = nameof(WriteDataSource);
        }

        public WriteDataSource(IConfiguration configuration, string connectionStringName, Provider provider = Provider.MsSql) : base(configuration, connectionStringName, provider)
        {
            Name = nameof(WriteDataSource);
        }
    }
}