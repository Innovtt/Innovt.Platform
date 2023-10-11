// Innovt Company
// Author: Michel Borges
// Project: Innovt.Data

using System;
using Innovt.Core.Exceptions;

namespace Innovt.Data.Exceptions;

/// <summary>
/// Represents an exception thrown for SQL syntax related errors.
/// </summary>
[Serializable]
public class SqlSyntaxException : BaseException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SqlSyntaxException"/> class with the specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public SqlSyntaxException(string message) : base(message)
    {
    }
}