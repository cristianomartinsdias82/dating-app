using System.Text.Json;
using DatingApp.Api.Data;
using DatingApp.Api.Entities;
using DatingApp.Api.Services.Hashing;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Api.Extensions
{

    public static class HostExtensions
    {
        public async static Task<IHost> MigrateDatabase<T>(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DatingAppDbContext>();
            var logger = scope.ServiceProvider.GetService<ILogger<T>>();

            try
            {
                logger?.LogInformation("Trying to migrate database...");

                await dbContext.Database.MigrateAsync();
            }
            catch (Exception exc)
            {
                logger?.LogError(exc, "Error while attempting to migrate database!");
            }

            return host;
        }


        public async static Task<IHost> SeedDatabase<T>(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DatingAppDbContext>();
            var logger = scope.ServiceProvider.GetService<ILogger<T>>();
            var hashProvider = scope.ServiceProvider.GetRequiredService<IHashProvider>();

            try
            {
                if (!await dbContext.Users.AnyAsync())
                {
                    logger?.LogInformation("Trying to seed database with user data...");

                    var usersJson = await File.ReadAllTextAsync($"{Environment.CurrentDirectory}/Data/UserSeedData.json");
                    var users = JsonSerializer.Deserialize<List<AppUser>>(usersJson);

                    var salt = default(byte[]);
                    users.ForEach(u => {
                        u.PasswordHash = hashProvider.ComputeUtf8EncodedHash("D3f@ulT01!", out salt);
                        u.PasswordSalt = salt;
                    });

                    await dbContext.Users.AddRangeAsync(users);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception exc)
            {
                logger?.LogError(exc, "Error while attempting to seed database with user data!");
            }

            return host;
        }
    }
}