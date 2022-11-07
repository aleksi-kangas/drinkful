using Drinkful.Application.Authentication.Commands;
using Drinkful.Application.Common.Interfaces.Authentication;
using Drinkful.Application.Common.Interfaces.Persistence;
using Drinkful.Domain.Common.Errors;
using Drinkful.Domain.User;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Drinkful.Application.UnitTests.Systems.Authentication.Commands;

public class TestRegisterCommandHandler {
  [Fact]
  public async void Register_WithValidUserInformation_ReturnsToken() {
    // Arrange
    var newUser = User.Create(
      "username", 
      "user@example.com",
      new PasswordHasher<string>().HashPassword("username", "password"));
    var mockUserRepository = new Mock<IUserRepository>();
    mockUserRepository
      .Setup(x => x.GetByEmail(newUser.Email))
      .Returns(null as User);
    var mockJwtGenerator = new Mock<IJwtGenerator>();
    mockJwtGenerator
      .Setup(x => x.GenerateToken(It.IsAny<User>()))
      .Returns("token");
    var handler = new RegisterCommandHandler(mockJwtGenerator.Object, mockUserRepository.Object);
    // Act
    var authResult = await handler.Handle(
      new RegisterCommand(newUser.Username, newUser.Email, "password"),
      new CancellationToken());
    // Assert
    authResult.IsError.Should().BeFalse();
    authResult.Value.Token.Should().NotBeNullOrEmpty();
    authResult.Value.Token.Should().Be("token");
  }

  [Fact]
  public async void Register_WithoutExistingUser_AddsUserToRepository() {
    // Arrange
    var newUser = User.Create(
      "username", 
      "user@example.com",
      new PasswordHasher<string>().HashPassword("username", "password"));
    var mockUserRepository = new Mock<IUserRepository>();
    mockUserRepository
      .Setup(x => x.GetByEmail(newUser.Email))
      .Returns(null as User);
    var mockJwtGenerator = new Mock<IJwtGenerator>();
    mockJwtGenerator
      .Setup(x => x.GenerateToken(newUser))
      .Returns("token");
    var handler = new RegisterCommandHandler(mockJwtGenerator.Object, mockUserRepository.Object);
    // Act
    var authResult = await handler.Handle(
      new RegisterCommand(newUser.Username, newUser.Email, "password"),
      new CancellationToken());
    // Assert
    authResult.IsError.Should().BeFalse();
    mockUserRepository.Verify(x => x.Create(It.Is<User>(u =>
      u.Username == newUser.Username && u.Email == newUser.Email)));
  }

  [Fact]
  public async void Register_WithExistingEmail_ReturnsDuplicateEmailError() {
    // Arrange
    var existingUser = User.Create(
      "username", 
      "user@example.com",
      new PasswordHasher<string>().HashPassword("username", "password"));
    var mockUserRepository = new Mock<IUserRepository>();
    mockUserRepository
      .Setup(x => x.GetByEmail("user@example.com"))
      .Returns(existingUser);
    var mockJwtGenerator = new Mock<IJwtGenerator>();
    var handler = new RegisterCommandHandler(mockJwtGenerator.Object, mockUserRepository.Object);
    // Act
    var authResult = await handler.Handle(
      new RegisterCommand("username", "user@example.com", "password"),
      new CancellationToken());
    // Assert
    authResult.IsError.Should().BeTrue();
    authResult.FirstError.Should().Be(Errors.Authentication.DuplicateEmail);
  }

  [Fact]
  public async void Register_WithExistingUsername_ThrowsException() {
    // Arrange
    var existingUser = User.Create(
      "username", 
      "user@example.com",
      new PasswordHasher<string>().HashPassword("username", "password"));
    var mockUserRepository = new Mock<IUserRepository>();
    mockUserRepository
      .Setup(x => x.GetByUsername("username"))
      .Returns(existingUser);
    var mockJwtGenerator = new Mock<IJwtGenerator>();
    var handler = new RegisterCommandHandler(mockJwtGenerator.Object, mockUserRepository.Object);
    // Act
    var authResult = await handler.Handle(
      new RegisterCommand("username", "user@example.com", "password"),
      new CancellationToken());
    // Assert
    authResult.IsError.Should().BeTrue();
    authResult.FirstError.Should().Be(Errors.Authentication.DuplicateUsername);
  }
}