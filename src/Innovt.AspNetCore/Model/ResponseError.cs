using Innovt.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Innovt.AspNetCore.Model
{
   public class ResponseError
    {

        public string Message { get; set; }

        public string Code { get; set; }

        public IEnumerable<ErrorMessage> Errors { get; set; }


    }
}
