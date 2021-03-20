using System;
using Innovt.Core.Exceptions;

namespace Innovt.Data.Exceptions
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell",
        "S3925:\"ISerializable\" should be implemented correctly", Justification = "<Pending>")]
    public class ConnectionStringException : ConfigurationException
    {
        public ConnectionStringException(string message) : base(message)
        {
        }
    }
}