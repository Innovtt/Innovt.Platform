// Innovt Company
// Author: Michel Borges
// Project: Innovt.OpenTelemetry

using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Innovt.Core.Utilities;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Resources;

namespace Innovt.OpenTelemetry;

/// <summary>
///    This is a simple exporter that logs telemetry to the console.
/// </summary>
public class LoggerActivityExporter : BaseExporter<Activity>
{
    private const string StatusCodeKey = "otel.status_code";
    private const string StatusDescriptionKey = "otel.status_description";
    private static Core.CrossCutting.Log.ILogger logger;
    private readonly IServiceCollection serviceCollection;

    /// <summary>
    ///    Initializes a new instance of the <see cref="LoggerActivityExporter" /> class.
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public LoggerActivityExporter(IServiceCollection serviceCollection)
    {
        this.serviceCollection = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));
    }

    private void WriteLog(string logContent, params object[] parameters)
    {
        if (logContent.IsNullOrEmpty())
            return;

        logger ??= serviceCollection.BuildServiceProvider().GetService<Core.CrossCutting.Log.ILogger>();

        if (logger is null)
            Console.WriteLine(logContent);
        else
            logger.Info(logContent, parameters);
    }


    /// <summary>
    ///   Exports a batch of telemetry data.
    /// </summary>
    /// <param name="batch"></param>
    /// <returns></returns>
    public override ExportResult Export(in Batch<Activity> batch)
    {
        var logBuffer = new StringBuilder();

        foreach (var activity in batch)
        {
            logBuffer.AppendLine($"Activity.TraceId:          {activity.TraceId}");
            logBuffer.AppendLine($"Activity.SpanId:           {activity.SpanId}");
            logBuffer.AppendLine($"Activity.TraceFlags:           {activity.ActivityTraceFlags}");
            if (!string.IsNullOrEmpty(activity.TraceStateString))
            {
                logBuffer.AppendLine($"Activity.TraceState:    {activity.TraceStateString}");
            }

            if (activity.ParentSpanId != default)
            {
                logBuffer.AppendLine($"Activity.ParentSpanId:    {activity.ParentSpanId}");
            }

            logBuffer.AppendLine($"Activity.ActivitySourceName: {activity.Source.Name}");
            logBuffer.AppendLine($"Activity.DisplayName: {activity.DisplayName}");
            logBuffer.AppendLine($"Activity.Kind:        {activity.Kind}");
            logBuffer.AppendLine($"Activity.StartTime:   {activity.StartTimeUtc:yyyy-MM-ddTHH:mm:ss.fffffffZ}");
            logBuffer.AppendLine($"Activity.Duration:    {activity.Duration}");
            var statusCode = string.Empty;
            var statusDesc = string.Empty;

            if (activity.TagObjects.Any())
            {
                logBuffer.AppendLine("Activity.Tags:");
                foreach (var tag in activity.TagObjects)
                {
                    if (tag.Key == StatusCodeKey)
                    {
                        statusCode = tag.Value as string;
                        continue;
                    }

                    if (tag.Key == StatusDescriptionKey)
                    {
                        statusDesc = tag.Value as string;
                        continue;
                    }

                    logBuffer.AppendLine($"    {tag}");
                }
            }

            if (activity.Status != ActivityStatusCode.Unset)
            {
                logBuffer.AppendLine($"StatusCode : {activity.Status}");
                if (!string.IsNullOrEmpty(activity.StatusDescription))
                {
                    logBuffer.AppendLine($"Activity.StatusDescription : {activity.StatusDescription}");
                }
            }
            else if (!string.IsNullOrEmpty(statusCode))
            {
                logBuffer.AppendLine($"   StatusCode : {statusCode}");
                if (!string.IsNullOrEmpty(statusDesc))
                {
                    logBuffer.AppendLine($"   Activity.StatusDescription : {statusDesc}");
                }
            }

            if (activity.Events.Any())
            {
                logBuffer.AppendLine("Activity.Events:");
                foreach (var activityEvent in activity.Events)
                {
                    logBuffer.AppendLine($"    {activityEvent.Name} [{activityEvent.Timestamp}]");
                    foreach (var attribute in activityEvent.Tags)
                    {
                        logBuffer.AppendLine($"        {attribute}");
                    }
                }
            }

            if (activity.Links.Any())
            {
                logBuffer.AppendLine("Activity.Links:");
                foreach (var activityLink in activity.Links)
                {
                    logBuffer.AppendLine($"    {activityLink.Context.TraceId} {activityLink.Context.SpanId}");
                    foreach (var attribute in activityLink.Tags)
                    {
                        logBuffer.AppendLine($"        {attribute}");
                    }
                }
            }

            var resource = ParentProvider.GetResource();
            if (resource != Resource.Empty)
            {
                logBuffer.AppendLine("Resource associated with Activity:");
                foreach (var resourceAttribute in resource.Attributes)
                {
                    logBuffer.AppendLine($"    {resourceAttribute}");
                }
            }

            logBuffer.AppendLine(string.Empty);

            WriteLog(logBuffer.ToString());

            logBuffer.Clear();
        }

        return ExportResult.Success;
    }
}