using Drinkful.Application.Services.Authentication;
using Drinkful.Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Drinkful.API.Controllers;

[ApiController]
[Route("auth")]
public class AuthenticationController : ControllerBase {
  private readonly IAuthenticationService _authenticationService;

  public AuthenticationController(IAuthenticationService authenticationService) {
    _authenticationService = authenticationService;
  }

  [HttpPost("login")]
  public IActionResult Login(LoginRequest request) {
    var authResult = _authenticationService.Login(request.Email, request.Password);
    var response = MapToResponse(authResult);
    return Ok(response);
  }

  [HttpPost("register")]
  public IActionResult Register(RegisterRequest request) {
    var authResult =
      _authenticationService.Register(request.Username, request.Email, request.Password);
    var response = MapToResponse(authResult);
    return Ok(response);
  }
  private AuthenticationResponse MapToResponse(AuthenticationResult authResult) {
    return new AuthenticationResponse(
      authResult.User.Id,
      authResult.User.Username,
      authResult.User.Email,
      authResult.Token);
  }
}