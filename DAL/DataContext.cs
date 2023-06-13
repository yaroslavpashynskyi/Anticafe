using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
            // Database.EnsureDeleted();
            // Database.EnsureCreated();

        }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Activity> Activities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Activity>()
                .HasOne(p => p.Hall)
                .WithMany(d => d.Activities)
                .HasForeignKey(p => p.HallId);


        }
    }

}