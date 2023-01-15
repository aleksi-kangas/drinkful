using Drinkful.Application.Common.Interfaces.Persistence;
using Drinkful.Domain.Drink;
using MediatR;

namespace Drinkful.Application.Drinks.Queries.ListDrinks;

public class ListDrinksQueryHandler : IRequestHandler<ListDrinksQuery, IEnumerable<Drink>> {
  private readonly IDrinkRepository _drinkRepository;

  public ListDrinksQueryHandler(IDrinkRepository drinkRepository) {
    _drinkRepository = drinkRepository;
  }

  public async Task<IEnumerable<Drink>> Handle(ListDrinksQuery request,
    CancellationToken cancellationToken) {
    return await _drinkRepository.GetAllAsync();
  }
}
