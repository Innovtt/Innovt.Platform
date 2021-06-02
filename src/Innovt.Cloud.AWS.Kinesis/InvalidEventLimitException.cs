// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud.AWS.Kinesis
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Core.Exceptions;

namespace Innovt.Cloud.AWS.Kinesis
{
    internal class InvalidEventLimitException : BaseException
    {
        internal InvalidEventLimitException() : base(
            "Invalid event limit. Kinesis supports up to 500 messages per request.")
        {
        }
    }
}