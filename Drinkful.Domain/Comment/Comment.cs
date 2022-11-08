using Drinkful.Domain.Comment.ValueObjects;
using Drinkful.Domain.Common.Models;
using Drinkful.Domain.Like.ValueObjects;
using Drinkful.Domain.User.ValueObjects;

namespace Drinkful.Domain.Comment;

public class Comment : AggregateRoot<CommentId> {
  public string Content { get; private set; }
  public UserId AuthorId { get; private set; }
  public DateTime CreatedAt { get; private set; }
  public DateTime? UpdatedAt { get; private set; }
  private readonly List<LikeId> _likeIds = new();
  public IReadOnlyList<LikeId> LikeIds => _likeIds.AsReadOnly();

  private Comment(
    CommentId commentId, string content, UserId authorId, DateTime createdAt, DateTime? updatedAt) :
    base(commentId) {
    Content = content;
    AuthorId = authorId;
    CreatedAt = createdAt;
    UpdatedAt = updatedAt;
  }

  public static Comment Create(string content, UserId authorId) =>
    new(CommentId.CreateUnique(), content, authorId, DateTime.UtcNow, null);
}