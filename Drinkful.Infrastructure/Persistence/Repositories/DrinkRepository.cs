using Drinkful.Application.Common.Interfaces.Persistence;
using Drinkful.Domain.Drink;

namespace Drinkful.Infrastructure.Persistence.Repositories;

public class DrinkRepository : IDrinkRepository {
  private readonly DrinkfulDbContext _dbContext;

  public DrinkRepository(DrinkfulDbContext dbContext) {
    _dbContext = dbContext;
  }

  public void Create(Drink drink) {
    _dbContext.Drinks.Add(drink);
    _dbContext.SaveChanges();
  }
}
