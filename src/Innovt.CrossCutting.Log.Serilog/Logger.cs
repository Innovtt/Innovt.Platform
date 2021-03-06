﻿using System;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Innovt.CrossCutting.Log.Serilog
{
    public class Logger: Core.CrossCutting.Log.ILogger
    {
        private readonly global::Serilog.Core.Logger logger=null;

        private const string ConsoleTemplate =
            "[{Timestamp:HH:mm:ss} {Level:u3}] {TraceId:TraceId} {SpanId:SpanId} {Message:lj}{NewLine}{Exception}{NewLine}{Properties:j}";
        
        /// <summary>
        /// The default sink is Console
        /// </summary>
        public Logger():this(new LoggerConfiguration())
        {
           
        }

        public Logger(ILogEventEnricher enricher)
        {
            if (enricher is null)
            {
                throw new ArgumentNullException(nameof(enricher));
            }

            logger = new LoggerConfiguration().WriteTo.Console(outputTemplate:ConsoleTemplate
                ).Enrich.With(enricher).CreateLogger();
        }

        public Logger(LoggerConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            //The default Enricher ir OpenTracing
            logger = configuration.WriteTo.Console(outputTemplate:ConsoleTemplate).Enrich.With<OpenTracingContextLogEnricher>().Enrich.FromLogContext().CreateLogger();
        }

        public void Debug(string messageTemplate)
        {
            if (!IsEnabled(LogLevel.Debug)) {
                Console.WriteLine("LogLevel Debug not enabled.");
                return;
            }
          
            logger.Debug(messageTemplate);
        }

        public void Debug(string messageTemplate, params object[] propertyValues)
        {
            if (!IsEnabled(LogLevel.Debug))
            {
                Console.WriteLine("LogLevel Debug not enabled.");
                return;
            }

            logger.Debug(messageTemplate,propertyValues: propertyValues);
        }

        public void Debug(Exception exception, string messageTemplate)
        {
            if (!IsEnabled(LogLevel.Debug))
            {
                Console.WriteLine("LogLevel Debug not enabled.");
                return;
            }

            logger.Debug(exception, messageTemplate);
        }

        public void Debug(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            if (!IsEnabled(LogLevel.Debug))
            {
                Console.WriteLine("LogLevel Debug not enabled.");
                return;
            }

            logger.Debug(exception, messageTemplate, propertyValues);
        }

        public void Error(string messageTemplate)
        {
            if (!IsEnabled(LogLevel.Error))
            {
                Console.WriteLine("LogLevel Error not enabled.");
                return;
            }

            logger.Error(messageTemplate);
        }

        public void Error(string messageTemplate, params object[] propertyValues)
        {
            if (!IsEnabled(LogLevel.Error))
            {
                Console.WriteLine("LogLevel Error not enabled.");
                return;
            }

            logger.Error(messageTemplate, propertyValues);
        }

        public void Error(Exception exception, string messageTemplate)
        {
            if (!IsEnabled(LogLevel.Error))
            {
                Console.WriteLine("LogLevel Error not enabled.");
                return;
            }

            logger.Error(exception, messageTemplate);
        }

        public void Error(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            if (!IsEnabled(LogLevel.Error))
            {
                Console.WriteLine("LogLevel Error not enabled.");
                return;
            }

            logger.Error(exception, messageTemplate, propertyValues);
        }

        public void Fatal(string messageTemplate)
        {
            if (!IsEnabled(LogLevel.Critical))
            {
                Console.WriteLine("LogLevel Critical not enabled.");
                return;
            }

            logger.Fatal(messageTemplate);
        }

        public void Fatal(string messageTemplate, params object[] propertyValues)
        {
            if (!IsEnabled(LogLevel.Critical))
            {
                Console.WriteLine("LogLevel Critical not enabled.");
                return;
            }

            logger.Fatal(messageTemplate, propertyValues);
        }

        public void Fatal(Exception exception, string messageTemplate)
        {
            if (!IsEnabled(LogLevel.Critical))
            {
                Console.WriteLine("LogLevel Critical not enabled.");
                return;
            }

            logger.Fatal(exception,messageTemplate);
        }

        public void Fatal(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            if (!IsEnabled(LogLevel.Critical))
            {
                Console.WriteLine("LogLevel Critical not enabled.");
                return;
            }

            logger.Fatal(exception, messageTemplate, propertyValues);
        }

        public void Info(string messageTemplate)
        {
            if (!IsEnabled(LogLevel.Information))
            {
                Console.WriteLine("LogLevel Info not enabled.");
                return;
            }

            logger.Information(messageTemplate);
        }

        public void Info(string messageTemplate, params object[] propertyValues)
        {
            if (!IsEnabled(LogLevel.Information))
            {
                Console.WriteLine("LogLevel Info not enabled.");
                return;
            }

            logger.Information(messageTemplate, propertyValues);
        }

        public void Info(Exception exception, string messageTemplate)
        {
            if (!IsEnabled(LogLevel.Information))
            {
                Console.WriteLine("LogLevel Info not enabled.");
                return;
            }

            logger.Information(exception,messageTemplate);
        }

        public void Info(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            if (!IsEnabled(LogLevel.Information))
            {
                Console.WriteLine("LogLevel Info not enabled.");
                return;
            }

            logger.Information(exception, messageTemplate, propertyValues);
        }

        public void Verbose(string messageTemplate)
        {
            if (!IsEnabled(LogLevel.Trace))
            {
                Console.WriteLine("LogLevel Trace not enabled.");
                return;
            }

            logger.Verbose(messageTemplate);
        }

        public void Verbose(string messageTemplate, params object[] propertyValues)
        {
            if (!IsEnabled(LogLevel.Trace))
            {
                Console.WriteLine("LogLevel Trace not enabled.");
                return;
            }

            logger.Verbose(messageTemplate, propertyValues);
        }

        public void Verbose(Exception exception, string messageTemplate)
        {
            if (!IsEnabled(LogLevel.Trace))
            {
                Console.WriteLine("LogLevel Trace not enabled.");
                return;
            }

            logger.Verbose(exception,messageTemplate);
        }

        public void Verbose(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            if (!IsEnabled(LogLevel.Trace))
            {
                Console.WriteLine("LogLevel Trace not enabled.");
                return;
            }

            logger.Verbose(exception, messageTemplate, propertyValues);
        }

        public void Warning(string messageTemplate)
        {
            if (!IsEnabled(LogLevel.Warning))
            {
                Console.WriteLine("LogLevel Warning not enabled.");
                return;
            }

            logger.Warning(messageTemplate);
        }

        public void Warning(string messageTemplate, params object[] propertyValues)
        {
            if (!IsEnabled(LogLevel.Warning))
            {
                Console.WriteLine("LogLevel Warning not enabled.");
                return;
            }

            logger.Warning(messageTemplate, propertyValues);
        }

        public void Warning(Exception exception, string messageTemplate)
        {
            if (!IsEnabled(LogLevel.Warning))
            {
                Console.WriteLine("LogLevel Warning not enabled.");
                return;
            }

            logger.Warning(exception,messageTemplate);
        }

        public void Warning(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            if (!IsEnabled(LogLevel.Warning))
            {
                Console.WriteLine("LogLevel Warning not enabled.");
                return;
            }

            logger.Warning(exception, messageTemplate, propertyValues);
        }


        private bool IsEnabled(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    {
                        return logger.IsEnabled(LogEventLevel.Debug) || logger.IsEnabled(LogEventLevel.Verbose);
                    }
                case LogLevel.Information:
                    {
                        return logger.IsEnabled(LogEventLevel.Information);
                    }
                case LogLevel.Warning:
                    {
                        return logger.IsEnabled(LogEventLevel.Warning);
                    }
                case LogLevel.Error:
                    {
                        return logger.IsEnabled(LogEventLevel.Error);
                    }

                case LogLevel.Critical:
                    {
                        return logger.IsEnabled(LogEventLevel.Fatal);
                    }
                default:
                    {
                        return false;
                    }
            }
        }

    }
}
