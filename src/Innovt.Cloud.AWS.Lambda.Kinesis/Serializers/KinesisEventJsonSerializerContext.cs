// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito

using System.Text.Json.Serialization;
using Amazon.Lambda.KinesisEvents;

namespace Innovt.Cloud.AWS.Lambda.Kinesis.Serializers;

/// <summary>
/// Represents a JSON serializer context for Kinesis events.
/// </summary>
[JsonSerializable(typeof(KinesisEvent))]
[JsonSerializable(typeof(BatchFailureResponse))]
public partial class KinesisEventJsonSerializerContext : JsonSerializerContext
{
    
}