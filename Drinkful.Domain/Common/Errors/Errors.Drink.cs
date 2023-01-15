using ErrorOr;

namespace Drinkful.Domain.Common.Errors;

public static partial class Errors {
  public static class Drink {
    public static Error NotFound = Error.NotFound(
      code: "Drink.NotFound",
      description: "Drink not found");
  }
}
