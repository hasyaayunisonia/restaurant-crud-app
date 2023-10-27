using ASPNETCoreRestaurantApplication.Models;
using Microsoft.EntityFrameworkCore;
public class FoodContext : DbContext {
        public FoodContext(DbContextOptions<FoodContext> options) : base(options)
        {
        }

        public DbSet<Food> Foods { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Food>().HasKey(c => c.food_id);
        }
}

