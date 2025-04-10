using Microsoft.EntityFrameworkCore;
using Remarket_SE2.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
        ;
    }
    public DbSet<Account> Accounts { get; set; }
}