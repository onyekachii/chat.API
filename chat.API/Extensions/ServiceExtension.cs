using chat.Domain;
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
                    .AllowAnyHeader());
            });
        }
       
        public static void ConfigureMySqlContext(this IServiceCollection services, string connString)
        {
            services.AddDbContext<RepositoryContext<Guid>>(o => o.UseMySql(connString, MySqlServerVersion.LatestSupportedServerVersion));
        }
    }
}
