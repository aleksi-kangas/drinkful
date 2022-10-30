using Drinkful.Application.Common.Interfaces.Authentication;
using Drinkful.Application.Common.Interfaces.Persistence;
using Drinkful.Domain.Common.Errors;
using Drinkful.Domain.Entities;
using ErrorOr;
using Microsoft.AspNetCore.Identity;

namespace Drinkful.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService {
  private readonly IJwtGenerator _jwtGenerator;
  private readonly IUserRepository _userRepository;

  public AuthenticationService(IJwtGenerator jwtGenerator, IUserRepository userRepository) {
    _jwtGenerator = jwtGenerator;
    _userRepository = userRepository;
  }

  public ErrorOr<AuthenticationResult> Login(string email, string password) {
    var user = _userRepository.GetByEmail(email);
    if (user is null) {
      return Errors.Authentication.InvalidCredentials;
    }

    var passwordHasher = new PasswordHasher<string>();
    var result = passwordHasher.VerifyHashedPassword(user.Username, user.PasswordHash, password);
    if (result == PasswordVerificationResult.Failed) {
      return Errors.Authentication.InvalidCredentials;
    }

    var token = _jwtGenerator.GenerateToken(user);
    return new AuthenticationResult(user, token);
  }

  public ErrorOr<AuthenticationResult> Register(string username, string email, string password) {
    if (_userRepository.GetByEmail(email) is not null) {
      return Errors.Authentication.DuplicateEmail;
    }

    if (_userRepository.GetByUsername(username) is not null) {
      return Errors.Authentication.DuplicateUsername;
    }

    var passwordHasher = new PasswordHasher<string>();
    var passwordHash = passwordHasher.HashPassword(username, password);
    var user = new User { Username = username, Email = email, PasswordHash = passwordHash };
    _userRepository.Add(user);
    var token = _jwtGenerator.GenerateToken(user);
    return new AuthenticationResult(user, token);
  }
}