using Drinkful.Domain.Drink;
using ErrorOr;
using MediatR;

namespace Drinkful.Application.Drinks.Queries.GetDrink; 

public record GetDrinkQuery(Guid Id) : IRequest<ErrorOr<Drink>>;
