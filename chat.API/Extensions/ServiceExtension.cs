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
                    builder => builder.WithOrigins(config.FrontendUrl ?? throw new ArgumentNullException())
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
        }
       
       
    }
}
