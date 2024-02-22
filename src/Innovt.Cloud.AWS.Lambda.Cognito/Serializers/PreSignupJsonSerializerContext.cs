// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito

using System.Text.Json.Serialization;
using Innovt.Cloud.AWS.Lambda.Cognito.Events.PreSignup;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Serializers;

/// <summary>
///     A custom JSON serializer context for handling serialization of objects related to PreSignup events.
/// </summary>
[JsonSerializable(typeof(PreSignupEvent))]
[JsonSerializable(typeof(PreSignupRequest))]
[JsonSerializable(typeof(PreSignupResponse))]
public partial class PreSignupJsonSerializerContext : JsonSerializerContext;