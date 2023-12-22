// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Amazon.Lambda.S3Events;

namespace Innovt.Cloud.AWS.Lambda.S3.Serializers;

/// <summary>
///     Represents a JSON serializer context for S3 Serializer for AOT Support
/// </summary>
[JsonSerializable(typeof(S3Event))]
[JsonSerializable(typeof(BatchFailureResponse))]
public class S3EventJsonSerializerContext (JsonSerializerOptions options) : JsonSerializerContext(options)
{
    
    private static readonly JsonSerializerOptions DefaultOptions = new();

    public S3EventJsonSerializerContext() : this(null)
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