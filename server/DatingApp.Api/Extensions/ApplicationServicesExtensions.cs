using DatingApp.Api.Data;
using DatingApp.Api.Services.AuthTokenIssuing;
using DatingApp.Api.Services.Hashing;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace DatingApp.Api.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DatingAppDbContext>(config =>
            {
                Enum.TryParse<ServerType>("MySql", true, out var serverType);
                config.UseMySql(configuration.GetConnectionString("DatingApp"), MySqlServerVersion.Create(Version.Parse("8.0.30"), serverType));
            });
            services.AddScoped<IHashProvider, HashProvider>();
            services.AddScoped<IAuthTokenIssuing, AuthTokenIssuer>();


            return services;
        }
    }
}