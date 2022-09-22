using OpenTelemetry.Trace;
using OpenTelemetry;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace Innovt.OpenTelemetry;

public static class LoggerActivityExporterExtensions
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "The objects should not be disposed.")]
    public static TracerProviderBuilder AddLoggerExporter(this TracerProviderBuilder builder, IServiceCollection serviceCollection)
    {
        if (builder == null) throw new ArgumentNullException(nameof(builder));
        if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));

        return builder.AddProcessor(new SimpleActivityExportProcessor(new LoggerActivityExporter(serviceCollection)));
    }

}