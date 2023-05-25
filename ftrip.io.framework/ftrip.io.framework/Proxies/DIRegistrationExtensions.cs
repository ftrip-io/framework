using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace ftrip.io.framework.Proxies
{
    public static class DIRegistrationExtensions
    {
        public static void AddProxiedScoped<TInterface, TImplementation>(this IServiceCollection services)
            where TInterface : class
            where TImplementation : class, TInterface
        {
            services.AddScoped<TImplementation>();
            services.AddScoped(typeof(TInterface), serviceProvider =>
            {
                var proxyGenerator = serviceProvider.GetRequiredService<ProxyGenerator>();
                var actual = serviceProvider.GetRequiredService<TImplementation>();
                var interceptors = serviceProvider.GetServices<IInterceptor>().ToArray();

                return proxyGenerator.CreateInterfaceProxyWithTarget(typeof(TInterface), actual, interceptors);
            });
        }

        public static void AddProxiedScoped<TInterface, TImplementation>(this IServiceCollection services, Type[] interceptorTypes)
            where TInterface : class
            where TImplementation : class, TInterface
        {
            services.AddScoped<TImplementation>();
            services.AddScoped(typeof(TInterface), serviceProvider =>
            {
                var proxyGenerator = serviceProvider.GetRequiredService<ProxyGenerator>();
                var actual = serviceProvider.GetRequiredService<TImplementation>();
                var interceptors = interceptorTypes?
                    .Select(interceptorType => serviceProvider.GetService(interceptorType))
                    .Cast<IInterceptor>()
                    .ToArray();

                return proxyGenerator.CreateInterfaceProxyWithTarget(typeof(TInterface), actual, interceptors);
            });
        }

        public static void AddProxiedScoped(this IServiceCollection services, Type TInterface, Type TImplementation)
        {
            services.AddScoped(TImplementation);
            services.AddScoped(TInterface, serviceProvider =>
            {
                var proxyGenerator = serviceProvider.GetRequiredService<ProxyGenerator>();
                var actual = serviceProvider.GetRequiredService(TImplementation);
                var interceptors = serviceProvider.GetServices<IInterceptor>().ToArray();

                return proxyGenerator.CreateInterfaceProxyWithTarget(TInterface, actual, interceptors);
            });
        }

        public static void AddProxiedScoped(this IServiceCollection services, Type TInterface, Type TImplementation, Type[] interceptorTypes)
        {
            services.AddScoped(TImplementation);
            services.AddScoped(TInterface, serviceProvider =>
            {
                var proxyGenerator = serviceProvider.GetRequiredService<ProxyGenerator>();
                var actual = serviceProvider.GetRequiredService(TImplementation);
                var interceptors = interceptorTypes?
                    .Select(interceptorType => serviceProvider.GetService(interceptorType))
                    .Cast<IInterceptor>()
                    .ToArray();

                return proxyGenerator.CreateInterfaceProxyWithTarget(TInterface, actual, interceptors);
            });
        }
    }
}