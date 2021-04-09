using System;
using System.Text.Json.Serialization;

namespace Innovt.Core.Serialization
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class JsonFormatAttribute : JsonAttribute
    {
        public string Format { get; }

        public JsonFormatAttribute(string format)
        {
            Format = format;
        }
    }
}