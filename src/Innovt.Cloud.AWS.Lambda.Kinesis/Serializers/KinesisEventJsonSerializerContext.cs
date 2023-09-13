// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito

using System.Text.Json.Serialization;
using Amazon.Lambda.KinesisEvents;

namespace Innovt.Cloud.AWS.Lambda.Kinesis.Serializers;

[JsonSerializable(typeof(KinesisEvent))]
public partial class KinesisEventJsonSerializerContext : JsonSerializerContext
{
    
}