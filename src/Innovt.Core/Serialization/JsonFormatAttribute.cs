// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;
using System.Text.Json.Serialization;

namespace Innovt.Core.Serialization;

/// <summary>
/// Represents a custom JSON attribute for specifying the format of a property when serializing to JSON.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class JsonFormatAttribute : JsonAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="JsonFormatAttribute"/> class with the specified format.
    /// </summary>
    /// <param name="format">The format string used to specify how the property should be formatted when serialized to JSON.</param>
    public JsonFormatAttribute(string format)
    {
        Format = format;
    }

    /// <summary>
    /// Gets the format string that specifies how the property should be formatted when serialized to JSON.
    /// </summary>
    public string Format { get; }
}