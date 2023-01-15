using Drinkful.Application.Common.Interfaces.Persistence;
using Drinkful.Domain.Common.Errors;
using Drinkful.Domain.Drink;
using Drinkful.Domain.Drink.ValueObjects;
using ErrorOr;
using MediatR;

namespace Drinkful.Application.Drinks.Queries.GetDrink;

public class GetDrinkQueryHandler : IRequestHandler<GetDrinkQuery, ErrorOr<Drink>> {
  private readonly IDrinkRepository _drinkRepository;

  public GetDrinkQueryHandler(IDrinkRepository drinkRepository) {
    _drinkRepository = drinkRepository;
  }

  public async Task<ErrorOr<Drink>> Handle(GetDrinkQuery request,
    CancellationToken cancellationToken) {
    var drink = await _drinkRepository.GetByIdAsync(DrinkId.Create(request.Id), false);
    if (drink is null) {
      return Errors.Drink.NotFound;
    }

    return drink;
  }
}
