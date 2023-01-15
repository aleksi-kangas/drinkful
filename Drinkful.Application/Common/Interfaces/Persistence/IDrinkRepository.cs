using Drinkful.Domain.Drink;
using Drinkful.Domain.Drink.ValueObjects;

namespace Drinkful.Application.Common.Interfaces.Persistence;

public interface IDrinkRepository {
  void Create(Drink drink);
  
  Drink? GetById(DrinkId drinkId);
}
