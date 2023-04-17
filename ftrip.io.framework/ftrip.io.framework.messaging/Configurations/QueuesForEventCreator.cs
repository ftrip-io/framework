using ftrip.io.framework.Domain;
using ftrip.io.framework.messaging.Attributes;
using ftrip.io.framework.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ftrip.io.framework.messaging.Configurations
{
    internal static class QueuesForEventCreator
    {
        public static QueuesForEvent FromAssembly<TAssembly>()
        {
            var queuesForEvent = new QueuesForEvent();

            GetPossibleFtripioAssemblies(typeof(TAssembly).Assembly)
                .ToList()
                .ForEach(assembly =>
                {
                    assembly.ExportedTypes
                        .Where(type => typeof(IEvent).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                        .Where(HasQueueAttribues)
                        .ToList()
                        .ForEach(eventType =>
                        {
                            queuesForEvent[eventType].AddRange(GetQueueAttribues(eventType).Select(attribute => attribute.Name));
                        });
                });

            return queuesForEvent;
        }

        private static IList<Assembly> GetPossibleFtripioAssemblies(Assembly assembly)
        {
            var assemblies = new List<Assembly>
            {
                assembly
            };

            assemblies.AddRange(
                assembly.GetReferencedAssemblies()
                    .Where(a => a.Name.StartsWith("ftrip.io"))
                    .Select(a => Assembly.Load(a))
            );

            return assemblies;
        }

        private static IList<QueueAttribute> GetQueueAttribues(Type type) => CustomAttributeFinder.GetFrom<QueueAttribute>(type);

        private static bool HasQueueAttribues(Type type) => CustomAttributeFinder.Has<QueueAttribute>(type);
    }
}