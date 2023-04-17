using ftrip.io.framework.messaging.Attributes;
using ftrip.io.framework.Utilities;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ftrip.io.framework.messaging.Configurations
{
    internal class ConsumersForQueueCreator
    {
        public static ConsumersForQueue FromAssembly<TAssembly>()
        {
            var consumersForQueue = new ConsumersForQueue();

            typeof(TAssembly)
                .Assembly
                .ExportedTypes
                .Where(type => typeof(IConsumer).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                .Where(HasQueueAttribues)
                .ToList()
                .ForEach(consumerType =>
                {
                    var attributes = GetQueueAttribues(consumerType);
                    foreach (var attribute in attributes)
                    {
                        consumersForQueue[attribute.Name].Add(consumerType);
                    }
                });

            return consumersForQueue;
        }

        private static IList<QueueAttribute> GetQueueAttribues(Type type) => CustomAttributeFinder.GetFrom<QueueAttribute>(type);

        private static bool HasQueueAttribues(Type type) => CustomAttributeFinder.Has<QueueAttribute>(type);
    }
}