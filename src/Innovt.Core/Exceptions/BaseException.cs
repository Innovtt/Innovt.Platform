// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Core
// Solution: Innovt.Platform
// Date: 2021-05-03
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using System.Diagnostics.CodeAnalysis;

namespace Innovt.Core.Exceptions
{
    [Serializable]
    [SuppressMessage("Major Code Smell",
        "S3925:\"ISerializable\" should be implemented correctly", Justification = "<Pending>")]
    public class BaseException : Exception
    {
        public BaseException()
        {
        }

        public BaseException(string message)
            : this(message, null)
        {
        }

        public BaseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public BaseException(Exception innerException)
            : base(null, innerException)
        {
        }
    }
}