using ftrip.io.framework.Installers;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;

namespace ftrip.io.framework.Tracing
{
    public class TracingInstaller : IInstaller
    {
        private readonly IServiceCollection _services;
        private readonly TracingSettings _tracingSettings;
        private readonly JaegerSettings _jaegerSettings;

        public TracingInstaller(IServiceCollection services, Action<TracingSettings> settingsBuilder)
        {
            _services = services;

            _tracingSettings = new TracingSettings();
            _jaegerSettings = new FromEnvJaegerSettings();
            settingsBuilder(_tracingSettings);
        }

        public TracingInstaller(IServiceCollection services, Action<TracingSettings, JaegerSettings> settingsBuilder)
        {
            _services = services;

            _tracingSettings = new TracingSettings();
            _jaegerSettings = new JaegerSettings();
            settingsBuilder(_tracingSettings, _jaegerSettings);
        }

        public void Install()
        {
            var tracer = new Tracer(_tracingSettings.ApplicationLabel, _tracingSettings.ApplicationVersion);
            _services.AddSingleton<ITracer>(tracer);

            Action<ResourceBuilder> resourceBuilder = builder =>
            {
                builder.AddService(
                    _tracingSettings.ApplicationLabel,
                    serviceVersion: _tracingSettings.ApplicationVersion,
                    serviceInstanceId: _tracingSettings.MachineName
                )
                .AddTelemetrySdk()
                .AddEnvironmentVariableDetector();
            };

            Action<TracerProviderBuilder> traceBuilder = builder =>
            {
                builder
                    .AddSource(_tracingSettings.ApplicationLabel)
                    .SetSampler(new AlwaysOnSampler())
                    .AddMassTransitInstrumentation()
                    .AddHttpClientInstrumentation(x =>
                    {
                        var spamPaths = new[]
                        {
                            "/loki/api",
                            "api/health"
                        };

                        x.FilterHttpRequestMessage = ((HttpRequestMessage arg) =>
                            spamPaths.All(path => !arg.RequestUri.AbsolutePath.Contains(path))
                        );
                    })
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddMongoDBInstrumentation()
                    .AddProcessor<SpamTracesProcessor>();

                builder.AddJaegerExporter(o =>
                {
                    o.AgentHost = _jaegerSettings.Server;
                    o.AgentPort = int.Parse(_jaegerSettings.Port);
                    o.MaxPayloadSizeInBytes = 4096;
                    o.ExportProcessorType = ExportProcessorType.Batch;
                    o.BatchExportProcessorOptions = new BatchExportProcessorOptions<Activity>
                    {
                        MaxQueueSize = 2048,
                        ScheduledDelayMilliseconds = 5000,
                        ExporterTimeoutMilliseconds = 30000,
                        MaxExportBatchSize = 512,
                    };
                });
            };

            _services
                .AddOpenTelemetry()
                .ConfigureResource(resourceBuilder)
                .WithTracing(traceBuilder);
        }
    }
}