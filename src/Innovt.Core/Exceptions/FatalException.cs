// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Core
// Solution: Innovt.Platform
// Date: 2021-04-08
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using System.Diagnostics.CodeAnalysis;

namespace Innovt.Core.Exceptions
{
    [Serializable]
    [SuppressMessage("Major Code Smell",
        "S3925:\"ISerializable\" should be implemented correctly", Justification = "<Pending>")]
    public class FatalException : BaseException
    {
        public FatalException(string message) : base(message)
        {
        }

        public FatalException(string message, Exception ex) : base(message, ex)
        {
        }

        public FatalException(Exception ex) : base(ex)
        {
        }
    }
}