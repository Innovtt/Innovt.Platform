// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.EventBridge

using System.Text.Json.Serialization;
using Innovt.Cloud.AWS.Lambda.EventBridge.Events;

namespace Innovt.Cloud.AWS.Lambda.EventBridge.Serializers;

[JsonSerializable(typeof(EventBridgeMessage))]
public partial class EventBridgeEventJsonSerializerContext : JsonSerializerContext
{
}
