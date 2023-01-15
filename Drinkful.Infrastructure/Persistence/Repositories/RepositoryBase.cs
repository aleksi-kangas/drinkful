using System.Linq.Expressions;
using Drinkful.Application.Common.Interfaces.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Drinkful.Infrastructure.Persistence.Repositories;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class {
  protected DrinkfulDbContext DrinkfulDbContext { get; }

  protected RepositoryBase(DrinkfulDbContext drinkfulDbContext) {
    DrinkfulDbContext = drinkfulDbContext;
  }

  public IQueryable<T> FindAll(bool trackChanges) {
    return trackChanges
      ? DrinkfulDbContext.Set<T>().AsNoTracking()
      : DrinkfulDbContext.Set<T>();
  }

  public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) {
    return trackChanges
      ? DrinkfulDbContext.Set<T>().Where(expression).AsNoTracking()
      : DrinkfulDbContext.Set<T>().Where(expression);
  }

  public void Add(T entity) {
    DrinkfulDbContext.Set<T>().Add(entity);
  }

  public void Update(T entity) {
    DrinkfulDbContext.Set<T>().Update(entity);
  }

  public void Delete(T entity) {
    DrinkfulDbContext.Set<T>().Remove(entity);
  }

  public async Task SaveAsync() {
    await DrinkfulDbContext.SaveChangesAsync();
  }
}
