// Company: INNOVT
// Project: Innovt.Common
// Created By: Michel Borges
// Date: 2016/10/19

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Innovt.Core.Utilities;

namespace Innovt.Core.Exceptions
{
    [Serializable]
    public class BusinessException : BaseException
    {
        public string Code { get; protected set; }
        public IEnumerable<ErrorMessage> Errors { get; set; }

        public BusinessException(string message) : base(message)
        {
        }

        public BusinessException(string message, Exception ex) : base(message, ex)
        {
        }


        public BusinessException(string code, string message) : base(message)
        {
            this.Code = code;
        }

        public BusinessException(string code, string message, Exception ex) : base(message, ex)
        {
            this.Code = code;
        }

        public BusinessException(IList<ErrorMessage> errors) : base(CreateMessage(errors))
        {
            this.Errors = errors;
        }

        public BusinessException(ErrorMessage[] errors) : base(CreateMessage(errors))
        {
            this.Errors = errors;
        }

        public BusinessException(ErrorMessage error) : this(new[] {error})
        {
        }

        private static string CreateMessage(IEnumerable<ErrorMessage> errors)
        {
            var errorMessages = errors as ErrorMessage[] ?? errors.ToArray();

            if (!errorMessages.Any()) return string.Empty;

            var strError = new StringBuilder();
            strError.Append("[");
            foreach (var errorInfo in errorMessages)
            {
                if (errorInfo.Message.IsNotNullOrEmpty())
                {
                    strError.Append("{" +
                                    $"\"Message\":\"{errorInfo.Message}\",\"Code\":\"{errorInfo.Code}\",\"PropertyName\":\"{errorInfo.PropertyName}\"" +
                                    "}");
                }
            }

            strError.Append("]");

            return strError.ToString();
        }
    }
}