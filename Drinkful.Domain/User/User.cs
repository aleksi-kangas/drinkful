using Drinkful.Domain.Common.Models;
using Drinkful.Domain.Drink.ValueObjects;
using Drinkful.Domain.User.ValueObjects;

namespace Drinkful.Domain.User;

public class User : AggregateRoot<UserId> {
  public string Username { get; set; }
  public string Email { get; set; }
  public string PasswordHash { get; set; }
  private readonly List<DrinkId> _drinkIds = new();
  public IReadOnlyCollection<DrinkId> DrinkIds => _drinkIds.AsReadOnly();

  private User(UserId userId, string username, string email, string passwordHash) : base(userId) {
    Username = username;
    Email = email;
    PasswordHash = passwordHash;
  }

  public static User Create(string username, string email, string passwordHash) {
    return new User(UserId.CreateUnique(), username, email, passwordHash);
  }
}
