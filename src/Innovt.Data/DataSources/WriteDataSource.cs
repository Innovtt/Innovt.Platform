using Innovt.Data.Model;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;

namespace Innovt.Data.DataSources
{
    public class WriteDataSource:DataSourceBase, IWriteDataSource
    {  
        public WriteDataSource([NotNull] string name, [NotNull] string connectionString, Provider provider = Provider.MsSql) : base(name, connectionString, provider)
        {
        }

        public WriteDataSource([NotNull] IConfiguration configuration, [NotNull] string connectionStringName, Provider provider = Provider.MsSql) : base(configuration, connectionStringName, provider)
        {
            Name = nameof(WriteDataSource);
        }

        public WriteDataSource(IConfiguration configuration, string name, string connectionStringName, Provider provider = Provider.MsSql) : base(configuration, name, connectionStringName, provider)
        { 
         
        }
    }
}