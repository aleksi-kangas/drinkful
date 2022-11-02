using Drinkful.Application.Authentication.Common;
using Drinkful.Contracts.Authentication;
using Drinkful.Domain.Entities;
using ErrorOr;

namespace Drinkful.API.UnitTests.Fixtures;

public static class AuthenticationFixture {
  public class TestUser : User {
    public string Password { get; set; } = null!;
  }

  public static ErrorOr<AuthenticationResult> GetAuthenticationResultSuccess(TestUser user) {
    return new AuthenticationResult(user, "token");
  }
  
  public static ErrorOr<AuthenticationResult> GetAuthenticationResultFailure(Error error) {
    return error;
  }
  
  public static AuthenticationResponse GetAuthenticationResponseSuccess(TestUser user) {
    return new AuthenticationResponse(user.Id, user.Username, user.Email, "token");
  }

  public static TestUser GetTestUser() {
    return new TestUser {
      Id = Guid.NewGuid(),
      Email = "user@example.com",
      Password = "password",
      PasswordHash = "passwordHash",
      Username = "username"
    };
  }
}