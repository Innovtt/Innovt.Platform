// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Data
// Solution: Innovt.Platform
// Date: 2021-05-03
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Data.Model;

namespace Innovt.Data.DataSources
{
    public interface IDataSource
    {
        string Name { get; set; }

        Provider Provider { get; }

        string GetConnectionString();
    }
}