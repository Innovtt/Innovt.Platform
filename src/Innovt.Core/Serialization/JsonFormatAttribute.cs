// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;
using System.Text.Json.Serialization;

namespace Innovt.Core.Serialization;

[AttributeUsage(AttributeTargets.Property)]
public class JsonFormatAttribute : JsonAttribute
{
    public JsonFormatAttribute(string format)
    {
        Format = format;
    }

    public string Format { get; }
}