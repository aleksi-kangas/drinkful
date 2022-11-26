using Drinkful.API.Common.Errors;
using Drinkful.API.Common.Mapping;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Drinkful.API;

public static class DependencyInjection {
  public static IServiceCollection AddPresentation(this IServiceCollection services) {
    services.AddControllers();
    services.AddSingleton<ProblemDetailsFactory, DrinkfulProblemDetailsFactory>();
    services.AddMappings();
    return services;
  }
}
