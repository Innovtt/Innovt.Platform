// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Data
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Core.Exceptions;
using System;

namespace Innovt.Data.Exceptions
{    
    [Serializable]
    public class ConnectionStringException : ConfigurationException
    {
        public ConnectionStringException(string message) : base(message)
        {
        }  
    }
}