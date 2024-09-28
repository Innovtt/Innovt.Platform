// Innovt Company
// Author: Michel Borges
// Project: Innovt.Data.Ado

using System;
using System.Data;
using Innovt.Data.DataSources;
using Innovt.Data.Exceptions;

namespace Innovt.Data.Ado;

/// <summary>
///     Represents an interface for a factory that creates database connections based on a data source.
/// </summary>
public interface IConnectionFactory
{
    /// <summary>
    ///     Creates a new instance of <see cref="IDbConnection" /> based on the provided data source.
    /// </summary>
    /// <param name="dataSource">The data source for which a connection will be created.</param>
    /// <returns>A new instance of <see cref="IDbConnection" /> based on the data source.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataSource" /> is null.</exception>
    /// <exception cref="ConnectionStringException">Thrown when the connection string is null or empty.</exception>
    IDbConnection Create(IDataSource dataSource);
}