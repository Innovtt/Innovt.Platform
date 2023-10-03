// Innovt Company
// Author: Michel Borges
// Project: Innovt.CrossCutting.Log.Serilog

using System;
using Serilog;
using Serilog.Configuration;

namespace Innovt.CrossCutting.Log.Serilog;
/// <summary>
/// Provides extension methods to enrich Serilog log events with specific enrichers.
/// </summary>
public static class LoggerEnrichExtensions
{
    /// <summary>
    /// Enriches log events with DataDog trace and span IDs.
    /// </summary>
    /// <param name="enrichmentConfiguration">The logger enrichment configuration.</param>
    /// <returns>The updated logger configuration.</returns>
    public static LoggerConfiguration WithDataDogEnrich(this LoggerEnrichmentConfiguration enrichmentConfiguration)
    {
        if (enrichmentConfiguration == null) throw new ArgumentNullException(nameof(enrichmentConfiguration));

        return enrichmentConfiguration.With<DataDogEnrich>();
    }
    /// <summary>
    /// Enriches log events with activity trace and span IDs.
    /// </summary>
    /// <param name="enrichmentConfiguration">The logger enrichment configuration.</param>
    /// <returns>The updated logger configuration.</returns>
    public static LoggerConfiguration WithActivityEnrich(this LoggerEnrichmentConfiguration enrichmentConfiguration)
    {
        if (enrichmentConfiguration == null) throw new ArgumentNullException(nameof(enrichmentConfiguration));

        return enrichmentConfiguration.With<ActivityEnrich>();
    }
}