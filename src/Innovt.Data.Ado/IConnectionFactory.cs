// Innovt Company
// Author: Michel Borges
// Project: Innovt.Data.Ado

using System.Data;
using Innovt.Data.DataSources;

namespace Innovt.Data.Ado;

public interface IConnectionFactory
{
    IDbConnection Create(IDataSource dataSource);
}