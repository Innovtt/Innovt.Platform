// Company: INNOVT
// Project: Innovt.Common
// Created By: Michel Borges
// Date: 2016/10/19

using System;

namespace Innovt.Core.Exceptions
{
    public class CriticalException : BaseException
    {
        public CriticalException(string message):base(message)
        {
            
        }

        public CriticalException(string message,Exception ex) : base(message,ex)
        {

        }

        public CriticalException(Exception ex) : base(ex)
        {

        }
    }
}