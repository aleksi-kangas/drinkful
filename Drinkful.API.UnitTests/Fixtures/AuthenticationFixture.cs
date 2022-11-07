using Drinkful.Application.Authentication.Common;
using Drinkful.Contracts.Authentication;
using Drinkful.Domain.User;
using ErrorOr;
using Microsoft.AspNetCore.Identity;

namespace Drinkful.API.UnitTests.Fixtures;

public static class AuthenticationFixture {
  public class TestUser {
    public User User { get; }
    public string Username => User.Username;
    public string Email => User.Email;
    public string PasswordHash => User.PasswordHash;
    public string Password { get; }

    public TestUser(string username, string email, string password) {
      User = User.Create(
        username,
        email,
        new PasswordHasher<string>().HashPassword(username, password));
      Password = password;
    }
  }

  public static ErrorOr<AuthenticationResult> GetAuthenticationResultSuccess(TestUser testUser) {
    return new AuthenticationResult(testUser.User, "token");
  }

  public static ErrorOr<AuthenticationResult> GetAuthenticationResultFailure(Error error) {
    return error;
  }

  public static AuthenticationResponse GetAuthenticationResponseSuccess(TestUser testUser) {
    return new AuthenticationResponse(testUser.User.Id.Value, testUser.Username, testUser.Email,
      "token");
  }
}