﻿namespace Drinkful.Domain.Common.Models;

public abstract class ValueObject : IEquatable<ValueObject> {
  protected abstract IEnumerable<object> GetEqualityComponents();

  public bool Equals(ValueObject? other) {
    return Equals((object?) other);
  }

  public override bool Equals(object? obj) {
    if (obj == null || obj.GetType() != GetType()) {
      return false;
    }

    var other = (ValueObject) obj;
    return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
  }

  public static bool operator ==(ValueObject lhs, ValueObject rhs) {
    return Equals(lhs, rhs);
  }

  public static bool operator !=(ValueObject lhs, ValueObject rhs) {
    return !Equals(lhs, rhs);
  }

  public override int GetHashCode() {
    return GetEqualityComponents()
      .Select(x => x?.GetHashCode() ?? 0)
      .Aggregate((x, y) => x ^ y);
  }
}
