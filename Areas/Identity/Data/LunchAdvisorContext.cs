using LunchAdvisor.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LunchAdvisor.Models;

namespace LunchAdvisor.Data;

public class LunchAdvisorContext : IdentityDbContext<LunchAdvisorUser>
{
    public DbSet<LunchAdvisor.Models.Restaurant> Restaurant { get; set; }

    public DbSet<LunchAdvisor.Models.RestaurantRating> RestaurantRating { get; set; }

    public DbSet<LunchAdvisor.Models.Dish> Dish { get; set; }

    public DbSet<LunchAdvisor.Models.DishRating> DishRating { get; set; }

    public DbSet<LunchAdvisor.Models.Waiter> Waiter { get; set; }

    public DbSet<LunchAdvisor.Models.WaiterRating> WaiterRating { get; set; }

    public LunchAdvisorContext(DbContextOptions<LunchAdvisorContext> options)
        : base(options)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
    }
    

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        builder.Entity<RestaurantRating>()
            .HasOne(rr => rr.Restaurant)
            .WithMany(r => r.RestaurantRatings)
            .HasForeignKey(rr => rr.RestaurantID);

        builder.Entity<WaiterRating>()
            .HasOne(rr => rr.Waiter)
            .WithMany(r => r.WaiterRatings)
            .HasForeignKey(rr => rr.WaiterID);

        builder.Entity<DishRating>()
            .HasOne(rr => rr.Dish)
            .WithMany(r => r.DishRatings)
            .HasForeignKey(rr => rr.DishID);

        //builder.Entity<Restaurant>().HasMany(r => r.RestaurantRatings).WithOne(rr => rr.Restaurant).IsRequired();

    }
}
