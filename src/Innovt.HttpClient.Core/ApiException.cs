using System;

namespace Innovt.HttpClient.Core;

/// <summary>
///     Represents an exception specific to API operations, carrying an error response of type <typeparamref name="T" />.
/// </summary>
/// <typeparam name="T">The type of the error response associated with the exception.</typeparam>
public class ApiException<T> : Exception
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ApiException{T}" /> class with the specified error response.
    /// </summary>
    /// <param name="errorResponse">The error response associated with the exception.</param>
    public ApiException(T errorResponse)
    {
        ErrorResponse = errorResponse;
    }

    /// <summary>
    ///     Gets or sets the error response associated with the API exception.
    /// </summary>
    public T ErrorResponse { get; set; }

    public ApiException()
    {
    }
}