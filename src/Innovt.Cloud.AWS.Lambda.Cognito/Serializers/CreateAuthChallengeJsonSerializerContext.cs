// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito

using System.Text.Json.Serialization;
using Innovt.Cloud.AWS.Lambda.Cognito.Events.CreateAuthChallenge;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Serializers;

[JsonSerializable(typeof(CreateAuthChallengeEvent))]
[JsonSerializable(typeof(CreateAuthChallengeRequest))]
[JsonSerializable(typeof(CreateAuthChallengeResponse))]
public partial class CreateAuthChallengeJsonSerializerContext : JsonSerializerContext
{
}