// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito

using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Lambda.Serializers;

/// <summary>
/// Represents a JSON serializer context for BatchFailureResponse Serializer for AOT Support
/// </summary>
[JsonSerializable(typeof(BatchFailureResponse))]
public partial class BatchFailureResponseJsonSerializerContext : JsonSerializerContext
{
    
}