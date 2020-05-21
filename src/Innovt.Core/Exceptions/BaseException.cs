// Company: INNOVT
// Project: Innovt.Common
// Created By: Michel Borges
// Date: 2016/10/19

using System;

namespace Innovt.Core.Exceptions
{
    [Serializable]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "<Pending>")]
    public class BaseException : Exception
    {
        public BaseException(): base()
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