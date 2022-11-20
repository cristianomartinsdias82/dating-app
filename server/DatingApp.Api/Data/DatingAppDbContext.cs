using DatingApp.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Api.Data
{
    public class DatingAppDbContext : DbContext
    {
        public DatingAppDbContext(DbContextOptions<DatingAppDbContext> options) : base(options) { }

        public DbSet<AppUser> Users { get; set; }

        public DbSet<Photo> Photos { get; set; }

        public DbSet<UserLike> UserLike { get; set; }

        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //     modelBuilder
            //         .Entity<AppUser>()
            //         .Ignore(x => x.Age);

            //Likes MANY-TO-MANY RELATIONSHIP!
            modelBuilder.Entity<UserLike>()
                        .HasKey(x => new { x.LikerPersonId, x.LikedByPersonId }); //Composite key!

            modelBuilder.Entity<UserLike>()
                        .HasOne(x => x.LikerPerson)
                        .WithMany(x => x.LikedUsers)
                        .HasForeignKey(x => x.LikerPersonId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserLike>()
                        .HasOne(x => x.LikedByPerson)
                        .WithMany(x => x.LikedByUsers)
                        .HasForeignKey(x => x.LikedByPersonId)
                        .OnDelete(DeleteBehavior.Cascade);

            //Messages ONE-TO-MANY RELATIONSHIP!
            modelBuilder.Entity<Message>()
                        .HasOne(x => x.Sender)
                        .WithMany(x => x.SentMessages)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                        .HasOne(x => x.Recipient)
                        .WithMany(x => x.ReceivedMessages)
                        .OnDelete(DeleteBehavior.Restrict);
        }
    }
}