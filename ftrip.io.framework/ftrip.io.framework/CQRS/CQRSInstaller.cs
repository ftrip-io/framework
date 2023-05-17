using ftrip.io.framework.Installers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ftrip.io.framework.CQRS
{
    public class CQRSInstaller<T> : IInstaller
    {
        private readonly IServiceCollection _services;

        public CQRSInstaller(IServiceCollection services)
        {
            _services = services;
        }

        public void Install()
        {
            _services.AddMediatR(typeof(T));
            _services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
        }
    }
}