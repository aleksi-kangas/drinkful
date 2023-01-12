namespace Drinkful.Domain.Common.Models;

public abstract class AggregateRoot<TId> : Entity<TId> where TId : notnull {
  protected AggregateRoot() { } // For EF-Core
  protected AggregateRoot(TId id) : base(id) { }
}
