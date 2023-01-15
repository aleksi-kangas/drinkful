using Drinkful.Application.Drinks.Commands.CreateDrink;
using Drinkful.Application.Drinks.Queries.GetDrink;
using Drinkful.Application.Drinks.Queries.ListDrinks;
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

  [HttpGet("{id:guid}")]  // GET /drinks/{id}
  public async Task<IActionResult> GetDrinkAsync(Guid id) {
    var query = new GetDrinkQuery(id);
    var getDrinkResult = await _sender.Send(query);
    return getDrinkResult.Match(
      onValue: drink => Ok(_mapper.Map<DrinkResponse>(drink)),
      onError: Problem);
  }
  
  [HttpGet] // GET: /drinks
  public async Task<IActionResult> ListDrinksAsync() {
    var drinks = await _sender.Send(new ListDrinksQuery());
    return Ok(drinks);
  }

  [HttpPost]  // POST: /drinks
  public async Task<IActionResult> CreateDrink(CreateDrinkRequest request) {
    var command = _mapper.Map<CreateDrinkCommand>(request);
    var createDrinkResult = await _sender.Send(command);
    return createDrinkResult.Match(
      onValue: drink => Ok(_mapper.Map<DrinkResponse>(drink)),
      onError: Problem);
  }
}
