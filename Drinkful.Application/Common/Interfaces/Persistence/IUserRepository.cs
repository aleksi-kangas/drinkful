using Drinkful.Domain.Entities;

namespace Drinkful.Application.Common.Interfaces.Persistence; 

public interface IUserRepository {
  void Add(User user);
  User? GetByEmail(string email);
  User? GetByUsername(string username);
}