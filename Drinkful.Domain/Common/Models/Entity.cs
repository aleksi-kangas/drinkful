namespace Drinkful.Domain.Common.Models;

public abstract class Entity<TId> : IEquatable<Entity<TId>> where TId : notnull {
  public TId Id { get; }

#pragma warning disable CS8618
  protected Entity() { } // For EF-Core
#pragma warning restore CS8618

  protected Entity(TId id) {
    Id = id;
  }

  public bool Equals(Entity<TId>? other) {
    return Equals((object?) other);
  }

  public override bool Equals(object? obj) {
    return obj is Entity<TId> entity && Id.Equals(entity.Id);
  }

  public static bool operator ==(Entity<TId> lhs, Entity<TId> rhs) {
    return Equals(lhs, rhs);
  }

  public static bool operator !=(Entity<TId> lhs, Entity<TId> rhs) {
    return !Equals(lhs, rhs);
  }

  public override int GetHashCode() {
    return Id.GetHashCode();
  }
}
