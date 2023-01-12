using Drinkful.Domain.Common.Models;

namespace Drinkful.Domain.Drink.ValueObjects;

public sealed class DrinkId : ValueObject {
  public Guid Value { get; }
  private DrinkId(Guid value) => Value = value;
  public static DrinkId CreateUnique() => new(Guid.NewGuid());
  public static DrinkId Create(Guid value) => new(value);

  protected override IEnumerable<object> GetEqualityComponents() {
    yield return Value;
  }
}
