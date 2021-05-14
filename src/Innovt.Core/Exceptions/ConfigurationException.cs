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
    public class ConfigurationException : BaseException
    {
        public ConfigurationException(string message) : base(message)
        {
        }

        public ConfigurationException(string message, Exception ex) : base(message, ex)
        {
        }

        public ConfigurationException(Exception ex) : base(ex)
        {
        }

        private ConfigurationException()
        {
        }

        protected ConfigurationException(System.Runtime.Serialization.SerializationInfo serializationInfo,
            System.Runtime.Serialization.StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}