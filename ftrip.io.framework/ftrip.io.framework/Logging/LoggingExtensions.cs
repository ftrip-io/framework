using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.Grafana.Loki;
using System;
using System.Collections.Generic;

namespace ftrip.io.framework.Logging
{
    public static class LoggingExtensions
    {
        public static IHostBuilder UseSerilogLogging(this IHostBuilder builder, Func<HostBuilderContext, LoggingOptions> loggingOptionsBuilder)
        {
            builder.UseSerilog((hostingContext, loggerConfig) =>
            {
                var options = loggingOptionsBuilder(hostingContext);

                loggerConfig = loggerConfig.MinimumLevel.Information()
                    .Enrich.WithProperty("Application", options.ApplicationName)
                    .Enrich.WithClientIp(options.ApplicationLabel)
                    .Enrich.FromLogContext();

                if (options.WriteToGrafanaLoki)
                {
                    loggerConfig = loggerConfig.WriteTo.GrafanaLoki(
                        options.GrafanaLokiUrl,
                        new List<LokiLabel>() { new LokiLabel() { Key = "service", Value = options.ApplicationLabel } }
                    );
                }

                if (options.WriteToConsole)
                {
                    loggerConfig = loggerConfig.WriteTo.Console(
                        outputTemplate: options.ConsoleOutputTemplate
                    );
                }
            });

            return builder;
        }
    }
}