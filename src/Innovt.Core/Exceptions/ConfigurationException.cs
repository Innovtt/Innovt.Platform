// Company: INNOVT
// Project: Innovt.Common
// Created By: Michel Borges
// Date: 2016/10/19

using System;

namespace Innovt.Core.Exceptions
{
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
    }
}