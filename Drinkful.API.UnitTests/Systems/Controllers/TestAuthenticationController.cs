using Drinkful.API.Controllers;
using Drinkful.Application.Authentication.Commands;
using Drinkful.Application.Authentication.Common;
using Drinkful.Application.Authentication.Queries;
using Drinkful.Contracts.Authentication;
using Drinkful.Domain.Entities;
using ErrorOr;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Drinkful.API.UnitTests.Systems.Controllers;

public class TestAuthenticationController {
  [Fact]
  public async void Login_OnSuccessReturns200() {
    // Arrange
    var mockSender = new Mock<ISender>();
    var loginQuery = new LoginQuery("user@example.com", "password");
    ErrorOr<AuthenticationResult> authResult = new AuthenticationResult(new User(), "token");
    mockSender.Setup(x => x.Send(loginQuery, It.IsAny<CancellationToken>()))
      .Returns(Task.FromResult(authResult));
    var controller = new AuthenticationController(mockSender.Object);
    // Act
    var request = new LoginRequest("user@example.com", "password");
    var result = await controller.Login(request);
    // Assert
    result.Should().BeOfType<OkObjectResult>();
    var okResult = (OkObjectResult)result;
    okResult.StatusCode.Should().Be(200);
  }

  [Fact]
  public async void Register_OnSuccessReturns200() {
    // Arrange
    var mockSender = new Mock<ISender>();
    var registerCommand = new RegisterCommand("username", "user@example.com", "password");
    ErrorOr<AuthenticationResult> authResult = new AuthenticationResult(new User(), "token");
    mockSender.Setup(x => x.Send(registerCommand, It.IsAny<CancellationToken>()))
      .Returns(Task.FromResult(authResult));
    var controller = new AuthenticationController(mockSender.Object);
    // Act
    var request = new RegisterRequest("username", "user@example.com", "password");
    var result = await controller.Register(request);
    // Assert
    result.Should().BeOfType<OkObjectResult>();
    var okResult = (OkObjectResult)result;
    okResult.StatusCode.Should().Be(200);
  }
}