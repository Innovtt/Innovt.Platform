// Solution: Innovt.Platform
// Project: Innovt.Core
// User: Michel Magalhães
// Date: 2020/02/17 at 11:48 PM

using System;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Innovt.Core.Serialization.Converters
{
    public class DateTimeConverter:JsonConverter<DateTime>
    {
        private readonly string format;

        public DateTimeConverter(string format)
        {
            this.format = format;
        }

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Debug.Assert(typeToConvert == typeof(DateTime));

            return DateTime.Parse(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {

            writer.WriteStringValue(value.ToString(format));
        }
    }
}