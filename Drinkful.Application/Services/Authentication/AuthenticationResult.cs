using Drinkful.Domain.Entities;

namespace Drinkful.Application.Services.Authentication; 

public record AuthenticationResult(User User, string Token);