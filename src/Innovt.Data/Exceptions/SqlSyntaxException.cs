﻿using System;
using Innovt.Core.Exceptions;

namespace Innovt.Data.Exceptions
{
    [Serializable]
    public class SqlSyntaxException : BaseException
    {
        public SqlSyntaxException(string message):base(message)
        {
            
        }
    }
}