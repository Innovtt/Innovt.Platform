// Innovt Company
// Author: Michel Borges
// Project: Innovt.Data

using Innovt.Data.Model;

namespace Innovt.Data.DataSources;
/// <summary>
/// Represents an interface for a data source, providing information about the data source.
/// </summary>
public interface IDataSource
{
    /// <summary>
    /// Gets or sets the name of the data source.
    /// </summary>
    string Name { get; set; }
    /// <summary>
    /// Gets the provider for the data source.
    /// </summary>
    Provider Provider { get; }
    /// <summary>
    /// Retrieves the connection string for the data source.
    /// </summary>
    /// <returns>The connection string for the data source.</returns>
    string GetConnectionString();
}