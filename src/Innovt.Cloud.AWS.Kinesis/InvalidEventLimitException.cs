// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud.AWS.Kinesis
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Core.Exceptions;

namespace Innovt.Cloud.AWS.Kinesis
{
#pragma warning disable CA1032 // Implement standard exception constructors
    internal class InvalidEventLimitException : BaseException
#pragma warning restore CA1032 // Implement standard exception constructors
    {
        internal InvalidEventLimitException() : base(
            "Invalid event limit. Kinesis supports up to 500 messages per request.")
        {
        }
    }
}