// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Data
// Solution: Innovt.Platform
// Date: 2021-04-08
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Data.Model;
using Microsoft.Extensions.Configuration;

namespace Innovt.Data.DataSources
{
    public class DataSourceWriter : DataSourceBase, IDataSourceWriter
    {
        public DataSourceWriter(string name, string connectionString, Provider provider = Provider.MsSql) : base(name,
            connectionString, provider)
        {
        }

        public DataSourceWriter(IConfiguration configuration, string connectionStringName,
            Provider provider = Provider.MsSql) : base(configuration, connectionStringName, provider)
        {
            Name = nameof(DataSourceWriter);
        }

        public DataSourceWriter(IConfiguration configuration, string name, string connectionStringName,
            Provider provider = Provider.MsSql) : base(configuration, name, connectionStringName, provider)
        {
        }
    }
}