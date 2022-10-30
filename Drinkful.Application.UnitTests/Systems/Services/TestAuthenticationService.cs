using Drinkful.Application.Common.Interfaces.Authentication;
using Drinkful.Application.Common.Interfaces.Persistence;
using Drinkful.Application.Services.Authentication;
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
    authResult.Token.Should().NotBeNullOrEmpty();
    authResult.Token.Should().Be("token");
  }

  [Fact]
  public void Login_WithoutExistingUser_ThrowsException() {
    // Arrange
    var mockUserRepository = new Mock<IUserRepository>();
    mockUserRepository
      .Setup(x => x.GetByEmail("user@example.com"))
      .Returns(null as User);
    var mockJwtGenerator = new Mock<IJwtGenerator>();
    var service = new AuthenticationService(mockJwtGenerator.Object, mockUserRepository.Object);
    // Act
    var act = () => service.Login("user@example.com", "password");
    // Assert
    act.Should().Throw<Exception>().WithMessage("User with the given email does not exist.");
  }

  [Fact]
  public void Login_WithInvalidPassword_ThrowsException() {
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
    var act = () => service.Login("user@example.com", "invalid-password");
    // Assert
    act.Should().Throw<Exception>().WithMessage("Invalid password.");
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
    authResult.Token.Should().NotBeNullOrEmpty();
    authResult.Token.Should().Be("token");
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
    mockUserRepository.Verify(x => x.Add(It.Is<User>(u =>
      u.Username == newUser.Username && u.Email == newUser.Email)));
  }

  [Fact]
  public void Register_WithExistingEmail_ThrowsException() {
    // Arrange
    var mockUserRepository = new Mock<IUserRepository>();
    mockUserRepository
      .Setup(x => x.GetByEmail("user@example.com"))
      .Returns(new User());
    var mockJwtGenerator = new Mock<IJwtGenerator>();
    var service = new AuthenticationService(mockJwtGenerator.Object, mockUserRepository.Object);
    // Act
    var act = () => service.Register("username", "user@example.com", "password");
    // Assert
    act.Should().Throw<Exception>().WithMessage("User with the given email already exists.");
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
    var act = () => service.Register("username", "user@example.com", "password");
    // Assert
    act.Should().Throw<Exception>().WithMessage("User with the given username already exists.");
  }
}