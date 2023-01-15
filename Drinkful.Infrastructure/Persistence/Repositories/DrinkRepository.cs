using Drinkful.Application.Common.Interfaces.Persistence;
using Drinkful.Domain.Drink;
using Drinkful.Domain.Drink.ValueObjects;

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

  public Drink? GetById(DrinkId drinkId) {
    return _dbContext.Drinks.Find(drinkId);
  }
  
  public IEnumerable<Drink> GetAll() {
    return _dbContext.Drinks;
  }
}
