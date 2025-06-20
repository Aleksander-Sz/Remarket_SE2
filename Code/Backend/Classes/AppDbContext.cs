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
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<ListingPhoto> ListingPhotos {  get; set; }
        public DbSet<Review> Reviews { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<Listing>()
                .ToTable("listing");
            modelBuilder.Entity<Category>()
                .ToTable("category");
            modelBuilder.Entity<Description>()
                .ToTable("description");
            modelBuilder.Entity<Account>()
                .ToTable("account");
            modelBuilder.Entity<Photo>()
                .ToTable("photo");
            modelBuilder.Entity<ListingPhoto>()
                .ToTable("listingphoto")
                .HasKey(lp => new { lp.ListingId, lp.PhotoId });

            modelBuilder.Entity<ListingPhoto>()
                .HasOne(lp => lp.Listing)
                .WithMany(l => l.ListingPhotos)
                .HasForeignKey(lp => lp.ListingId);

            modelBuilder.Entity<ListingPhoto>()
                .HasOne(lp => lp.Photo)
                .WithMany(p => p.ListingPhotos)
                .HasForeignKey(lp => lp.PhotoId);

            modelBuilder.Entity<Review>()
                .ToTable("review")
                .HasKey(r => r.Id);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Account)
                .WithMany() // or `.WithMany(a => a.Reviews)` if you define a `List<Review>` in `Account`
                .HasForeignKey(r => r.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Listing)
                .WithMany() // or `.WithMany(l => l.Reviews)` if you define a `List<Review>` in `Listing`
                .HasForeignKey(r => r.ListingId)
                .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
