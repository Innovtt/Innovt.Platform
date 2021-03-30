using System.Diagnostics.CodeAnalysis;
using Innovt.Data.Model;
using Microsoft.Extensions.Configuration;

namespace Innovt.Data.DataSources
{
    public class DataSourceReader : DataSourceBase, IDataSourceReader
    {
        public DataSourceReader([NotNull] string name, [NotNull] string connectionString,
            Provider provider = Provider.MsSql) : base(name, connectionString, provider)
        {
        }

        public DataSourceReader([NotNull] IConfiguration configuration, [NotNull] string connectionStringName,
            Provider provider = Provider.MsSql) : base(configuration, connectionStringName, provider)
        {
            Name = nameof(DataSourceReader);
        }

        public DataSourceReader(IConfiguration configuration, string name, string connectionStringName,
            Provider provider = Provider.MsSql) : base(configuration, name, connectionStringName, provider)
        {
        }
    }
}