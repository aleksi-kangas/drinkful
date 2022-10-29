using Drinkful.API.Controllers;
using Drinkful.Application.Services.Authentication;
using Drinkful.Contracts.Authentication;
using Drinkful.Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Drinkful.API.UnitTests.Systems.Controllers;

public class TestAuthenticationController {
  [Fact]
  public void Login_OnSuccessReturns200() {
    // Arrange
    var mockAuthenticationService = new Mock<IAuthenticationService>();
    mockAuthenticationService
      .Setup(x => x.Login("user@example.com", "password"))
      .Returns(new AuthenticationResult(new User(), "token"));
    var controller = new AuthenticationController(mockAuthenticationService.Object);
    // Act
    var request = new LoginRequest("user@example.com", "password");
    var result = controller.Login(request);
    // Assert
    result.Should().BeOfType<OkObjectResult>();
    var okResult = (OkObjectResult)result;
    okResult.StatusCode.Should().Be(200);
  }

  [Fact]
  public void Register_OnSuccessReturns200() {
    // Arrange
    var mockAuthenticationService = new Mock<IAuthenticationService>();
    mockAuthenticationService
      .Setup(x => x.Register("username", "user@example.com", "password"))
      .Returns(new AuthenticationResult(new User(), "token"));
    var controller = new AuthenticationController(mockAuthenticationService.Object);
    // Act
    var request = new RegisterRequest("username", "user@example.com", "password");
    var result = controller.Register(request);
    // Assert
    result.Should().BeOfType<OkObjectResult>();
    var okResult = (OkObjectResult)result;
    okResult.StatusCode.Should().Be(200);
  }
}