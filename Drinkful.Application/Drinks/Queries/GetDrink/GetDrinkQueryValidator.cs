using FluentValidation;

namespace Drinkful.Application.Drinks.Queries.GetDrink; 

public class GetDrinkQueryValidator : AbstractValidator<GetDrinkQuery> {
  public GetDrinkQueryValidator() {
    RuleFor(x => x.Id).NotEmpty();
  }
}
