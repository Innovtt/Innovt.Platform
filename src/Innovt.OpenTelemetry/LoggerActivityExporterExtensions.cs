// Innovt Company
// Author: Michel Borges
// Project: Innovt.OpenTelemetry

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Trace;

namespace Innovt.OpenTelemetry;

/// <summary>
///     This is a simple exporter that logs telemetry to the console.
/// </summary>
public static class LoggerActivityExporterExtensions
{
    /// <summary>
    ///     Adds a simple activity exporter that logs telemetry to the console.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="serviceCollection"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope",
        Justification = "The objects should not be disposed.")]
    public static TracerProviderBuilder AddLoggerExporter(this TracerProviderBuilder builder,
        IServiceCollection serviceCollection)
    {
        if (builder == null) throw new ArgumentNullException(nameof(builder));
        if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));

        return builder.AddProcessor(new SimpleActivityExportProcessor(new LoggerActivityExporter(serviceCollection)));
    }
}