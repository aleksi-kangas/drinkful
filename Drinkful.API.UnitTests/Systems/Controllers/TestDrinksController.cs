using Drinkful.API.Controllers;
using Drinkful.Application.Drinks.Commands.CreateDrink;
using Drinkful.Contracts.Drinks;
using Drinkful.Domain.Drink;
using Drinkful.Domain.User.ValueObjects;
using FluentAssertions;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Drinkful.API.UnitTests.Systems.Controllers;

public class TestDrinksController {
  private readonly Mock<ISender> _mediatorMock;
  private readonly Mock<IMapper> _mapperMock;
  private readonly DrinksController _controller;

  public TestDrinksController() {
    _mediatorMock = new Mock<ISender>();
    _mapperMock = new Mock<IMapper>();
    _controller = new DrinksController(_mapperMock.Object, _mediatorMock.Object);
  }

  private static Drink _sampleDrink = Drink.Create("Cappucino", "Summary...", "Description...",
    "https://example.com/image.jpg", UserId.CreateUnique());

  [Fact]
  public async void CreateDrink_OnSuccess_Returns200() {
    // TODO Update status code test, once proper CreatedAtAction is returned.
    // Arrange
    var command = new CreateDrinkCommand(_sampleDrink.Name, _sampleDrink.Summary, _sampleDrink.Description,
      _sampleDrink.ImageUrl, _sampleDrink.AuthorId.Value.ToString());
    _mapperMock.Setup(x => x.Map<CreateDrinkCommand>(It.IsAny<CreateDrinkRequest>()))
      .Returns(command);
    _mediatorMock.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
      .ReturnsAsync(Drink.Create("name", "summary", "description", "image-url", UserId.CreateUnique()));
    _mapperMock.Setup(x => x.Map<DrinkResponse>(It.IsAny<Drink>()))
      .Returns(new DrinkResponse(_sampleDrink.Id.Value.ToString(), _sampleDrink.Name, _sampleDrink.Summary,
        _sampleDrink.Description, _sampleDrink.ImageUrl, _sampleDrink.CreatedAt, _sampleDrink.UpdatedAt,
        _sampleDrink.AuthorId.Value.ToString(),
        new List<DrinkCommentResponse>()));
    // Act
    var request = new CreateDrinkRequest("name", "summary", "description", "image-url", "author-id");
    var result = await _controller.CreateDrink(request);
    // Assert
    result.Should().BeOfType<OkObjectResult>();
    var okResult = (OkObjectResult) result;
    okResult.StatusCode.Should().Be(200);
  }

  [Fact]
  public async void CreateDrink_Invokes_Mediator() {
    // Arrange
    var request = new CreateDrinkRequest(_sampleDrink.Name, _sampleDrink.Summary, _sampleDrink.Description,
      _sampleDrink.ImageUrl, _sampleDrink.AuthorId.Value.ToString());
    _mapperMock.Setup(x => x.Map<CreateDrinkCommand>(It.IsAny<CreateDrinkRequest>()))
      .Returns(new CreateDrinkCommand(_sampleDrink.Name, _sampleDrink.Summary, _sampleDrink.Description,
        _sampleDrink.ImageUrl, _sampleDrink.AuthorId.Value.ToString()));
    // Act
    await _controller.CreateDrink(request);
    _mediatorMock.Verify(x => x.Send(It.IsAny<CreateDrinkCommand>(), It.IsAny<CancellationToken>()),
      Times.Once);
  }

  [Fact]
  public async void CreateDrink_Invokes_MapperForRequest() {
    // Arrange
    var request = new CreateDrinkRequest(_sampleDrink.Name, _sampleDrink.Summary, _sampleDrink.Description,
      _sampleDrink.ImageUrl, _sampleDrink.AuthorId.Value.ToString());
    // Act
    await _controller.CreateDrink(request);
    // Assert
    _mapperMock.Verify(x => x.Map<CreateDrinkCommand>(It.IsAny<CreateDrinkRequest>()),
      Times.Once);
  }

  [Fact]
  public async void CreateDrink_Invokes_MapperForResponse() {
    // Arrange
    // Act
    await _controller.CreateDrink(new CreateDrinkRequest(_sampleDrink.Name, _sampleDrink.Summary,
      _sampleDrink.Description, _sampleDrink.ImageUrl, _sampleDrink.AuthorId.Value.ToString()));
    // Assert
    _mapperMock.Verify(x => x.Map<DrinkResponse>(It.IsAny<Drink>()),
      Times.Once);
  }
}
