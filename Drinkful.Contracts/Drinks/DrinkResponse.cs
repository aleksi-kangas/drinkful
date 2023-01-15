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
  List<DrinkCommentResponse> Comments);

public record DrinkCommentResponse(
  string Id,
  string Content,
  string AuthorId,
  DateTime CreatedAt,
  DateTime UpdatedAt);
