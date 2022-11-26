using Drinkful.Domain.Drink;

namespace Drinkful.Application.Common.Interfaces.Persistence;

public interface IDrinkRepository {
  void Create(Drink drink);
}
