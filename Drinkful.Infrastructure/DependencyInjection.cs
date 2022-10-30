using Drinkful.Application.Common.Interfaces.Authentication;
using Drinkful.Application.Common.Interfaces.Persistence;
using Drinkful.Infrastructure.Authentication;
using Drinkful.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Drinkful.Infrastructure;

public static class DependencyInjection {
  public static IServiceCollection AddInfrastructure(
    this IServiceCollection services,
    ConfigurationManager configurationManager) {
    services.Configure<JwtSettings>(configurationManager.GetSection(JwtSettings.SectionName));
    services.AddSingleton<IJwtGenerator, JwtGenerator>();
    services.AddScoped<IUserRepository, UserRepository>();
    return services;
  }
}