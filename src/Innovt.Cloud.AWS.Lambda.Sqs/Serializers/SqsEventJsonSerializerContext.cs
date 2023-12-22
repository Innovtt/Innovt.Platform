// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito

using System.Text.Json.Serialization;
using Amazon.Lambda.SQSEvents;
using Innovt.Core.Serialization;

namespace Innovt.Cloud.AWS.Lambda.Sqs.Serializers;

/// <summary>
///     Represents a JSON serializer context for Sqs Serializer for AOT Support
/// </summary>
[JsonSerializable(typeof(SQSEvent))]
[JsonSerializable(typeof(BatchFailureResponse))]
public class SqsEventJsonSerializerContext : JsonSerializerContextBase
{
}