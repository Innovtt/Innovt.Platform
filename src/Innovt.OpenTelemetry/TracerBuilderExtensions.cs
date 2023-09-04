// Innovt Company
// Author: Michel Borges
// Project: Innovt.OpenTelemetry

using System;
using OpenTelemetry.Trace;

namespace Innovt.OpenTelemetry;

/// <summary>
///  This is a simple exporter that logs telemetry to the console.
/// </summary>
public static class TracerBuilderExtensions
{
    /// <summary>
    /// Adds a simple activity exporter that logs telemetry to the console.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static TracerProviderBuilder AddInnovtInstrumentation(this TracerProviderBuilder builder)
    {
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        builder.AddSource("Innovt.Cloud.AWS.Cognito.CognitoIdentityProvider")
            .AddSource("Innovt.Cloud.AWS.Dynamo.Repository")
            .AddSource("Innovt.Cloud.AWS.KinesisDataProducer")
            .AddSource("Innovt.Cloud.AWS.S3.S3FileSystem")
            .AddSource("Innovt.Cloud.AWS.SQS.QueueService")
            .AddSource("Innovt.Cloud.AWS.Lambda.EventProcessor");

        return builder;
    }
}