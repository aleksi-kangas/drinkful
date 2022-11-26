using Drinkful.Application.Authentication.Common;
using ErrorOr;
using MediatR;

namespace Drinkful.Application.Authentication.Commands;

public record RegisterCommand(
  string Username,
  string Email,
  string Password) : IRequest<ErrorOr<AuthenticationResult>>;
