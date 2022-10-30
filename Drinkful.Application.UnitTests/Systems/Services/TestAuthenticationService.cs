using Drinkful.Application.Common.Interfaces.Authentication;
using Drinkful.Application.Common.Interfaces.Persistence;
using Drinkful.Application.Services.Authentication;
using Drinkful.Domain.Common.Errors;
using Drinkful.Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Drinkful.Application.UnitTests.Systems.Services;

public class TestAuthenticationService {
  [Fact]
  public void Login_WithValidCredentials_ReturnsToken() {
    // Arrange
    var passwordHasher = new PasswordHasher<string>();
    var user = new User {
      Id = Guid.NewGuid(),
      Username = "username",
      Email = "user@example.com",
      PasswordHash = passwordHasher.HashPassword("username", "password")
    };
    var mockUserRepository = new Mock<IUserRepository>();
    mockUserRepository
      .Setup(x => x.GetByEmail(user.Email))
      .Returns(user);
    var mockJwtGenerator = new Mock<IJwtGenerator>();
    mockJwtGenerator
      .Setup(x => x.GenerateToken(user))
      .Returns("token");
    var service = new AuthenticationService(mockJwtGenerator.Object, mockUserRepository.Object);
    // Act
    var authResult = service.Login(user.Email, "password");
    // Assert
    authResult.IsError.Should().BeFalse();
    authResult.Value.Token.Should().NotBeNullOrEmpty();
    authResult.Value.Token.Should().Be("token");
  }

  [Fact]
  public void Login_WithoutExistingUser_ReturnsInvalidCredentialsError() {
    // Arrange
    var mockUserRepository = new Mock<IUserRepository>();
    mockUserRepository
      .Setup(x => x.GetByEmail("user@example.com"))
      .Returns(null as User);
    var mockJwtGenerator = new Mock<IJwtGenerator>();
    var service = new AuthenticationService(mockJwtGenerator.Object, mockUserRepository.Object);
    // Act
    var authResult = service.Login("user@example.com", "password");
    // Assert
    authResult.IsError.Should().BeTrue();
    authResult.FirstError.Should().Be(Errors.Authentication.InvalidCredentials);
  }

  [Fact]
  public void Login_WithInvalidPassword_ReturnsInvalidCredentialsError() {
    // Arrange
    var user = new User {
      Id = Guid.NewGuid(),
      Username = "username",
      Email = "user@example.com",
      PasswordHash = new PasswordHasher<string>().HashPassword("username", "password")
    };
    var mockUserRepository = new Mock<IUserRepository>();
    mockUserRepository
      .Setup(x => x.GetByEmail("user@example.com"))
      .Returns(user);
    var mockJwtGenerator = new Mock<IJwtGenerator>();
    var service = new AuthenticationService(mockJwtGenerator.Object, mockUserRepository.Object);
    // Act
    var authResult = service.Login("user@example.com", "invalid-password");
    // Assert
    authResult.IsError.Should().BeTrue();
    authResult.FirstError.Should().Be(Errors.Authentication.InvalidCredentials);
  }

  [Fact]
  public void Register_WithValidUserInformation_ReturnsToken() {
    // Arrange
    var newUser = new User {
      Id = Guid.NewGuid(),
      Username = "username",
      Email = "user@example.com",
      PasswordHash = new PasswordHasher<string>().HashPassword("username", "password")
    };
    var mockUserRepository = new Mock<IUserRepository>();
    mockUserRepository
      .Setup(x => x.GetByEmail(newUser.Email))
      .Returns(null as User);
    var mockJwtGenerator = new Mock<IJwtGenerator>();
    mockJwtGenerator
      .Setup(x => x.GenerateToken(It.IsAny<User>()))
      .Returns("token");
    var service = new AuthenticationService(mockJwtGenerator.Object, mockUserRepository.Object);
    // Act
    var authResult = service.Register(newUser.Username, newUser.Email, "password");
    // Assert
    authResult.IsError.Should().BeFalse();
    authResult.Value.Token.Should().NotBeNullOrEmpty();
    authResult.Value.Token.Should().Be("token");
  }

  [Fact]
  public void Register_WithoutExistingUser_AddsUserToRepository() {
    // Arrange
    var newUser = new User {
      Id = Guid.NewGuid(),
      Username = "username",
      Email = "user@example.com",
      PasswordHash = "password"
    };
    var mockUserRepository = new Mock<IUserRepository>();
    mockUserRepository
      .Setup(x => x.GetByEmail(newUser.Email))
      .Returns(null as User);
    var mockJwtGenerator = new Mock<IJwtGenerator>();
    mockJwtGenerator
      .Setup(x => x.GenerateToken(newUser))
      .Returns("token");
    var service = new AuthenticationService(mockJwtGenerator.Object, mockUserRepository.Object);
    // Act
    var authResult = service.Register(newUser.Username, newUser.Email, newUser.PasswordHash);
    // Assert
    authResult.IsError.Should().BeFalse();
    mockUserRepository.Verify(x => x.Create(It.Is<User>(u =>
      u.Username == newUser.Username && u.Email == newUser.Email)));
  }

  [Fact]
  public void Register_WithExistingEmail_ReturnsDuplicateEmailError() {
    // Arrange
    var mockUserRepository = new Mock<IUserRepository>();
    mockUserRepository
      .Setup(x => x.GetByEmail("user@example.com"))
      .Returns(new User());
    var mockJwtGenerator = new Mock<IJwtGenerator>();
    var service = new AuthenticationService(mockJwtGenerator.Object, mockUserRepository.Object);
    // Act
    var authResult = service.Register("username", "user@example.com", "password");
    // Assert
    authResult.IsError.Should().BeTrue();
    authResult.FirstError.Should().Be(Errors.Authentication.DuplicateEmail);
  }

  [Fact]
  public void Register_WithExistingUsername_ThrowsException() {
    // Arrange
    var mockUserRepository = new Mock<IUserRepository>();
    mockUserRepository
      .Setup(x => x.GetByUsername("username"))
      .Returns(new User());
    var mockJwtGenerator = new Mock<IJwtGenerator>();
    var service = new AuthenticationService(mockJwtGenerator.Object, mockUserRepository.Object);
    // Act
    var authResult = service.Register("username", "user@example.com", "password");
    // Assert
    authResult.IsError.Should().BeTrue();
    authResult.FirstError.Should().Be(Errors.Authentication.DuplicateUsername);
  }
}