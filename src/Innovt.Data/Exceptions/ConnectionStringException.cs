// Innovt Company
// Author: Michel Borges
// Project: Innovt.Data

using System;
using Innovt.Core.Exceptions;

namespace Innovt.Data.Exceptions;

[Serializable]
public class ConnectionStringException : ConfigurationException
{
    public ConnectionStringException(string message) : base(message)
    {
    }
}