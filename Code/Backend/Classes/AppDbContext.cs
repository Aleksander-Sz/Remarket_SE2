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
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<OrderListing> OrderListings { get; set; }
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
                .WithMany() 
                .HasForeignKey(r => r.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Listing)
                .WithMany() 
                .HasForeignKey(r => r.ListingId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .ToTable("order")
                .HasKey(o => o.Id);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Seller)          
                .WithMany()                    
                .HasForeignKey(o => o.SellerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Buyer)           
                .WithMany()                    
                .HasForeignKey(o => o.BuyerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Payment>()
                .ToTable("payment")
                .HasKey(p => p.Id);

            modelBuilder.Entity<Payment>()
                .Property(p => p.PaidOn)
                .HasColumnName("paidOn");

            modelBuilder.Entity<Payment>()
                .Property(p => p.Total)
                .HasColumnName("total");

            modelBuilder.Entity<Payment>()
                .Property(p => p.AccountId)
                .HasColumnName("accountId");

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Account)
                .WithMany() 
                .HasForeignKey(p => p.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderListing>()
                .ToTable("orderlisting")
                .HasKey(ol => new { ol.OrderId, ol.ListingId });

            modelBuilder.Entity<OrderListing>()
                .HasOne(ol => ol.Order)
                .WithMany()
                .HasForeignKey(ol => ol.OrderId);

            modelBuilder.Entity<OrderListing>()
                .HasOne(ol => ol.Listing)
                .WithMany()
                .HasForeignKey(ol => ol.ListingId);

        }
    }
}
