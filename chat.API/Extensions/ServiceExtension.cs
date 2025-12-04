using chat.Domain;
using chat.Repo;
using chat.Service.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace chat.API.Extensions
{
    public static class ServiceExtension
    {
        public static void ConfigureCors(this IServiceCollection services, Appsettings config)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(config.CorsPolicyName ?? throw new ArgumentNullException(),
                    builder => builder
                    .SetIsOriginAllowed(origin =>
                    {
                        if (Uri.TryCreate(origin, UriKind.Absolute, out var uri))
                        {
                            var host = uri.Host.ToLowerInvariant();
                            var allowedDomains = config.FrontendUrl!
                                .Select(d => d.ToLowerInvariant())
                                .ToList();
                            return allowedDomains!.Any(h => host.Contains(h));
                        }
                        return false;
                    })
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
        }
       
       
    }
}
