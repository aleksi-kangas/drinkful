using Drinkful.Domain.Common;
using Drinkful.Domain.Common.Models;

namespace Drinkful.Domain.Drink.ValueObjects;

public sealed class DrinkId : ValueObject {
  public Guid Value { get; }
  private DrinkId(Guid value) => Value = value;
  public static DrinkId CreateUnique() => new(Guid.NewGuid());

  protected override IEnumerable<object> GetEqualityComponents() {
    yield return Value;
  }
}