using Drinkful.Application.Common.Interfaces.Persistence;
using Drinkful.Domain.Drink;

namespace Drinkful.Infrastructure.Persistence;

public class DrinkRepository : IDrinkRepository {
  private static readonly List<Drink> Drinks = new();

  public void Create(Drink drink) {
    Drinks.Add(drink);
  }
}
