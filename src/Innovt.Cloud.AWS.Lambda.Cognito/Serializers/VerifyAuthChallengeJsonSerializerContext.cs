// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito

using System.Text.Json.Serialization;
using Innovt.Cloud.AWS.Lambda.Cognito.Events.VerifyAuthChallenge;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Serializers;

[JsonSerializable(typeof(VerifyAuthChallengeEvent))]
[JsonSerializable(typeof(VerifyAuthChallengeRequest))]
[JsonSerializable(typeof(VerifyAuthChallengeResponse))]
public partial class VerifyAuthChallengeJsonSerializerContext : JsonSerializerContext
{
}