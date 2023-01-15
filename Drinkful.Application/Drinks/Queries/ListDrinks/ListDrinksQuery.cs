using Drinkful.Domain.Drink;
using MediatR;

namespace Drinkful.Application.Drinks.Queries.ListDrinks;

public record ListDrinksQuery() : IRequest<IEnumerable<Drink>>;
