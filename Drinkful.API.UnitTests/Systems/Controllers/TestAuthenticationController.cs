using Drinkful.API.Controllers;
using Drinkful.Application.Authentication.Commands;
using Drinkful.Application.Authentication.Common;
using Drinkful.Application.Authentication.Queries;
using Drinkful.Contracts.Authentication;
using Drinkful.Domain.Common.Errors;
using Drinkful.Domain.User;
using ErrorOr;
using FluentAssertions;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Drinkful.API.UnitTests.Systems.Controllers;

public class TestAuthenticationController {
  private readonly Mock<ISender> _mediatorMock;
  private readonly Mock<IMapper> _mapperMock;
  private readonly AuthenticationController _controller;

  public TestAuthenticationController() {
    _mediatorMock = new Mock<ISender>();
    _mapperMock = new Mock<IMapper>();
    _controller = new AuthenticationController(_mediatorMock.Object, _mapperMock.Object);
  }

  [Fact]
  public async void Login_OnSuccess_Returns200() {
    // Arrange
    var loginQuery = new LoginQuery("user@example.com", "password");
    _mapperMock.Setup(x => x.Map<LoginQuery>(It.IsAny<LoginRequest>()))
      .Returns(loginQuery);
    _mediatorMock.Setup(x => x.Send(loginQuery, It.IsAny<CancellationToken>()))
      .ReturnsAsync(new AuthenticationResult(User.Create("username", "email", "passwordHash"), "token"));
    _mapperMock.Setup(x => x.Map<AuthenticationResponse>(It.IsAny<AuthenticationResult>()))
      .Returns(new AuthenticationResponse(Guid.NewGuid(), "username", "email", "token"));
    // Act
    var request = new LoginRequest("username", "password");
    var result = await _controller.Login(request);
    // Assert
    result.Should().BeOfType<OkObjectResult>();
    var okResult = (OkObjectResult)result;
    okResult.StatusCode.Should().Be(200);
  }

  [Fact]
  public async void Login_Invokes_Mediator() {
    // Arrange
    _mapperMock.Setup(x => x.Map<LoginQuery>(It.IsAny<LoginRequest>()))
      .Returns(new LoginQuery("user@example.com", "password"));
    // Act
    await _controller.Login(new LoginRequest("user@example.com", "password"));
    _mediatorMock.Verify(x => x.Send(It.IsAny<LoginQuery>(), It.IsAny<CancellationToken>()),
      Times.Once);
  }

  [Fact]
  public async void Login_Invokes_MapperForRequest() {
    // Arrange
    // Act
    await _controller.Login(new LoginRequest("user@example.com", "password"));
    // Assert
    _mapperMock.Verify(x => x.Map<LoginQuery>(It.IsAny<LoginRequest>()),
      Times.Once);
  }

  [Fact]
  public async void Login_Invokes_MapperForResponse() {
    // Arrange
    // Act
    await _controller.Login(new LoginRequest("user@example.com", "password"));
    // Assert
    _mapperMock.Verify(x => x.Map<AuthenticationResponse>(It.IsAny<AuthenticationResult>()),
      Times.Once);
  }

  [Fact]
  public async void Login_OnWrongCredentials_HasValidationProblemDetails() {
    // Arrange
    var loginQuery = new LoginQuery("user@example.com", "wrong-password");
    _mapperMock.Setup(x => x.Map<LoginQuery>(It.IsAny<LoginRequest>()))
      .Returns(loginQuery);
    _mediatorMock.Setup(x => x.Send(loginQuery, It.IsAny<CancellationToken>()))
      .ReturnsAsync(Errors.Authentication.InvalidCredentials);
    // Act
    var result = await _controller.Login(new LoginRequest("user@example.com", "wrong-password"));
    // Assert
    result.Should().BeOfType<ObjectResult>();
    var objectResult = (ObjectResult)result;
    objectResult.Value.Should().BeOfType<ValidationProblemDetails>();
    var validationProblemDetails = (ValidationProblemDetails)objectResult.Value!;
    validationProblemDetails.Errors.Should().HaveCount(1);
    validationProblemDetails.Errors[Errors.Authentication.InvalidCredentials.Code].First().Should()
      .Be(Errors.Authentication.InvalidCredentials.Description);
  }

  [Fact]
  public async void Register_OnSuccess_Returns200() {
    // Arrange
    var registerCommand = new RegisterCommand("username", "user@example.com", "password");
    _mapperMock.Setup(x => x.Map<RegisterCommand>(It.IsAny<RegisterRequest>()))
      .Returns(registerCommand);
    _mediatorMock.Setup(x => x.Send(registerCommand, It.IsAny<CancellationToken>()))
      .ReturnsAsync(new AuthenticationResult(User.Create("username", "email", "passwordHash"), "token"));
    _mapperMock.Setup(x => x.Map<AuthenticationResponse>(It.IsAny<AuthenticationResult>()))
      .Returns(new AuthenticationResponse(Guid.NewGuid(), "username", "email@example.com", "token"));
    // Act
    var result = await _controller.Register(new RegisterRequest("username", "email", "password"));
    // Assert
    result.Should().BeOfType<OkObjectResult>();
    var okResult = (OkObjectResult)result;
    okResult.StatusCode.Should().Be(200);
  }
}