using Drinkful.Domain.Common;
using Drinkful.Domain.Common.Models;

namespace Drinkful.Domain.Comment.ValueObjects;

public sealed class CommentId : ValueObject, ILikeableId {
  public Guid Value { get; }
  private CommentId(Guid value) => Value = value;
  public static CommentId CreateUnique() => new(Guid.NewGuid());

  protected override IEnumerable<object> GetEqualityComponents() {
    yield return Value;
  }
}