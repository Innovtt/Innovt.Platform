using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Innovt.Core.Serialization;

public class JsonSerializerContextBase(JsonSerializerOptions options) : JsonSerializerContext(options)
{
    private static readonly JsonSerializerOptions DefaultOptions = new();

    public JsonSerializerContextBase() : this(null)
    {
    }

    /// <summary>
    ///     Returns the <see cref="T:System.Text.Json.Serialization.JsonTypeInfo" /> for the specified type.
    /// </summary>
    protected override JsonSerializerOptions GeneratedSerializerOptions => DefaultOptions;

    /// <summary>
    ///     Get a <see cref="JsonTypeInfo" /> for the specified type.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public override JsonTypeInfo GetTypeInfo(Type type)
    {
        return Options.GetTypeInfo(type);
    }
}