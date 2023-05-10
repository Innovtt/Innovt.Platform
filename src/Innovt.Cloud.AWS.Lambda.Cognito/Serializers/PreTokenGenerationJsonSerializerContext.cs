// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito

using System.Text.Json.Serialization;
using Innovt.Cloud.AWS.Lambda.Cognito.Events.PreTokenGeneration;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Serializers;

[JsonSerializable(typeof(PreTokenGenerationEvent))]
[JsonSerializable(typeof(PreTokenGenerationRequest))]
[JsonSerializable(typeof(PreTokenGenerationResponse))]
public partial class PreTokenGenerationJsonSerializerContext : JsonSerializerContext
{
}