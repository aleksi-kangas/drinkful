using ErrorOr;

namespace Drinkful.Domain.Common.Errors; 

public static partial class Errors {
  public static class Authentication {
    public static Error DuplicateEmail = Error.Conflict(
      code: "Authentication.DuplicateEmail",
      description: "Email is already in use.");
    public static Error DuplicateUsername = Error.Conflict(
      code: "Authentication.DuplicateUsername",
      description: "Username is already in use.");
    public static Error InvalidCredentials = Error.Validation(
      code: "Authentication.InvalidCredentials",
      description: "Invalid credentials.");
  }
}