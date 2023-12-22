// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito

using System.Text.Json.Serialization;
using Innovt.Cloud.AWS.Lambda.Cognito.Events.PostConfirmation;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Serializers;

/// <summary>
///     A custom JSON serializer context for handling serialization of objects related to PostConfirmation events.
/// </summary>
[JsonSerializable(typeof(PostConfirmationEvent))]
[JsonSerializable(typeof(PostConfirmationRequest))]
[JsonSerializable(typeof(PostConfirmationResponse))]
public partial class PostConfirmationJsonSerializerContext : JsonSerializerContext
{
}