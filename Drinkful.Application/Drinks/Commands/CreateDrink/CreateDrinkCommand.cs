using Drinkful.Domain.Drink;
using ErrorOr;
using MediatR;

namespace Drinkful.Application.Drinks.Commands.CreateDrink;

public record CreateDrinkCommand(
  string Name,
  string Summary,
  string Description,
  string ImageUrl,
  string AuthorId) : IRequest<ErrorOr<Drink>>;
