using Drinkful.Domain.Entities;

namespace Drinkful.Application.Authentication.Common; 

public record AuthenticationResult(User User, string Token);