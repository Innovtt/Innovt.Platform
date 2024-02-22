// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito

using System.Text.Json.Serialization;
using Amazon.Lambda.S3Events;

namespace Innovt.Cloud.AWS.Lambda.S3.Serializers;

/// <summary>
///     Represents a JSON serializer context for S3 Serializer for AOT Support
/// </summary>
[JsonSerializable(typeof(S3Event))]
[JsonSerializable(typeof(BatchFailureResponse))]
public partial class S3EventJsonSerializerContext : JsonSerializerContext;