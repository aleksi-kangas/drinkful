using Drinkful.Application.Services.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Drinkful.Application; 

public static class DependencyInjection {
  public static IServiceCollection AddApplication(this IServiceCollection services) {
    services.AddScoped<IAuthenticationService, AuthenticationService>();
    return services;
  }
}