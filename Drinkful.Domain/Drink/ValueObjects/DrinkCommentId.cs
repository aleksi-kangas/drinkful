using Drinkful.Domain.Common.Models;

namespace Drinkful.Domain.Drink.ValueObjects; 

public sealed class DrinkCommentId : ValueObject {
  public Guid Value { get; }
  private DrinkCommentId(Guid value) => Value = value;
  public static DrinkCommentId CreateUnique() => new(Guid.NewGuid());
  public static DrinkCommentId Create(Guid value) => new(value);

  protected override IEnumerable<object> GetEqualityComponents() {
    yield return Value;
  }
}
