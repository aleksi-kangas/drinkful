using Drinkful.API.Controllers;
using Drinkful.API.UnitTests.Fixtures;
using Drinkful.Application.Authentication.Commands;
using Drinkful.Application.Authentication.Common;
using Drinkful.Application.Authentication.Queries;
using Drinkful.Contracts.Authentication;
using FluentAssertions;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Drinkful.API.UnitTests.Systems.Controllers;

public class TestAuthenticationController {
  private readonly Mock<ISender> _mediatorMock;
  private readonly Mock<IMapper> _mapperMock;

  public TestAuthenticationController() {
    _mediatorMock = new Mock<ISender>();
    _mapperMock = new Mock<IMapper>();
  }

  [Fact]
  public async void Login_OnSuccessReturns200() {
    // Arrange
    var testUser = AuthenticationFixture.GetTestUser();
    var loginQuery = new LoginQuery(testUser.Email, testUser.Password);
    _mapperMock.Setup(x => x.Map<LoginQuery>(It.IsAny<LoginRequest>()))
      .Returns(loginQuery);
    _mediatorMock.Setup(x => x.Send(loginQuery, It.IsAny<CancellationToken>()))
      .ReturnsAsync(AuthenticationFixture.GetAuthenticationResultSuccess(testUser));
    _mapperMock.Setup(x => x.Map<AuthenticationResponse>(It.IsAny<AuthenticationResult>()))
      .Returns(AuthenticationFixture.GetAuthenticationResponseSuccess(testUser));
    var controller = new AuthenticationController(_mediatorMock.Object, _mapperMock.Object);
    // Act
    var request = new LoginRequest(testUser.Email, testUser.Password);
    var result = await controller.Login(request);
    // Assert
    result.Should().BeOfType<OkObjectResult>();
    var okResult = (OkObjectResult)result;
    okResult.StatusCode.Should().Be(200);
  }

  [Fact]
  public async void Login_InvokesMediator() {
    // Arrange
    var testUser = AuthenticationFixture.GetTestUser();
    _mapperMock.Setup(x => x.Map<LoginQuery>(It.IsAny<LoginRequest>()))
      .Returns(new LoginQuery(testUser.Email, testUser.Password));
    var controller = new AuthenticationController(_mediatorMock.Object, _mapperMock.Object);
    // Act
    await controller.Login(new LoginRequest(testUser.Email, testUser.Password));
    // Assert
    _mediatorMock.Verify(x => x.Send(It.IsAny<LoginQuery>(), It.IsAny<CancellationToken>()),
      Times.Once);
  }

  [Fact]
  public async void Login_InvokesMapperForRequest() {
    // Arrange
    var testUser = AuthenticationFixture.GetTestUser();
    var controller = new AuthenticationController(_mediatorMock.Object, _mapperMock.Object);
    // Act
    await controller.Login(new LoginRequest(testUser.Email, testUser.Password));
    // Assert
    _mapperMock.Verify(x => x.Map<LoginQuery>(It.IsAny<LoginRequest>()),
      Times.Once);
  }

  [Fact]
  public async void Login_InvokesMapperForResponse() {
    // Arrange
    var testUser = AuthenticationFixture.GetTestUser();
    var controller = new AuthenticationController(_mediatorMock.Object, _mapperMock.Object);
    // Act
    await controller.Login(new LoginRequest(testUser.Email, testUser.Password));
    // Assert
    _mapperMock.Verify(x => x.Map<AuthenticationResponse>(It.IsAny<AuthenticationResult>()),
      Times.Once);
  }

  [Fact]
  public async void Register_OnSuccessReturns200() {
    // Arrange
    var testUser = AuthenticationFixture.GetTestUser();
    var registerCommand = new RegisterCommand(testUser.Username, testUser.Email, testUser.Password);
    _mapperMock.Setup(x => x.Map<RegisterCommand>(It.IsAny<RegisterRequest>()))
      .Returns(registerCommand);
    _mediatorMock.Setup(x => x.Send(registerCommand, It.IsAny<CancellationToken>()))
      .ReturnsAsync(AuthenticationFixture.GetAuthenticationResultSuccess(testUser));
    _mapperMock.Setup(x => x.Map<AuthenticationResponse>(It.IsAny<AuthenticationResult>()))
      .Returns(AuthenticationFixture.GetAuthenticationResponseSuccess(testUser));
    var controller = new AuthenticationController(_mediatorMock.Object, _mapperMock.Object);
    // Act
    var result =
      await controller.Register(new RegisterRequest(testUser.Username, testUser.Email,
        testUser.Password));
    // Assert
    result.Should().BeOfType<OkObjectResult>();
    var okResult = (OkObjectResult)result;
    okResult.StatusCode.Should().Be(200);
  }
}