using Drinkful.Domain.User;

namespace Drinkful.Application.Authentication.Common; 

public record AuthenticationResult(User User, string Token);