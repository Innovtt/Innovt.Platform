// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Data
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using Innovt.Core.Exceptions;

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