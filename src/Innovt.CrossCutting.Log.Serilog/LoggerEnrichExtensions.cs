using Serilog;
using Serilog.Configuration;
using System;

namespace Innovt.CrossCutting.Log.Serilog;

public static class LoggerEnrichExtensions
{
    public static LoggerConfiguration WithDataDogEnrich(this LoggerEnrichmentConfiguration enrichmentConfiguration)
    {
        if (enrichmentConfiguration == null) throw new ArgumentNullException(nameof(enrichmentConfiguration));

        return enrichmentConfiguration.With<DataDogEnrich>();
    }

    public static LoggerConfiguration WithActivityEnrich(this LoggerEnrichmentConfiguration enrichmentConfiguration)
    {
        if (enrichmentConfiguration == null) throw new ArgumentNullException(nameof(enrichmentConfiguration));

        return enrichmentConfiguration.With<ActivityEnrich>();
    }

}