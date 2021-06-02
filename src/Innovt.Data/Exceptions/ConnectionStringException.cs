// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Data
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Core.Exceptions;

namespace Innovt.Data.Exceptions
{
    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell",
    //    "S3925:\"ISerializable\" should be implemented correctly", Justification = "<Pending>")]
    public class ConnectionStringException : ConfigurationException
    {
        public ConnectionStringException(string message) : base(message)
        {
        }
    }
}