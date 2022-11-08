using Drinkful.Domain.Common.Models;

namespace Drinkful.Domain.Like.ValueObjects;

public sealed class LikeId : ValueObject {
  public Guid Value { get; }
  private LikeId(Guid value) => Value = value;
  public static LikeId CreateUnique() => new(Guid.NewGuid());

  protected override IEnumerable<object> GetEqualityComponents() {
    yield return Value;
  }
}