using Drinkful.Application.Authentication.Queries;
using Drinkful.Application.Common.Interfaces.Authentication;
using Drinkful.Application.Common.Interfaces.Persistence;
using Drinkful.Domain.Common.Errors;
using Drinkful.Domain.User;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Drinkful.Application.UnitTests.Systems.Authentication.Queries;

public class TestLoginQueryHandler {
  [Fact]
  public async void Login_WithValidCredentials_ReturnsToken() {
    // Arrange
    var user = User.Create(
      "username",
      "user@example.com",
      new PasswordHasher<string>().HashPassword("username", "password"));
    var mockUserRepository = new Mock<IUserRepository>();
    mockUserRepository
      .Setup(x => x.GetByEmail(user.Email, false))
      .Returns(Task.FromResult(user)!);
    var mockJwtGenerator = new Mock<IJwtGenerator>();
    mockJwtGenerator
      .Setup(x => x.GenerateToken(user))
      .Returns("token");
    var handler = new LoginQueryHandler(mockJwtGenerator.Object, mockUserRepository.Object);
    // Act
    var authResult = await handler.Handle(
      new LoginQuery(user.Email, "password"),
      new CancellationToken());
    // Assert
    authResult.IsError.Should().BeFalse();
    authResult.Value.Token.Should().NotBeNullOrEmpty();
    authResult.Value.Token.Should().Be("token");
  }

  [Fact]
  public async void Login_WithoutExistingUser_ReturnsInvalidCredentialsError() {
    // Arrange
    var mockUserRepository = new Mock<IUserRepository>();
    mockUserRepository
      .Setup(x => x.GetByEmail("user@example.com", false))
      .Returns(Task.FromResult(null as User));
    var mockJwtGenerator = new Mock<IJwtGenerator>();
    var handler = new LoginQueryHandler(mockJwtGenerator.Object, mockUserRepository.Object);
    // Act
    var authResult = await handler.Handle(
      new LoginQuery("user@example.com", "password"),
      new CancellationToken());
    // Assert
    authResult.IsError.Should().BeTrue();
    authResult.FirstError.Should().Be(Errors.Authentication.InvalidCredentials);
  }

  [Fact]
  public async void Login_WithInvalidPassword_ReturnsInvalidCredentialsError() {
    // Arrange
    var user = User.Create(
      "username",
      "user@example.com",
      new PasswordHasher<string>().HashPassword("username", "password"));
    var mockUserRepository = new Mock<IUserRepository>();
    mockUserRepository
      .Setup(x => x.GetByEmail("user@example.com", false))
      .Returns(Task.FromResult(user)!);
    var mockJwtGenerator = new Mock<IJwtGenerator>();
    var handler = new LoginQueryHandler(mockJwtGenerator.Object, mockUserRepository.Object);
    // Act
    var authResult = await handler.Handle(
      new LoginQuery("user@example.com", "invalid-password"),
      new CancellationToken());
    // Assert
    authResult.IsError.Should().BeTrue();
    authResult.FirstError.Should().Be(Errors.Authentication.InvalidCredentials);
  }
}
