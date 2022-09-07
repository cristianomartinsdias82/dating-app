using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.Api.Extensions
{
    public static class IdentityServicesExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true, //our server is gonna assign the token. we need to tell it the signed token is correct
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"])),
                        ValidateIssuer = false, //For academic development purposes, no need to validate the issuer as THIS application issues the token itself
                        ValidateAudience = false //For academic development purposes, no need to validate the application(consumer) of the token which in this case is the DatingApp (Angular SPA)
                    };
                });

            return services;
        }
    }
}