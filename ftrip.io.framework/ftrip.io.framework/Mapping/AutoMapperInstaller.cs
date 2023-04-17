using AutoMapper;
using ftrip.io.framework.Installers;
using ftrip.io.framework.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ftrip.io.framework.Mapping
{
    public class AutoMapperInstaller<T> : IInstaller
    {
        private readonly IServiceCollection _services;

        public AutoMapperInstaller(IServiceCollection services)
        {
            _services = services;
        }

        public void Install()
        {
            _services.AddAutoMapper(typeof(T));

            _services.AddAutoMapper(configuration =>
            {
                FindMappableTypes<T>().ForEach(kvp => configuration.CreateMap(kvp.Source, kvp.Destination));
            });
        }

        private static List<Mapping> FindMappableTypes<TAssembly>()
        {
            return typeof(TAssembly).Assembly
                .ExportedTypes
                .Where(HasMappableAttribute)
                .SelectMany(sourceType => GetMappableAttributes(sourceType).Select(attribute => new Mapping(sourceType, attribute.Destination)))
                .ToList();
        }

        private static IList<MappableAttribute> GetMappableAttributes(Type type) => CustomAttributeFinder.GetFrom<MappableAttribute>(type);

        private static bool HasMappableAttribute(Type type) => CustomAttributeFinder.Has<MappableAttribute>(type);

        private class Mapping
        {
            public Type Source { get; set; }
            public Type Destination { get; set; }

            public Mapping(Type source, Type destination)
            {
                Source = source;
                Destination = destination;
            }
        }
    }
}