using Microsoft.OpenApi.Models;
using DatingApp.Api.Extensions;
using DatingApp.Api.Middlewares;
using DatingApp.Api.ActionFilters;

namespace DatingApp.Api
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            /*
            .AddJsonOptions(jsonOptions =>
            {
                jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
            })
            */;
            services.AddCors();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPIv5", Version = "v1" });
            });

            services.AddApplicationServices(_config);
            services.AddIdentityServices(_config);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRequestProcessorWithExceptionHandling();

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPIv5 v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(policy =>
            {
                var commaSeparatedAllowedOrigins = _config["AllowedOrigins"].Split(',');

                policy
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins(commaSeparatedAllowedOrigins);
            });

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}