using EFdigitalLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace EFdigitalLibrary
{
    public class AppContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public AppContext()
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-I2SQ3KE\\SQLEXPRESS01;Initial Catalog=EFdigitalLibrary;User ID=SuperAdmin;Password=***;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .Property(u => u.Name)
                .HasColumnType("nvarchar(max) COLLATE Cyrillic_General_CI_AS");  

            modelBuilder.Entity<Book>()
                .Property(u => u.Name)
                .HasColumnType("nvarchar(max) COLLATE Cyrillic_General_CI_AS");  
        }
    }
}