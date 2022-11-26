using Drinkful.Application.Common.Interfaces.Persistence;
using Drinkful.Domain.Drink;
using Drinkful.Domain.User.ValueObjects;
using ErrorOr;
using MediatR;

namespace Drinkful.Application.Drinks.Commands.CreateDrink;

public class CreateDrinkCommandHandler : IRequestHandler<CreateDrinkCommand, ErrorOr<Drink>> {
  private readonly IDrinkRepository _drinkRepository;

  public CreateDrinkCommandHandler(IDrinkRepository drinkRepository) {
    _drinkRepository = drinkRepository;
  }

  public async Task<ErrorOr<Drink>> Handle(CreateDrinkCommand command,
    CancellationToken cancellationToken) {
    // TODO Temporary implementation
    await Task.CompletedTask;

    var drink = Drink.Create(
      name: command.Name,
      summary: command.Summary,
      description: command.Description,
      imageUrl: command.ImageUrl,
      authorId: UserId.Create(command.AuthorId));
    _drinkRepository.Create(drink);
    return drink;
  }
}
