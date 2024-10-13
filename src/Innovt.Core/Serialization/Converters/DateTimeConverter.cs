// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Innovt.Core.Serialization.Converters;

/// <summary>
///     Custom JSON converter for <see cref="DateTime" /> objects with a specified format.
/// </summary>
public class DateTimeConverter : JsonConverter<DateTime>
{
    private readonly string format;

    /// <summary>
    ///     Initializes a new instance of the <see cref="DateTimeConverter" /> class with the specified format.
    /// </summary>
    /// <param name="format">The format string to be used for formatting and parsing <see cref="DateTime" /> objects.</param>
    public DateTimeConverter(string format)
    {
        this.format = format;
    }

    /// <summary>
    ///     Reads and converts a JSON value to a <see cref="DateTime" /> object.
    /// </summary>
    /// <param name="reader">The JSON reader to read from.</param>
    /// <param name="typeToConvert">The type to convert (should be <see cref="DateTime" />).</param>
    /// <param name="options">The JSON serializer options.</param>
    /// <returns>A <see cref="DateTime" /> object parsed from the JSON value.</returns>
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        Debug.Assert(typeToConvert == typeof(DateTime));

        return DateTime.Parse(reader.GetString(),CultureInfo.InvariantCulture);
    }

    /// <summary>
    ///     Writes a <see cref="DateTime" /> value as a JSON string using the specified format.
    /// </summary>
    /// <param name="writer">The JSON writer to write to.</param>
    /// <param name="value">The <see cref="DateTime" /> value to be written.</param>
    /// <param name="options">The JSON serializer options.</param>
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(format));
    }
}