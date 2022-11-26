using Drinkful.Application.Authentication.Commands;
using FluentValidation.TestHelper;

namespace Drinkful.Application.UnitTests.Systems.Authentication.Commands;

public class TestRegisterCommandValidator {
  private readonly RegisterCommandValidator _validator = new();

  [Theory]
  [InlineData("", "user@example.com", "password")]
  [InlineData("", "", "")]
  public void RegisterCommand_ShouldHaveError_WhenUsernameIsEmpty(
    string username,
    string email,
    string password) {
    // Arrange
    var registerCommand = new RegisterCommand(username, email, password);
    // Act
    var result = _validator.TestValidate(registerCommand);
    // Assert
    result
      .ShouldHaveValidationErrorFor(rc => rc.Username)
      .WithErrorCode("NotEmptyValidator");
  }

  [Theory]
  [InlineData("username", "", "password")]
  [InlineData("", "", "")]
  public void RegisterCommand_ShouldHaveError_WhenEmailIsEmpty(
    string username,
    string email,
    string password) {
    // Arrange
    var registerCommand = new RegisterCommand(username, email, password);
    // Act
    var result = _validator.TestValidate(registerCommand);
    // Assert
    result
      .ShouldHaveValidationErrorFor(rc => rc.Email)
      .WithErrorCode("NotEmptyValidator");
  }

  [Theory]
  [InlineData("username", "user@example.com", "")]
  [InlineData("", "", "")]
  public void RegisterCommand_ShouldHaveError_WhenPasswordIsEmpty(
    string username,
    string email,
    string password) {
    // Arrange
    var registerCommand = new RegisterCommand(username, email, password);
    // Act
    var result = _validator.TestValidate(registerCommand);
    // Assert
    result
      .ShouldHaveValidationErrorFor(rc => rc.Password)
      .WithErrorCode("NotEmptyValidator");
  }

  [Theory]
  [InlineData("username", "user@example.com", "password")]
  [InlineData("somebody", "somebody@gmail.com", "123456")]
  public void RegisterCommand_ShouldNotHaveAnyErrors_WhenUsernameEmailAndPasswordAreNonEmpty(
    string username,
    string email,
    string password) {
    // Arrange
    var loginQuery = new RegisterCommand(username, email, password);
    // Act
    var result = _validator.TestValidate(loginQuery);
    // Assert
    result.ShouldNotHaveAnyValidationErrors();
  }
}
