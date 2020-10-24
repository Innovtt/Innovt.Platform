using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Innovt.Core.Serialization
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class JsonFormatAttribute: JsonAttribute
    {
        public string Format { get; }

        public JsonFormatAttribute(string format)
        {
            this.Format = format;
        }
    }
}
