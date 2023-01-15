using Drinkful.Domain.Common.Models;
using Drinkful.Domain.Drink.ValueObjects;
using Drinkful.Domain.User.ValueObjects;

namespace Drinkful.Domain.Drink.Entities;

public class DrinkComment : Entity<DrinkCommentId> {
  public string Content { get; private set; }
  public UserId AuthorId { get; private set; }
  public DateTime CreatedAt { get; private set; }
  public DateTime UpdatedAt { get; private set; }

  private DrinkComment(
    DrinkCommentId id,
    string content,
    UserId authorId,
    DateTime createdAt,
    DateTime updatedAt
  ) : base(id) {
    Content = content;
    AuthorId = authorId;
    CreatedAt = createdAt;
    UpdatedAt = updatedAt;
  }

  public static DrinkComment Create(string content, UserId authorId) {
    return new DrinkComment(
      DrinkCommentId.CreateUnique(),
      content,
      authorId,
      DateTime.UtcNow,
      DateTime.UtcNow);
  }
}
