using Drinkful.API.Controllers;
using Drinkful.Application.Authentication.Commands;
using Drinkful.Application.Authentication.Common;
using Drinkful.Application.Authentication.Queries;
using Drinkful.Contracts.Authentication;
using Drinkful.Domain.Entities;
using ErrorOr;
using FluentAssertions;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Drinkful.API.UnitTests.Systems.Controllers;

public class TestAuthenticationController {
  [Fact]
  public async void Login_OnSuccessReturns200() {
    // Arrange
    var loginQuery = new LoginQuery("user@example.com", "password");
    ErrorOr<AuthenticationResult> authResult = new AuthenticationResult(new User(), "token");
    var mockSender = new Mock<ISender>();
    mockSender.Setup(x => x.Send(loginQuery, It.IsAny<CancellationToken>()))
      .Returns(Task.FromResult(authResult));

    var mockMapper = new Mock<IMapper>();
    mockMapper.Setup(x => x
      .Map<LoginRequest, LoginQuery>(It.IsAny<LoginRequest>()))
      .Returns(loginQuery);
    mockMapper.Setup(x => x
      .Map<AuthenticationResult, AuthenticationResponse>(It.IsAny<AuthenticationResult>()))
      .Returns(new AuthenticationResponse(Guid.NewGuid(), "", "", ""));

    var controller = new AuthenticationController(mockSender.Object, mockMapper.Object);
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
    var registerCommand = new RegisterCommand("username", "user@example.com", "password");
    ErrorOr<AuthenticationResult> authResult = new AuthenticationResult(new User(), "token");
    var mockSender = new Mock<ISender>();
    mockSender.Setup(x => x.Send(registerCommand, It.IsAny<CancellationToken>()))
      .Returns(Task.FromResult(authResult));
    
    var mockMapper = new Mock<IMapper>();
    mockMapper.Setup(x => x
        .Map<RegisterRequest, RegisterCommand>(It.IsAny<RegisterRequest>()))
      .Returns(registerCommand);
    mockMapper.Setup(x => x
        .Map<AuthenticationResult, AuthenticationResponse>(It.IsAny<AuthenticationResult>()))
      .Returns(new AuthenticationResponse(Guid.NewGuid(), "", "", ""));

    var controller = new AuthenticationController(mockSender.Object, mockMapper.Object);
    // Act
    var request = new RegisterRequest("username", "user@example.com", "password");
    var result = await controller.Register(request);
    // Assert
    result.Should().BeOfType<OkObjectResult>();
    var okResult = (OkObjectResult)result;
    okResult.StatusCode.Should().Be(200);
  }
}