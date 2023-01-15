using System.Linq.Expressions;

namespace Drinkful.Application.Common.Interfaces.Persistence; 

public interface IRepositoryBase<T> {
  IQueryable<T> FindAll(bool trackChanges);
  IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges);
  void Add(T entity);
  void Update(T entity);
  void Delete(T entity);
  
  // TODO Some sort of a repository manager?
  Task SaveAsync();
}
