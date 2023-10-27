
using ASPNETCoreRestaurantApplication.Models;
using Microsoft.EntityFrameworkCore;

public class TransactionContext : DbContext
{
    public TransactionContext(DbContextOptions<TransactionContext> options) : base(options)
    {
    }

    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<Transaction>().HasKey(c => c.food_id);
        modelBuilder.Entity<Transaction>()
             .HasOne(t => t.Customer)
             .WithMany(c => c.Transactions)
             .HasForeignKey(t => t.customer_id);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Food)
            .WithMany(f => f.Transactions)
            .HasForeignKey(t => t.food_id);
    }
}

