using Drinkful.Application.Authentication.Common;
using ErrorOr;
using MediatR;

namespace Drinkful.Application.Authentication.Queries;

public record LoginQuery(string Email, string Password) : IRequest<ErrorOr<AuthenticationResult>>;