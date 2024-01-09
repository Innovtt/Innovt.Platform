// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito

using System.Text.Json.Serialization;
using Innovt.Cloud.AWS.Lambda.Cognito.Events.VerifyAuthChallenge;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Serializers;

/// <summary>
///     A custom JSON serializer context for handling serialization of objects related to VerifyAuthChallenge events.
/// </summary>
[JsonSerializable(typeof(VerifyAuthChallengeEvent))]
[JsonSerializable(typeof(VerifyAuthChallengeRequest))]
[JsonSerializable(typeof(VerifyAuthChallengeResponse))]
public partial class VerifyAuthChallengeJsonSerializerContext : JsonSerializerContext;