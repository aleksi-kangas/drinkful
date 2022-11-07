using Drinkful.Application.Common.Interfaces.Persistence;
using Drinkful.Domain.User;

namespace Drinkful.Infrastructure.Persistence; 

public class UserRepository : IUserRepository {
  private static readonly List<User> Users = new(); 
  
  public void Create(User user) {
    Users.Add(user);
  }

  public User? GetByEmail(string email) {
    return Users.SingleOrDefault(user => user.Email == email);
  }

  public User? GetByUsername(string username) {
    return Users.SingleOrDefault(user => user.Username == username);
  }
}