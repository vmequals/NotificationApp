using Microsoft.EntityFrameworkCore;
using NotificationApp.Models;

namespace NotificationApp.Data
{
    public class NotificationDbContext : DbContext
    {
        public NotificationDbContext(DbContextOptions<NotificationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Notification> Notifications { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserNotification>(entity =>
            {
                entity.HasKey(un => new { un.UserId, un.NotificationId });

                entity.HasOne(un => un.User)
                    .WithMany(u => u.UserNotifications)
                    .HasForeignKey(un => un.UserId);

                entity.HasOne(un => un.Notification)
                    .WithMany(n => n.UserNotifications)
                    .HasForeignKey(un => un.NotificationId);
            });
        }
    }
}