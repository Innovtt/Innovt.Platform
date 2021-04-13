// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Core
// Solution: Innovt.Platform
// Date: 2021-04-08
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using System.Text.Json.Serialization;

namespace Innovt.Core.Serialization
{
    [AttributeUsage(AttributeTargets.Property)]
    public class JsonFormatAttribute : JsonAttribute
    {
        public JsonFormatAttribute(string format)
        {
            Format = format;
        }

        public string Format { get; }
    }
}