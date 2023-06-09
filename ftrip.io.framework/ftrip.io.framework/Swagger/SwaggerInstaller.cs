﻿using ftrip.io.framework.Installers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.IO;

namespace ftrip.io.framework.Swagger
{
    public class SwaggerInstaller<T> : IInstaller
    {
        private readonly IServiceCollection _services;
        private readonly SwaggerSettings _settings;

        public SwaggerInstaller(IServiceCollection services, SwaggerSettings settings = null)
        {
            _services = services;
            if (settings == null)
            {
                settings = new FromEnvSwaggerSettings();
            }
            _settings = settings;
        }

        public void Install()
        {
            var mainAssemblyName = typeof(T).Assembly.GetName().Name;

            _services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(name: "v1", new OpenApiInfo
                {
                    Title = $"{mainAssemblyName} API",
                    Version = "v1"
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });

                options.ExampleFilters();

                var xmlFile = $"{mainAssemblyName}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);

                if (!string.IsNullOrEmpty(_settings.ApiPathPrefix))
                {
                    options.DocumentFilter<SwaggerInsertPathPrefixFilter>(_settings.ApiPathPrefix);
                }
            });

            _services.AddSwaggerExamplesFromAssemblyOf<T>();
        }
    }
}