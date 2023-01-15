using Drinkful.Application.Drinks.Commands.CreateDrink;
using Drinkful.Contracts.Drinks;
using Drinkful.Domain.Drink;
using Drinkful.Domain.Drink.Entities;
using Mapster;

namespace Drinkful.API.Common.Mapping;

public class DrinkMappingConfig : IRegister {
  public void Register(TypeAdapterConfig config) {
    config.NewConfig<CreateDrinkRequest, CreateDrinkCommand>();

    config.NewConfig<Drink, DrinkResponse>()
      .Map(dest => dest.Id, src => src.Id.Value)
      .Map(dest => dest.AuthorId, src => src.AuthorId.Value);

    config.NewConfig<DrinkComment, DrinkCommentResponse>()
      .Map(dest => dest.Id, src => src.Id.Value)
      .Map(dest => dest.AuthorId, src => src.AuthorId.Value);
  }
}
