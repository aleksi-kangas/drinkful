using Drinkful.Application.Authentication.Queries;
using FluentValidation.TestHelper;

namespace Drinkful.Application.UnitTests.Systems.Authentication.Queries;

public class TestLoginQueryValidator {
  private readonly LoginQueryValidator _validator = new();

  [Theory]
  [InlineData("", "password")]
  [InlineData("", "")]
  public void LoginQuery_ShouldHaveError_WhenEmailIsEmpty(string email, string password) {
    // Arrange
    var loginQuery = new LoginQuery(email, password);
    // Act
    var result = _validator.TestValidate(loginQuery);
    // Assert
    result
      .ShouldHaveValidationErrorFor(lq => lq.Email)
      .WithErrorCode("NotEmptyValidator");
  }

  [Theory]
  [InlineData("user@example.com", "")]
  [InlineData("", "")]
  public void LoginQuery_ShouldHaveError_WhenPasswordIsEmpty(string email, string password) {
    // Arrange
    var loginQuery = new LoginQuery(email, password);
    // Act
    var result = _validator.TestValidate(loginQuery);
    // Assert
    result
      .ShouldHaveValidationErrorFor(lq => lq.Password)
      .WithErrorCode("NotEmptyValidator");
  }

  [Theory]
  [InlineData("user@example.com", "password")]
  [InlineData("somebody@gmail.com", "123456")]
  public void LoginQuery_ShouldNotHaveAnyErrors_WhenEmailAndPasswordAreNonEmpty(
    string email,
    string password) {
    // Arrange
    var loginQuery = new LoginQuery(email, password);
    // Act
    var result = _validator.TestValidate(loginQuery);
    // Assert
    result.ShouldNotHaveAnyValidationErrors();
  }
}
