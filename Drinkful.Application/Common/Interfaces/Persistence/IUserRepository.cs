using Drinkful.Domain.User;

namespace Drinkful.Application.Common.Interfaces.Persistence;

public interface IUserRepository : IRepositoryBase<User> {
  void Create(User user);
  Task<User?> GetByEmail(string email, bool trackChanges);
  Task<User?> GetByUsername(string username, bool trackChanges);
}
