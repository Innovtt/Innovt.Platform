// Company: Antecipa
// Project: Innovt.Core
// Solution: Innovt.Platform
// Date: 2021-07-18

using System;
using OpenTelemetry.Trace;

namespace Innovt.OpenTelemetry;

public static class TracerBuilderExtensions
{
    public static TracerProviderBuilder AddInnovtInstrumentation(this TracerProviderBuilder builder)
    {
        if (builder == null)
            throw new ArgumentNullException(nameof(builder));

        builder.AddSource("Innovt.Cloud.AWS.Cognito.CognitoIdentityProvider")
            .AddSource("Innovt.Cloud.AWS.Dynamo.Repository")
            .AddSource("Innovt.Cloud.AWS.KinesisDataProducer")
            .AddSource("Innovt.Cloud.AWS.S3.S3FileSystem")
            .AddSource("Innovt.Cloud.AWS.SQS.QueueService")
            .AddSource("Innovt.Cloud.AWS.Lambda.EventProcessor");
        return builder;
    }
}