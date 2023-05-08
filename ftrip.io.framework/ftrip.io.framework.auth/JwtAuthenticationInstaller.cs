using ftrip.io.framework.Contexts;
using ftrip.io.framework.Installers;
using ftrip.io.framework.Secrets;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace ftrip.io.framework.auth
{
    public class JwtAuthenticationInstaller : IInstaller
    {
        private readonly IServiceCollection _services;

        public JwtAuthenticationInstaller(IServiceCollection services)
        {
            _services = services;
        }

        public void Install()
        {
            AddJwtScheme();
            AddFilters();
            AddDependencies();
        }

        private void AddJwtScheme()
        {
            using var serviceProvider = _services.BuildServiceProvider();
            var secretsManager = serviceProvider.GetService(typeof(ISecretsManager)) as ISecretsManager;

            var sercet = secretsManager.Get("JWT_SECRET");
            var key = Encoding.ASCII.GetBytes(sercet);

            _services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                AdditionallyConfigureAuthentication(options);
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                AdditionallyConfigureJwtBearer(options);
            });
        }

        protected virtual void AdditionallyConfigureAuthentication(AuthenticationOptions options)
        {
        }

        protected virtual void AdditionallyConfigureJwtBearer(JwtBearerOptions options)
        {
        }

        private void AddFilters()
        {
            _services.AddControllers(c => c.Filters.Add<FillCurrentUserContextFilter>());
        }

        private void AddDependencies()
        {
            _services.AddScoped(typeof(CurrentUserContext));
        }
    }
}