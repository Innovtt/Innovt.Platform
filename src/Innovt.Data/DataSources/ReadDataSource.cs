using System.Diagnostics.CodeAnalysis;
using Innovt.Data.Model;
using Microsoft.Extensions.Configuration;

namespace Innovt.Data.DataSources
{
    public class ReadDataSource: DataSourceBase, IReadDataSource
    { 
        public ReadDataSource([NotNull] string name, [NotNull] string connectionString, Provider provider = Provider.MsSql) : base(name, connectionString, provider)
        {
        }

        public ReadDataSource([NotNull] IConfiguration configuration, [NotNull] string connectionStringName, Provider provider = Provider.MsSql) : base(configuration, connectionStringName, provider)
        {     Name = nameof(ReadDataSource);
        }

        public ReadDataSource(IConfiguration configuration, string name, string connectionStringName, Provider provider = Provider.MsSql) : base(configuration, name, connectionStringName, provider)
        { 
         
        }
    }
}