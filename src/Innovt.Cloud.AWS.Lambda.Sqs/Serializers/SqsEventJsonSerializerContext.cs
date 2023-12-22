// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Amazon.Lambda.SQSEvents;

namespace Innovt.Cloud.AWS.Lambda.Sqs.Serializers;

/// <summary>
///     Represents a JSON serializer context for Sqs Serializer for AOT Support
/// </summary>
[JsonSerializable(typeof(SQSEvent))]
[JsonSerializable(typeof(BatchFailureResponse))]
public class SqsEventJsonSerializerContext (JsonSerializerOptions options) : JsonSerializerContext(options)
{
    
    private static readonly JsonSerializerOptions DefaultOptions = new();

    public SqsEventJsonSerializerContext() : this(null)
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