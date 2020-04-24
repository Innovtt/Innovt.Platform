// Company: INNOVT
// Project: Innovt.Common
// Created By: Michel Borges
// Date: 2016/10/19

using System;

namespace Innovt.Core.Exceptions
{
    public class BaseException : Exception
    {
        public BaseException()
            : base()
        {
        }

        public BaseException(string message)
            : this(message, null) { }

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