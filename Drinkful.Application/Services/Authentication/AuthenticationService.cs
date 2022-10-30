using Drinkful.Application.Common.Interfaces.Authentication;
using Drinkful.Application.Common.Interfaces.Persistence;
using Drinkful.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Drinkful.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService {
  private readonly IJwtGenerator _jwtGenerator;
  private readonly IUserRepository _userRepository;

  public AuthenticationService(IJwtGenerator jwtGenerator, IUserRepository userRepository) {
    _jwtGenerator = jwtGenerator;
    _userRepository = userRepository;
  }

  public AuthenticationResult Login(string email, string password) {
    var user = _userRepository.GetByEmail(email);
    if (user is null) {
      throw new Exception("User with the given email does not exist.");
    }

    var passwordHasher = new PasswordHasher<string>();
    var result = passwordHasher.VerifyHashedPassword(user.Username, user.PasswordHash, password);
    if (result == PasswordVerificationResult.Failed) {
      throw new Exception("Invalid password.");
    }

    var token = _jwtGenerator.GenerateToken(user);
    return new AuthenticationResult(user, token);
  }

  public AuthenticationResult Register(string username, string email, string password) {
    if (_userRepository.GetByEmail(email) is not null) {
      throw new Exception("User with the given email already exists.");
    }

    if (_userRepository.GetByUsername(username) is not null) {
      throw new Exception("User with the given username already exists.");
    }

    var passwordHasher = new PasswordHasher<string>();
    var passwordHash = passwordHasher.HashPassword(username, password);
    var user = new User { Username = username, Email = email, PasswordHash = passwordHash };
    _userRepository.Add(user);
    var token = _jwtGenerator.GenerateToken(user);
    return new AuthenticationResult(user, token);
  }
}