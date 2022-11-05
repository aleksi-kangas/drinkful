using Drinkful.Application.Authentication.Commands;
using Drinkful.Application.Authentication.Queries;
using Drinkful.Contracts.Authentication;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Drinkful.API.Controllers;

[Route("auth")]
[AllowAnonymous]
public class AuthenticationController : ApiController {
  private readonly ISender _sender;
  private readonly IMapper _mapper;

  public AuthenticationController(ISender sender, IMapper mapper) {
    _sender = sender;
    _mapper = mapper;
  }

  [HttpPost("login")]
  public async Task<IActionResult> Login(LoginRequest request) {
    var loginQuery = _mapper.Map<LoginQuery>(request);
    var authResult = await _sender.Send(loginQuery);
    return authResult.Match(
      onValue: result => Ok(_mapper.Map<AuthenticationResponse>(result)),
      onError: Problem);
  }

  [HttpPost("register")]
  public async Task<IActionResult> Register(RegisterRequest request) {
    var registerCommand = _mapper.Map<RegisterCommand>(request);
    var authResult = await _sender.Send(registerCommand);
    return authResult.Match(
      onValue: result => Ok(_mapper.Map<AuthenticationResponse>(result)),
      onError: Problem);
  }
}