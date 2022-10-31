using Drinkful.Application.Authentication.Commands;
using Drinkful.Application.Authentication.Common;
using Drinkful.Application.Authentication.Queries;
using Drinkful.Contracts.Authentication;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Drinkful.API.Controllers;

[Route("auth")]
public class AuthenticationController : ApiController {
  private readonly ISender _sender;

  public AuthenticationController(ISender sender) {
    _sender = sender;
  }

  [HttpPost("login")]
  public async Task<IActionResult> Login(LoginRequest request) {
    var loginQuery = new LoginQuery(request.Email, request.Password);
    var authResult = await _sender.Send(loginQuery);
    return authResult.Match(
      onValue: result => Ok(MapToResponse(result)),
      onError: Problem);
  }

  [HttpPost("register")]
  public async Task<IActionResult> Register(RegisterRequest request) {
    var registerCommand = new RegisterCommand(request.Username, request.Email, request.Password);
    var authResult = await _sender.Send(registerCommand);
    return authResult.Match(
      onValue: result => Ok(MapToResponse(result)),
      onError: Problem);
  }

  private static AuthenticationResponse MapToResponse(AuthenticationResult authResult) {
    return new AuthenticationResponse(
      authResult.User.Id,
      authResult.User.Username,
      authResult.User.Email,
      authResult.Token);
  }
}