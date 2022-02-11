// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.AspNetCore
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

namespace Innovt.AspNetCore.Model
{
    public class ResponseError
    {
        public string TraceId { get; set; }

        public string Message { get; set; }

        public string Code { get; set; }

        public object Detail { get; set; }
    }
}