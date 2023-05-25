using ftrip.io.framework.Installers;
using ftrip.io.framework.messaging.Configurations;
using ftrip.io.framework.messaging.Filters;
using ftrip.io.framework.messaging.Logging;
using ftrip.io.framework.messaging.Publisher;
using ftrip.io.framework.messaging.Settings;
using ftrip.io.framework.Proxies;
using GreenPipes;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ftrip.io.framework.messaging.Installers
{
    public class RabbitMQInstaller<T> : IInstaller
    {
        private readonly IServiceCollection _services;
        private readonly RabbitMQSettings _settings;
        private readonly RabbitMQInstallerType _types;

        public RabbitMQInstaller(IServiceCollection services, RabbitMQInstallerType types, RabbitMQSettings settings = null)
        {
            _services = services;
            _types = types;
            if (settings == null)
            {
                _settings = new FromEnvRabbitMQSettings();
            }
        }

        public void Install()
        {
            if (_types.Is(RabbitMQInstallerType.None))
            {
                return;
            }

            _services.AddSingleton(_settings);

            if (_types.Has(RabbitMQInstallerType.Publisher))
            {
                _services.AddProxiedScoped<IMessagePublisher, MessagePublisher>();
                var queuesForEvent = QueuesForEventCreator.FromAssembly<T>();
                _services.AddSingleton(queuesForEvent);
            }

            _services.AddMassTransit(x =>
            {
                if (_types.Has(RabbitMQInstallerType.Consumer))
                {
                    x.AddConsumers(typeof(T).Assembly);
                    x.SetKebabCaseEndpointNameFormatter();
                }

                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
                {
                    config.Host(new Uri(_settings.GetConnectionString()), h =>
                    {
                        h.Username(_settings.User);
                        h.Password(_settings.Password);
                    });

                    config.ConfigureEndpoints(provider);
                    config.ConnectConsumeObserver(new LoggerObservable());
                    config.UseConsumeFilter(typeof(PopulateLogContextFilter<>), provider);

                    if (_types.Has(RabbitMQInstallerType.Consumer))
                    {
                        var consumersForQueue = ConsumersForQueueCreator.FromAssembly<T>();
                        foreach (var queue in consumersForQueue.Keys)
                        {
                            config.ReceiveEndpoint(queue, ep =>
                            {
                                ep.PrefetchCount = 16;
                                ep.UseMessageRetry(r => r.Interval(2, 100));
                                consumersForQueue[queue].ForEach(consumer => ep.ConfigureConsumer(provider, consumer));
                            });
                        }
                    }
                }));
            });

            _services.AddMassTransitHostedService();
        }
    }
}