// Innovt Company
// Author: Michel Borges
// Project: Innovt.Data

using System;
using Innovt.Core.Exceptions;

namespace Innovt.Data.Exceptions;
/// <summary>
/// Represents an exception thrown for connection string related errors.
/// </summary>
[Serializable]
public class ConnectionStringException : ConfigurationException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConnectionStringException"/> class with the specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public ConnectionStringException(string message) : base(message)
    {
    }
}