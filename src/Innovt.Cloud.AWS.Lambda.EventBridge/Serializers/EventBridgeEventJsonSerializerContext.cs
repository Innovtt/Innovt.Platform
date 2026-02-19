// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.EventBridge

using System.Text.Json;
using System.Text.Json.Serialization;
using Amazon.Lambda.CloudWatchEvents;

namespace Innovt.Cloud.AWS.Lambda.EventBridge.Serializers;

[JsonSerializable(typeof(CloudWatchEvent<JsonElement>))]
public partial class EventBridgeEventJsonSerializerContext : JsonSerializerContext
{
}
