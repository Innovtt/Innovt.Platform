using Innovt.Data.Model;
using Microsoft.Extensions.Configuration;

namespace Innovt.Data.DataSources
{
    public class WriteDataSource:DataSourceBase, IWriteDataSource
    {  
        public WriteDataSource(string name,string connectionString, Provider provider = Provider.MsSql) : base(name, connectionString, provider)
        {
        }

        public WriteDataSource(IConfiguration configuration,string connectionStringName, Provider provider = Provider.MsSql) : base(configuration, connectionStringName, provider)
        {
            Name = nameof(WriteDataSource);
        }

        public WriteDataSource(IConfiguration configuration, string name, string connectionStringName, Provider provider = Provider.MsSql) : base(configuration, name, connectionStringName, provider)
        { 
         
        }
    }
}