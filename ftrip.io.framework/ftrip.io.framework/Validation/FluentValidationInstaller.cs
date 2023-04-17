using FluentValidation.AspNetCore;
using ftrip.io.framework.Installers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace ftrip.io.framework.Validation
{
    public class FluentValidationInstaller<T> : IInstaller
    {
        private readonly IServiceCollection _services;

        public FluentValidationInstaller(IServiceCollection services)
        {
            _services = services;
        }

        public void Install()
        {
            _services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

#pragma warning disable CS0618 // Type or member is obsolete
            _services.AddFluentValidation(settings => settings.RegisterValidatorsFromAssemblyContaining<T>());
#pragma warning restore CS0618 // Type or member is obsolete

            _services.AddControllers(options => options.Filters.Add<ValidationFilter>());
        }
    }
}