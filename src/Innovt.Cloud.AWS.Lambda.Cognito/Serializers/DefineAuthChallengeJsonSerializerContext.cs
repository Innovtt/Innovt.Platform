// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito

using System.Text.Json.Serialization;
using Innovt.Cloud.AWS.Lambda.Cognito.Events.DefineAuthChallenge;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Serializers;

[JsonSerializable(typeof(DefineAuthChallengeEvent))]
[JsonSerializable(typeof(DefineAuthChallengeRequest))]
[JsonSerializable(typeof(DefineAuthChallengeResponse))]
public partial class DefineAuthChallengeJsonSerializerContext : JsonSerializerContext
{
}