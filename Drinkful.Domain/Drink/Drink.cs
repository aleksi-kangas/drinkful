using Drinkful.Domain.Comment.ValueObjects;
using Drinkful.Domain.Common.Models;
using Drinkful.Domain.Drink.ValueObjects;
using Drinkful.Domain.User.ValueObjects;

namespace Drinkful.Domain.Drink;

public class Drink : AggregateRoot<DrinkId> {
  public string Name { get; private set; }
  public string Summary { get; private set; }
  public string Description { get; private set; }
  public string ImageUrl { get; private set; }
  public DateTime CreatedAt { get; private set; }
  public DateTime UpdatedAt { get; private set; }
  public UserId AuthorId { get; private set; }
  private readonly List<CommentId> _commentIds = new();
  public IReadOnlyList<CommentId> CommentIds => _commentIds.AsReadOnly();

#pragma warning disable CS8618
  private Drink() { } // For EF-Core
#pragma warning restore CS8618

  private Drink(DrinkId drinkId, string name, string summary, string description, string imageUrl,
    DateTime createdAt, DateTime updatedAt, UserId authorId) : base(drinkId) {
    Name = name;
    Summary = summary;
    Description = description;
    ImageUrl = imageUrl;
    CreatedAt = createdAt;
    UpdatedAt = updatedAt;
    AuthorId = authorId;
  }

  public static Drink Create(
    string name, string summary, string description, string imageUrl, UserId authorId) {
    return new Drink(
      DrinkId.CreateUnique(), name, summary, description, imageUrl, DateTime.UtcNow,
      DateTime.UtcNow, authorId);
  }
}
