using Drinkful.Application.Common.Interfaces.Persistence;
using Drinkful.Domain.Entities;

namespace Drinkful.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService {
  private readonly IUserRepository _userRepository;

  public AuthenticationService(IUserRepository userRepository) {
    _userRepository = userRepository;
  }

  public AuthenticationResult Login(string email, string password) {
    var user = _userRepository.GetByEmail(email);
    if (user is null) {
      throw new Exception("User with the given email does not exist.");
    }

    // TODO Proper password hashing comparison
    if (user.PasswordHash != password) {
      throw new Exception("Invalid password.");
    }

    // TODO JWT generation
    return new AuthenticationResult(user, "token");
  }

  public AuthenticationResult Register(string username, string email, string password) {
    if (_userRepository.GetByEmail(email) is not null) {
      throw new Exception("User with the given email already exists.");
    }

    // TODO Proper password hashing
    var user = new User { Username = username, Email = email, PasswordHash = password };
    // TODO JWT generation
    return new AuthenticationResult(user, "token");
  }
}