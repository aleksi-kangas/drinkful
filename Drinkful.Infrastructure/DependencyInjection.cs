using System.Text;
using Drinkful.Application.Common.Interfaces.Authentication;
using Drinkful.Application.Common.Interfaces.Persistence;
using Drinkful.Infrastructure.Authentication;
using Drinkful.Infrastructure.Persistence;
using Drinkful.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Drinkful.Infrastructure;

public static class DependencyInjection {
  public static IServiceCollection AddInfrastructure(
    this IServiceCollection services,
    ConfigurationManager configurationManager) {
    services
      .AddAuth(configurationManager)
      .AddPersistence(configurationManager);
    return services;
  }

  private static IServiceCollection AddAuth(
    this IServiceCollection services,
    ConfigurationManager configurationManager) {
    var jwtSettings = new JwtSettings();
    configurationManager.Bind(JwtSettings.SectionName, jwtSettings);

    services.AddSingleton(Options.Create(jwtSettings));
    services.AddSingleton<IJwtGenerator, JwtGenerator>();

    services
      .AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer(options =>
        options.TokenValidationParameters = new TokenValidationParameters {
          ValidateAudience = true,
          ValidateIssuer = true,
          ValidateIssuerSigningKey = true,
          ValidateLifetime = true,
          ValidAudience = jwtSettings.Audience,
          ValidIssuer = jwtSettings.Issuer,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
        });
    return services;
  }

  private static IServiceCollection AddPersistence(this IServiceCollection services,
    ConfigurationManager configurationManager) {
    services.AddDbContext<DrinkfulDbContext>(options =>
      options.UseNpgsql(configurationManager.GetConnectionString("DefaultConnection")));
    services.AddScoped<IDrinkRepository, DrinkRepository>();
    services.AddScoped<IUserRepository, UserRepository>();
    return services;
  }
}
