using Drinkful.Application.Authentication.Commands;
using Drinkful.Application.Authentication.Common;
using Drinkful.Application.Authentication.Queries;
using Drinkful.Contracts.Authentication;
using Mapster;

namespace Drinkful.API.Common.Mapping;

public class AuthenticationMappingConfig : IRegister {
  public void Register(TypeAdapterConfig config) {
    config.NewConfig<LoginRequest, LoginQuery>();
    config.NewConfig<RegisterRequest, RegisterCommand>();
    config
      .NewConfig<AuthenticationResult, AuthenticationResponse>()
      .Map(dest => dest.Token, src => src.Token)
      .Map(dest => dest.Id, src => src.User.Id.Value)
      .Map(dest => dest, src => src.User);
  }
}
