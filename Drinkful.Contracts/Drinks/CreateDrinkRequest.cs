namespace Drinkful.Contracts.Drinks;

public record CreateDrinkRequest(
  string Name,
  string Summary,
  string Description,
  string ImageUrl,
  string AuthorId);