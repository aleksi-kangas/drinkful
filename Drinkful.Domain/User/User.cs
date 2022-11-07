using Drinkful.Domain.Common.Models;
using Drinkful.Domain.User.ValueObjects;

namespace Drinkful.Domain.User;

public class User : AggregateRoot<UserId> {
  public string Username { get; set; }
  public string Email { get; set; }
  public string PasswordHash { get; set; }

  private User(UserId userId, string username, string email, string passwordHash) : base(userId) {
    Username = username;
    Email = email;
    PasswordHash = passwordHash;
  }

  public static User Create(string username, string email, string passwordHash) {
    return new User(UserId.CreateUnique(), username, email, passwordHash);
  }
}