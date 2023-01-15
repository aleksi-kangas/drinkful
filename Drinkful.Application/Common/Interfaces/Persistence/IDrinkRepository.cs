using Drinkful.Domain.Drink;
using Drinkful.Domain.Drink.ValueObjects;

namespace Drinkful.Application.Common.Interfaces.Persistence;

public interface IDrinkRepository : IRepositoryBase<Drink> {
  void Create(Drink drink);

  Task<Drink?> GetByIdAsync(DrinkId drinkId, bool trackChanges);

  Task<IEnumerable<Drink>> GetAllAsync();
}
