namespace Drinkful.Contracts.Drinks; 

public record DrinkResponse(
  string Id,
  string Name,
  string Summary,
  string Description,
  string ImageUrl,
  DateTime CreatedAt,
  DateTime UpdatedAt,
  string AuthorId,
  List<string> CommentIds);