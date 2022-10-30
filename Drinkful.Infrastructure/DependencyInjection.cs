using Drinkful.Application.Common.Interfaces.Persistence;
using Drinkful.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Drinkful.Infrastructure;

public static class DependencyInjection {
  public static IServiceCollection AddInfrastructure(this IServiceCollection services) {
    services.AddScoped<IUserRepository, UserRepository>();
    return services;
  }
}