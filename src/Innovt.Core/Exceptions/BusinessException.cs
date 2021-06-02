// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Core
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

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
        public BusinessException(string message) : base(message)
        {
        }

        public BusinessException(string message, Exception ex) : base(message, ex)
        {
        }


        public BusinessException(string code, string message) : base(message)
        {
            Code = code;
        }

        public BusinessException(string code, string message, Exception ex) : base(message, ex)
        {
            Code = code;
        }

        public BusinessException(IList<ErrorMessage> errors) : base(CreateMessage(errors))
        {
            Errors = errors;
        }

        public BusinessException(ErrorMessage[] errors) : base(CreateMessage(errors))
        {
            Errors = errors;
        }

        public BusinessException(ErrorMessage error) : this(new[] {error})
        {
        }

        public string Code { get; protected set; }
        public IEnumerable<ErrorMessage> Errors { get; set; }

        private static string CreateMessage(IEnumerable<ErrorMessage> errors)
        {
            var errorMessages = errors as ErrorMessage[] ?? errors.ToArray();

            if (!errorMessages.Any()) return string.Empty;

            var strError = new StringBuilder();
            strError.Append("[");
            foreach (var errorInfo in errorMessages)
                if (errorInfo.Message.IsNotNullOrEmpty())
                    strError.Append("{" +
                                    $"\"Message\":\"{errorInfo.Message}\",\"Code\":\"{errorInfo.Code}\",\"PropertyName\":\"{errorInfo.PropertyName}\"" +
                                    "}");

            strError.Append("]");

            return strError.ToString();
        }
    }
}