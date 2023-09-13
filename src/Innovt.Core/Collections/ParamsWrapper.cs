// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

namespace Innovt.Core.Collections;

/// <summary>
/// Represents a wrapper for an array of parameters of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of parameters in the array.</typeparam>
/// <remarks>
/// This class allows you to wrap an array of parameters and provide a convenient way to work with them.
/// It is commonly used when you need to pass a variable number of parameters to a method or function.
/// </remarks>
public class ParamsWrappers<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ParamsWrappers{T}"/> class with the specified parameters.
    /// </summary>
    /// <param name="parameters">The array of parameters to wrap.</param>
    public ParamsWrappers(params T[] parameters)
    {
        Parameters = parameters;
    }

    /// <summary>
    /// Gets or sets the array of parameters wrapped by this instance.
    /// </summary>
    public T[] Parameters { get; set; }
}