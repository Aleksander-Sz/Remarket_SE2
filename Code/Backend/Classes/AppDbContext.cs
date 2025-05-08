using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReMarket.Models;


namespace ReMarket.Services
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Listing> Listings { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Description> Descriptions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ensure your Listing entity maps to the 'listing' table in the database
            modelBuilder.Entity<Listing>()
                .ToTable("listing");  // Specify the exact table name (singular 'listing')
            modelBuilder.Entity<Category>()
                .ToTable("category");
            modelBuilder.Entity<Description>()
                .ToTable("description");
        }
    }
}
