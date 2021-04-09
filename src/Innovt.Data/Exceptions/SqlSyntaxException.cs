using Innovt.Core.Exceptions;
using System;

namespace Innovt.Data.Exceptions
{
    [Serializable]
    public class SqlSyntaxException : BaseException
    {
        public SqlSyntaxException(string message) : base(message)
        {
        }
    }
}