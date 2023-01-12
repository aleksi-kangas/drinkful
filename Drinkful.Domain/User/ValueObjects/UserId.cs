using Drinkful.Domain.Common.Models;

namespace Drinkful.Domain.User.ValueObjects;

public sealed class UserId : ValueObject {
  public Guid Value { get; }
  private UserId(Guid value) => Value = value;

  public static UserId Create(string id) => new(Guid.Parse(id));
  public static UserId CreateUnique() => new(Guid.NewGuid());
  public static UserId Create(Guid value) => new(value);

  protected override IEnumerable<object> GetEqualityComponents() {
    yield return Value;
  }
}
