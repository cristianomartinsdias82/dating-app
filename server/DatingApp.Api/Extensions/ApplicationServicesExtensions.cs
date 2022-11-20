using DatingApp.Api.ActionFilters;
using DatingApp.Api.Data;
using DatingApp.Api.Data.Persistence;
using DatingApp.Api.Mappings;
using DatingApp.Api.Services.AuthTokenIssuing;
using DatingApp.Api.Services.Hashing;
using DatingApp.Api.Services.ImageUploading;
using DatingApp.Api.Settings;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace DatingApp.Api.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(configuration.GetSection(nameof(ImageUploadExternalProviderSettings)).Get<ImageUploadExternalProviderSettings>());

            services.AddDbContext<DatingAppDbContext>(config =>
            {
                Enum.TryParse<ServerType>("MySql", true, out var serverType);
                config.UseMySql(configuration.GetConnectionString("DatingApp"), MySqlServerVersion.Create(Version.Parse("8.0.30"), serverType));
            });
            services.AddScoped<LogUserActivity>();
            services.AddScoped<IHashProvider, HashProvider>();
            services.AddScoped<IAuthTokenIssuing, AuthTokenIssuer>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IImageUploadServiceClient, CloudinaryImageUploadServiceClient>();
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            
            return services;
        }
    }
}