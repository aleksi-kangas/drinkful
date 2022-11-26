using Drinkful.Application.Drinks.Commands.CreateDrink;
using Drinkful.Contracts.Drinks;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Drinkful.API.Controllers;

[Route("drinks")]
public class DrinksController : ApiController {
  private readonly IMapper _mapper;
  private readonly ISender _sender;

  public DrinksController(IMapper mapper, ISender sender) {
    _mapper = mapper;
    _sender = sender;
  }

  [HttpPost]
  public async Task<IActionResult> CreateDrink(CreateDrinkRequest request) {
    var command = _mapper.Map<CreateDrinkCommand>(request);
    var createDrinkResult = await _sender.Send(command);
    // TODO Implement GetDrink endpoint and use it here...
    return createDrinkResult.Match(
      onValue: drink => Ok(_mapper.Map<DrinkResponse>(drink)),
      onError: Problem);
  }
}
