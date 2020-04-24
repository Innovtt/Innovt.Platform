using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Innovt.Core.Serialization
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class JsonFormatAttribute: JsonAttribute
    {
        private readonly string format;

        public JsonFormatAttribute(string format)
        {
            this.format = format;
        }
    }
}
