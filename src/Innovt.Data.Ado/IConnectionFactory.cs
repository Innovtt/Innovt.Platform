// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Data.Ado
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System.Data;
using Innovt.Data.DataSources;

namespace Innovt.Data.Ado;

public interface IConnectionFactory
{
    IDbConnection Create(IDataSource dataSource);
}