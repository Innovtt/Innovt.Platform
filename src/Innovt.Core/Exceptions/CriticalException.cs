// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Core
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using System.Runtime.Serialization;

namespace Innovt.Core.Exceptions
{
    [Serializable]
    public class CriticalException : BaseException
    {
        public CriticalException(string message) : base(message)
        {
        }

        public CriticalException(string message, Exception ex) : base(message, ex)
        {
        }

        public CriticalException(Exception ex) : base(ex)
        {
        }

        private CriticalException()
        {
        }

        protected CriticalException(SerializationInfo serializationInfo,
            StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}