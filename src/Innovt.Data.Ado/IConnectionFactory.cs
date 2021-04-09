using Innovt.Data.DataSources;
using System.Data;

namespace Innovt.Data.Ado
{
    public interface IConnectionFactory
    {
        IDbConnection Create(IDataSource dataSource);
    }
}