using Drinkful.Domain.Drink;
using Microsoft.EntityFrameworkCore;

namespace Drinkful.Infrastructure.Persistence; 

public class DrinkfulDbContext : DbContext {
  public DrinkfulDbContext(DbContextOptions<DrinkfulDbContext> options) : base(options) { }

  public DbSet<Drink> Drinks { get; set; } = null!;

  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(DrinkfulDbContext).Assembly);
  }
}
