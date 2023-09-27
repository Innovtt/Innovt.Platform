// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain.Core

using System.Collections.Generic;
using System.Linq;

namespace Innovt.Domain.Core.Repository;
/// <summary>
/// Represents a class for managing entity inclusion paths for Entity Framework queries.
/// </summary>
public class Include
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Include"/> class.
    /// </summary>
    public Include()
    {
        Includes = new List<string>();
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="Include"/> class with specified include paths.
    /// </summary>
    /// <param name="includes">The include paths.</param>
    public Include(params string[] includes) : this()
    {
        Includes.AddRange(includes);
    }
    /// <summary>
    /// Gets the list of include paths.
    /// </summary>
    public List<string> Includes { get; }
    /// <summary>
    /// Checks if the include list is empty.
    /// </summary>
    /// <returns><c>true</c> if the include list is empty; otherwise, <c>false</c>.</returns>
    public bool IsEmpty()
    {
        return Includes == null || !Includes.Any();
    }
    /// <summary>
    /// Adds an include path.
    /// </summary>
    /// <param name="param">The include path to add.</param>
    /// <returns>The <see cref="Include"/> instance.</returns>
    public Include Add(string param)
    {
        Includes.Add(param);
        return this;
    }
    /// <summary>
    /// Adds multiple include paths.
    /// </summary>
    /// <param name="parameters">The include paths to add.</param>
    /// <returns>The <see cref="Include"/> instance.</returns>
    public Include Add(params string[] parameters)
    {
        Includes.AddRange(parameters);
        return this;
    }
    /// <summary>
    /// Creates a new instance of <see cref="Include"/> with specified include paths.
    /// </summary>
    /// <param name="parameters">The include paths.</param>
    /// <returns>The new <see cref="Include"/> instance.</returns>
    public static Include New(params string[] parameters)
    {
        return new Include(parameters);
    }
}