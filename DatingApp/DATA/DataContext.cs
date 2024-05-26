using DatingApp.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using WebAPIDatingAPP.Entities;

namespace WebAPIDatingAPP.DATA
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions option) : base(option)
        {

        }
        public DbSet<AppUsers> AppUsers { get; set; }
        public DbSet<UserLike> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserLike>()
            .HasKey(k => new { k.SourceUserId, k.TargetUserId });

            modelBuilder.Entity<UserLike>()
                .HasOne(s => s.SourceUser)
                .WithMany(l => l.LikedUsers)
                .HasForeignKey(s => s.SourceUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserLike>()
            .HasOne(s => s.TargetUser)
            .WithMany(l => l.LikedByUsers)
            .HasForeignKey(s => s.TargetUserId)
            .OnDelete(DeleteBehavior.NoAction);


        }

    }
}
