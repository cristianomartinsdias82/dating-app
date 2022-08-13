using DatingApp.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Api.Data
{
    public class DatingAppDbContext : DbContext
    {
        public DatingAppDbContext(DbContextOptions<DatingAppDbContext> options) : base(options) { }

        public DbSet<AppUser> Users { get; set; }
    }
}